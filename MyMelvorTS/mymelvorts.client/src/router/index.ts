import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/',
            name: 'home',
            component: HomeView
        },
        {
            path: '/inventory',
            name: 'inventory',
            component: () => import('../views/InventoryView.vue')
        },
        {
            path: '/woodcutting',
            name: 'woodcutting',
            component: () => import('../views/WoodcuttingView.vue')
        },
        {
            path: '/cooking',
            name: 'cooking',
            component: () => import('../views/CookingView.vue')
        },
        {
            path: '/fishing',
            name: 'fishing',
            component: () => import('../views/FishingView.vue')
        },
        {
            path: '/tests',
            name: 'tests',
            component: () => import('../views/TestsView.vue')
        }
    ]
})

export default router