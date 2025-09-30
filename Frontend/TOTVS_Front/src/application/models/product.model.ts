export interface Product {
  id: number;
  name: string;
  description: string;
  sku: string;
  price: number;
  image?: string;
  excluido: boolean;
  createTs?: Date;
  modTs?: Date; 
}