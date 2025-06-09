<script setup lang="ts">
import { ref, onMounted } from 'vue'
import RegisterPatientComponent from '@/components/RegisterPatientComponent.vue'
import PatientsOverview from '@/components/patientViewComponent/PatientsOverview.vue'
import { useRoute } from 'vue-router'

const route = useRoute()
const openFormState = ref(false)
const refreshTrigger = ref(0)

function handleSuccess() {
  openFormState.value = false
  refreshTrigger.value++
}

onMounted(() => {
  if (route.query.fromDashboard === 'true') {
    openFormState.value = true
  }
})
</script>

<template>
  <div class="container">
    <div class="title-container" v-if="!openFormState">
      <h1>Patients</h1>
      <button @click="openFormState = true">Create patient +</button>
    </div>

    <RegisterPatientComponent
      v-if="openFormState"
      @close="openFormState = false"
      @success="handleSuccess"
    />

    <PatientsOverview v-if="!openFormState" :refreshTrigger="refreshTrigger" />
  </div>
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

    button {
      background-color: #7393b3;
      padding: 16px;
      color: white;
      font-weight: bolder;
      border: none;
      border-radius: 5px;
    }

    button:hover {
      cursor: pointer;
      background-color: #5d7a9e;
    }
  }
}
</style>
