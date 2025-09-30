export interface UpdateProductDto {
  id: number;
  name: string;
  description: string;
  sku: string;
  price: number;
  image?: string;
  excluido: boolean;
}