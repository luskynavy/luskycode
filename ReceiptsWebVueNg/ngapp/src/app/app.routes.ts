import { Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { ProductsComponent } from './products/products.component';
import { GroupProductsComponent } from './group-products/group-products.component';
import { DetailsComponent } from './details/details.component';

export const routes: Routes = [
    {
        path: '',
        title: 'Home',
        component: HomeComponent,
    },
    {
        path: 'groupproducts',
        title: 'Group Products',
        component: GroupProductsComponent,
    },
    {
        path: 'products',
        title: 'Products',
        component: ProductsComponent,
    },
    {
        path: 'details/:id',
        title: 'Details',
        component: DetailsComponent,
    },
];
