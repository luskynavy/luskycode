import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/',
            name: 'home',
            //component: WeatherForecast
            component: HomeView
        },
        {
            path: '/inventory',
            name: 'inventory',
            component: () => import('../views/Inventory.vue')
        },
        {
            path: '/woodcutting',
            name: 'woodcutting',
            component: () => import('../views/Woodcutting.vue')
        },
        {
            path: '/cooking',
            name: 'cooking',
            component: () => import('../views/Cooking.vue')
        },
        {
            path: '/tests',
            name: 'tests',
            component: () => import('../views/Tests.vue')
        }
    ]
})

export default router