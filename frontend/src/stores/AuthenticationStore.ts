import { defineStore } from 'pinia'
import { CookieService } from '@/service/CookieService.ts'
import { UserService } from '@/service/UserService.ts'
import type { UserProfileResponse } from '@/DTO/response/UserProfileResponse.ts'

const cookieService = new CookieService()
const userService = new UserService()

/**
 * Pinia store for managing user authentication state and user profile.
 *
 * This store tracks whether a user is logged in, their role, and their full profile.
 * It integrates with `CookieService` to check for tokens and `UserService` to retrieve
 * the logged-in user's data.
 *
 * ## State
 * - `isLoggedIn`: `boolean` - Whether a valid JWT token is present in the cookies.
 * - `role`: `'PATIENT' | 'PHYSIOTHERAPIST' | 'ADMIN' | null` - The role of the authenticated user.
 * - `user`: `UserProfileResponse | null` - Full user profile object.
 *
 * ## Actions
 * - `checkAuthorization()`: Checks for token presence, retrieves the current user profile,
 *   and updates the authentication state accordingly.
 * - `logout()`: Clears the token and resets the store state.
 *
 * ## Usage example:
 * ```ts
 * const auth = useAuthenticationStore()
 *
 * // On app mount
 * await auth.checkAuthorization()
 *
 * // Logout user
 * auth.logout()
 * ```
 */
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
