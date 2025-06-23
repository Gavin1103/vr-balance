import { useAuthenticationStore } from '@/stores/AuthenticationStore'
import { computed } from 'vue'

/**
 * A composable that provides role-based access checks for authenticated users.
 *
 * This utility uses the `AuthenticationStore` to determine the logged-in user's role
 * and offers helper methods and computed properties for role-based logic in components.
 *
 * Usage example in a component:
 * ```ts
 * const { isAdmin, isPhysio, hasRole } = useRoleAccess()
 * if (isAdmin.value) {
 *   // show admin-only content
 * }
 * ```
 *
 * @returns {{
 *   hasRole: (roles: string[]) => boolean,
 *   isAdmin: import('vue').ComputedRef<boolean>,
 *   isPhysio: import('vue').ComputedRef<boolean>,
 *   isPatient: import('vue').ComputedRef<boolean>
 * }}
 * - `hasRole(roles: string[])`: function to check if the user has one of the specified roles
 * - `isAdmin`: computed property that returns `true` if the user has the `ADMIN` role
 * - `isPhysio`: computed property that returns `true` if the user has the `PHYSIOTHERAPIST` role
 * - `isPatient`: computed property that returns `true` if the user has the `PATIENT` role
 */
export function useRoleAccess() {
  const auth = useAuthenticationStore()

  const hasRole = (roles: string[]) =>
    auth.isLoggedIn && roles.includes(auth.role!)

  const isAdmin = computed(() => hasRole(['ADMIN']))
  const isPhysio = computed(() => hasRole(['PHYSIOTHERAPIST']))
  const isPatient = computed(() => hasRole(['PATIENT']))

  return { hasRole, isAdmin, isPhysio, isPatient }
}
