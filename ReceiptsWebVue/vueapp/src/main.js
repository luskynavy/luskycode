import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'

import i18n from './i18n'

import 'bootstrap-icons/font/bootstrap-icons.css'

import PrimeVue from 'primevue/config';

// Vuetify
//import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

const app = createApp(App)

app.use(createPinia())

app.use(router)

// Explicitly initialize the i18n library
i18n.setup()

// Pass the VueI18n instance as a plugin to use()
app.use(i18n.vueI18n)

//Init PrimeVue
app.use(PrimeVue, { /* options */ });

//Init Vuetify
const vuetify = createVuetify({
    components,
    directives,
    theme: { defaultTheme: 'light' }
})
app.use(vuetify)

app.mount('#app')