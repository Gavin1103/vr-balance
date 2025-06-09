<script setup lang="ts">
import { BarChart4Icon, Users, UserPlus, LucideUserRoundCog, Mail, Activity } from 'lucide-vue-next'
import DashboardCard from '@/components/dashboardViewComponent/DashboardCard.vue'
import { ref } from 'vue'
import RegisterPatientComponent from '@/components/RegisterPatientComponent.vue'
import { useRoleAccess } from '@/composables/useRoleAcces.ts'

const { hasRole } = useRoleAccess()

const openFormState = ref(false)
</script>

<template>
  <div v-if="openFormState === false" class="container">
    <div class="title-container">
      <h1>Quick actions</h1>
    </div>

    <section class="card-container">
      <DashboardCard title="Statistics" :to="'/statistics'" :icon="BarChart4Icon" />
      <DashboardCard  v-if="hasRole(['PATIENT'])" title="Activity" :to="'/personal-activity'" :icon="Activity" />

      <DashboardCard
        v-if="hasRole(['ADMIN', 'PHYSIOTHERAPIST'])"
        title="Patients"
        :to="'/patients'"
        :icon="Users"
      />
      <DashboardCard
        v-if="hasRole(['ADMIN', 'PHYSIOTHERAPIST'])"
        title="Create patient +"
        :icon="UserPlus"
        @click="openFormState = true"
        data-cy="registerPatientButton"
      />
      <DashboardCard title="Edit profile" :to="'/profile'" :icon="LucideUserRoundCog" />
      <DashboardCard
        v-if="hasRole(['ADMIN', 'PHYSIOTHERAPIST'])"
        title="Mail"
        :to="'/#'"
        :icon="Mail"
      />
    </section>
  </div>

  <RegisterPatientComponent
    v-else
    @close="openFormState = false"
    @success="openFormState = false"
    data-cy="registerPatientForm"
  />
</template>

<style scoped>
.container {
  margin: 0 auto 0 auto;
  width: 90%;
  display: flex;
  flex-direction: column;

  .title-container {
    width: 100%;
    display: flex;
    justify-content: space-between;
  }

  .card-container {
    margin: 30px 0 0 0;
    width: 100%;
    display: flex;
    gap: 0 50px;
    flex-wrap: wrap;
  }
}
</style>
