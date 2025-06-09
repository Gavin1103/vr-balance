import { createApp } from 'vue'
import { createPinia } from 'pinia'
import PrimeVue from 'primevue/config'
import App from './App.vue'
import router from './router/BaseRouter.ts'
import Aura from '@primevue/themes/aura'
import ToastService from 'primevue/toastservice'
import Toast from 'primevue/toast'
import './assets/css/Global.css'
import axios from 'axios'
import { CookieService } from '@/service/CookieService.ts'

const app = createApp(App)

const cookieService: CookieService = new CookieService()

app.use(createPinia())
app.use(PrimeVue, {
  theme: {
    preset: Aura,
    options: {
      darkModeSelector: 'none',
    },
  },
})

app.use(ToastService)
app.component('GlobalToast', Toast)
app.use(router)

// If the token is invalid -> remove token
axios.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      cookieService.removeTokenCookies()
      router.push('/').then(() => window.location.reload())
    }
    return Promise.reject(error)
  },
)

app.mount('#app')
