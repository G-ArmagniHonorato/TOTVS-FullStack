export interface CreateProductDto {
  name: string;
  description: string;
  sku: string;
  price: number;
  image?: string;
  excluido: boolean;
}