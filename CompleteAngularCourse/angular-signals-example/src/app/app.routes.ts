import { Routes } from '@angular/router';
import { SignalTestComponent } from './signal-test/signal-test.component';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { TestProductComponent } from './test-product/test-product.component';

export const routes: Routes = [
    {path: "home", component:HomeComponent},
    {path: "tests", component:SignalTestComponent},
    {path: "testProduct", component:TestProductComponent}
];
