import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'

import i18n from './i18n'

import 'bootstrap-icons/font/bootstrap-icons.css'

const app = createApp(App)

app.use(createPinia())

// Explicitly initialize the i18n library
i18n.setup()

// Pass the VueI18n instance as a plugin to use()
app.use(i18n.vueI18n)

app.use(router)

app.mount('#app')