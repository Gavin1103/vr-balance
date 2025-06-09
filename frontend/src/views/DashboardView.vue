<script setup lang="ts">
import { BarChart4Icon, Users, UserPlus, LucideUserRoundCog, Mail } from 'lucide-vue-next'
import DashboardCard from '@/components/dashboardViewComponent/DashboardCard.vue'
import { ref } from 'vue'
import RegisterPatientComponent from '@/components/RegisterPatientComponent.vue'

const openFormState = ref(false)
</script>

<template>
  <div v-if="openFormState === false" class="container">
    <div class="title-container">
      <h1>Quick actions</h1>
    </div>

    <section class="card-container">
      <DashboardCard title="Statistics" :to="'/statistics'" :icon="BarChart4Icon" />
      <DashboardCard title="Patients" :to="'/patients'" :icon="Users" />
      <DashboardCard
        title="Create patient +"
        :icon="UserPlus"
        @click="openFormState = true"
        data-cy="registerPatientButton"
      />
      <DashboardCard title="Edit profile" :to="'/profile'" :icon="LucideUserRoundCog" />
      <DashboardCard title="Mail" :to="'/#'" :icon="Mail" />
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
    justify-content: space-between;
    flex-wrap: wrap;
  }
}
</style>
