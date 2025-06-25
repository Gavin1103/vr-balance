import { createRouter, createWebHistory } from 'vue-router'
import DashboardView from '../views/DashboardView.vue'
import LoginView from '@/views/LoginView.vue'
import PatientsView from '@/views/PatientsView.vue'
import { useAuthenticationStore } from '@/stores/AuthenticationStore.ts'
import profilePageView from '@/views/ProfilePageView.vue'
import UnautorizedComponent from '@/components/UnautorizedComponent.vue'
import StatisticsView from '@/views/StatisticsView.vue'
import PatientDetailView from '@/views/PatientDetailView.vue'
import PersonalActivityView from '@/views/PersonalActivityView.vue'
import PrivacyPolicyView from '@/views/PrivacyPolicyView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'login',
      component: LoginView,
    },
    {
      path: '/unauthorized',
      name: 'unauthorized',
      component: UnautorizedComponent,
    },
    {
      path: '/dashboard',
      name: 'dashboard',
      component: DashboardView,
      meta: {
        requiresAuth: true,
        allowedRoles: ['PHYSIOTHERAPIST', 'ADMIN', 'PATIENT'],
      },
    },
    {
      path: '/statistics',
      name: 'statistics',
      component: StatisticsView,
      meta: {
        requiresAuth: true,
        allowedRoles: ['PHYSIOTHERAPIST', 'ADMIN', 'PATIENT'],
      },
    },
    {
      path: '/patients',
      name: 'patients',
      component: PatientsView,
      meta: {
        requiresAuth: true,
        allowedRoles: ['PHYSIOTHERAPIST', 'ADMIN'],
      },
    },
    {
      path: '/patient-detail/:patientId',
      name: 'patient-detail',
      component: PatientDetailView,
      meta: {
        requiresAuth: true,
        allowedRoles: ['PHYSIOTHERAPIST', 'ADMIN'],
      },
    },
    {
      path: '/profile',
      name: 'profile',
      component: profilePageView,
      meta: {
        requiresAuth: true,
        allowedRoles: ['PHYSIOTHERAPIST', 'ADMIN', 'PATIENT'],
      },
    },
    {
      path: '/personal-activity',
      name: 'personal-activity',
      component: PersonalActivityView,
      meta: {
        requiresAuth: true,
        allowedRoles: ['PATIENT'],
      },
    },
    {
      path: '/policy',
      name: 'privacy-policy',
      component: PrivacyPolicyView,
    },
  ],
})

router.beforeEach(async (to, from, next) => {
  const authStore = useAuthenticationStore()
  const isLoginPage = to.name === 'login'
  const requiresAuth = to.meta.requiresAuth === true
  const allowedRoles = to.meta.allowedRoles as string[] | undefined

  if (authStore.isLoggedIn && !authStore.role) {
    await authStore.checkAuthorization()
  }

  // User tries to access authorized page
  if (requiresAuth && !authStore.isLoggedIn) {
    if (!isLoginPage) {
      return next({ name: 'login' })
    }
    return next()
  }

  // User is logged in en tries to go to the login page -> redirect to dashboard
  if (isLoginPage && authStore.isLoggedIn) {
    return next({ name: 'dashboard' })
  }

  // User does not have the correct role
  if (allowedRoles && !allowedRoles.includes(authStore.role!)) {
    return next({ name: 'unauthorized' })
  }

  next()
})

export default router
