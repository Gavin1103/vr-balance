import { useAuthenticationStore } from '@/stores/AuthenticationStore'
import { computed } from 'vue'

export function useRoleAccess() {
  const auth = useAuthenticationStore()

  const hasRole = (roles: string[]) =>
    auth.isLoggedIn && roles.includes(auth.role!)

  const isAdmin = computed(() => hasRole(['ADMIN']))
  const isPhysio = computed(() => hasRole(['PHYSIOTHERAPIST']))
  const isPatient = computed(() => hasRole(['PATIENT']))

  return { hasRole, isAdmin, isPhysio, isPatient }
}
