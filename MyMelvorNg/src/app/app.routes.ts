import { Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { InventoryComponent } from './inventory/inventory.component';
import { WoodcuttingComponent } from './woodcutting/woodcutting.component';
import { FishingComponent } from './fishing/fishing.component';
import { CookingComponent } from './cooking/cooking.component';



export const routes: Routes = [
    {
        path: '',
        title: 'Home',
        component: HomeComponent,
    },
    {
        path: 'inventory',
        title: 'Inventory',
        component: InventoryComponent,
    },
    {
        path: 'woodcutting',
        title: 'Woodcutting',
        component: WoodcuttingComponent,
    },
    {
        path: 'fishing',
        title: 'Fishing',
        component: FishingComponent,
    },
    {
        path: 'cooking',
        title: 'Cooking',
        component: CookingComponent,
    },
];
