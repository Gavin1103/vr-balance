<script setup lang="ts">
import { RouterLink } from 'vue-router'
import { useAuthenticationStore } from '@/stores/AuthenticationStore.ts'
import router from '@/router/BaseRouter.ts'
import { useRoleAccess } from '@/composables/useRoleAcces.ts'
import { LogOut, HelpCircle, Users, BarChart2, LayoutDashboard } from 'lucide-vue-next'


const auth = useAuthenticationStore()
const { hasRole } = useRoleAccess()

function logout() {
  auth.logout()
  router.push({ name: 'login' }).then(() => {
    window.location.reload()
  })
}
</script>
<template>
  <nav>
    <RouterLink class="logo" to="/">VR-Balance</RouterLink>
    <div class="nav-items">
      <RouterLink class="nav-link" to="/dashboard">
        <LayoutDashboard class="icon" /> Dashboard
      </RouterLink>
      <RouterLink class="nav-link" to="/statistics">
        <BarChart2 class="icon" /> Statistics
      </RouterLink>
      <RouterLink v-if="hasRole(['ADMIN', 'PHYSIOTHERAPIST'])" class="nav-link" to="/patients">
        <Users class="icon" /> Patients
      </RouterLink>
      <RouterLink class="nav-link" to="#">
        <HelpCircle class="icon" /> Help
      </RouterLink>
      <a class="nav-link" href="#" @click.prevent="logout">
        <LogOut class="icon" /> Logout
      </a>
    </div>
  </nav>
</template>

<style scoped>
nav {
  min-width: 250px;
  height: 100%;
  background-color: #7393b3;
  display: flex;
  flex-direction: column;
  padding: 0 0 0 10px;

  .logo {
    font-size: 40px;
    font-weight: bold;
    color: white;
    text-decoration: none;
    margin: 10px 0 10px 0;
  }

  .nav-items {
    display: flex;
    flex-direction: column;
    gap: 1rem;
    margin: 10px 10px 0 0;

    .nav-link {
      text-decoration: none;
      color: white;
      font-weight: bold;
      transition: color 0.3s ease;
      display: flex;
      align-items: flex-end;
      font-size: 20px;

      &:hover {
        opacity: 0.6;
      }

      .icon {
        margin-right: 10px;
        vertical-align: middle;
      }
    }
  }
}
</style>
