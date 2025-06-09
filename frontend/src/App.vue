<script setup lang="ts">
import { RouterView } from 'vue-router'
import SideNavComponent from '@/components/SideNavComponent.vue'
import TopBarComponent from '@/components/TopBarComponent.vue'
import { useAuthenticationStore } from '@/stores/AuthenticationStore.ts'
import { onMounted } from 'vue'

const authentication = useAuthenticationStore()

onMounted(async () => {
  await authentication.checkAuthorization()
})
</script>

<template>
  <GlobalToast />
  <div class="app">
    <SideNavComponent v-if="authentication.isLoggedIn" />
    <main>
      <TopBarComponent v-if="authentication.isLoggedIn" />
      <RouterView />
    </main>
  </div>
</template>

<style scoped>
.app {
  display: flex;
  width: 100%;
  min-height: 600px;
  height: 100vh;

  main {
    width: auto;
    flex: 1;
    overflow-y: auto;
  }
}
</style>
