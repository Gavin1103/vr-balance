import { defineStore } from 'pinia'
import { CookieService } from '@/service/CookieService.ts'
import { UserService } from '@/service/UserService.ts'
import type { UserProfileResponse } from '@/DTO/response/UserProfileResponse.ts'

const cookieService = new CookieService()
const userService = new UserService()

export const useAuthenticationStore = defineStore('auth', {
  state: () => ({
    isLoggedIn: false,
    role: null as null | 'PATIENT' | 'PHYSIOTHERAPIST' | 'ADMIN',
    user: null as UserProfileResponse | null
  }),
  actions: {
    async checkAuthorization() {
      this.isLoggedIn = cookieService.tokenExists()

      if (this.isLoggedIn) {
        const response = await userService.getLoggedInUser()
        this.role = response.data.role
        this.user = response.data
      }
    },
    logout() {
      cookieService.removeTokenCookies()
      this.isLoggedIn = false
      this.role = null
      this.user = null
    },
  },
})
