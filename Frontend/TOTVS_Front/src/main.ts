import { bootstrapApplication } from '@angular/platform-browser';
import { importProvidersFrom } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ProductListComponent } from './application/components/product-list/product-list.component';


bootstrapApplication(ProductListComponent, {
  providers: [
    importProvidersFrom(HttpClientModule, FormsModule)
  ]
}).catch(err => console.error(err));