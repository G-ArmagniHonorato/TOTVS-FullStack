import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { CommonModule } from '@angular/common';
import { Product } from '../../models/product.model';
import { ProductFormComponent } from '../product-form/product-form.component';
import { CreateProductDto } from '../../models/create-product.model';
import { UpdateProductDto } from '../../models/update-product.model';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, ProductFormComponent],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent {
  private readonly BASE_URL = 'https://localhost:44383';

  product: Product = this.getEmptyProduct();
productToDelete: Product | null = null;
showDeleteModal = false;
  products: Product[] = [];
  isEditing = false;
  editingId: number | null = null;
  loading = false;
  message = '';
  messageType = 'info';

  constructor(private httpClient: HttpClient) {
    this.loadProducts();
  }

  loadProducts(): void {
    this.loading = true;
    this.httpClient.get<Product[]>(`${this.BASE_URL}/api/products/GetAllProducts`).pipe(
      map(data => data)
    ).subscribe({
      next: products => {
        this.products = products;
        this.loading = false;
      },
      error: () => {
        this.showMessage('Erro ao carregar produtos.', 'danger');
        this.loading = false;
      }
    });
  }

  saveProduct(product: Product): void {
    if (this.isEditing) {
      this.updateProduct(product);
    } else {
      this.createProduct(product);
    }
  }

createProduct(productData: CreateProductDto): void {
  this.httpClient.post<Product>(`${this.BASE_URL}/api/products`, productData).subscribe({
    next: () => {
      this.showMessage('Produto criado com sucesso.', 'success');
      this.resetForm();
      this.loadProducts();
    },
    error: () => this.showMessage('Erro ao criar produto.', 'danger')
  });
}

updateProduct(productData: UpdateProductDto): void {
  this.httpClient.put<Product>(`${this.BASE_URL}/api/products/${productData.id}`, productData).subscribe({
    next: () => {
      this.showMessage('Produto atualizado com sucesso.', 'success');
      this.resetForm();
      this.loadProducts();
    },
    error: () => this.showMessage('Erro ao atualizar produto.', 'danger')
  });
}


deleteProduct(product: Product): void {
  this.productToDelete = product;
  this.showDeleteModal = true; // abre o modal
}

confirmDelete(): void {
  if (!this.productToDelete) return;

  this.httpClient.delete(`${this.BASE_URL}/api/products/${this.productToDelete.id}`).subscribe({
    next: () => {
      this.showMessage('Produto excluído com sucesso.', 'success');
      this.loadProducts();
      this.productToDelete = null;
      this.showDeleteModal = false;
    },
    error: () => this.showMessage('Erro ao excluir produto.', 'danger')
  });
}

cancelDelete(): void {
  this.productToDelete = null;
  this.showDeleteModal = false;
}

  editProduct(product: Product): void {
    this.isEditing = true;
    this.editingId = product.id;
    this.product = { ...product };
  }

  cancelEdit(): void {
    this.resetForm();
  }

  private resetForm(): void {
    this.isEditing = false;
    this.editingId = null;
    this.product = this.getEmptyProduct();
  }

  private getEmptyProduct(): Product {
    return { id: 0, name: '', description: '', sku: '', price: 0, image: '', excluido: false };
  }

private showMessage(message: string, type: 'success' | 'warning' | 'danger' | 'info'): void {
  this.message = message;
  this.messageType = type;

  if (type !== 'danger') {
    setTimeout(() => this.message = '', 4000);
  }
}
}
