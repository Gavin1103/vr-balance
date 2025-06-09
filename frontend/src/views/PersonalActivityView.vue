// In script setup
<script setup lang="ts">
import type { ApiResponse } from '@/DTO/response/ApiResponse.ts'
import type { PatientDetailResponse } from '@/DTO/response/PatientDetailResponse.ts'
import { onMounted, ref } from 'vue'
import { UserService } from '@/service/UserService.ts'
import RecentExercisesComponent from '@/components/patientDetailview/RecentExercisesComponent.vue'
import RecentBalanceTestsComponent from '@/components/patientDetailview/RecentBalanceTestsComponent.vue'

const patientDetail = ref<ApiResponse<PatientDetailResponse>>()
const isLoading = ref(false)
const userService: UserService = new UserService()

async function fetchPatientDetailData() {
  try {
    isLoading.value = true
    patientDetail.value = await userService.fetchPatientDetailSelf()
  } catch (error) {
    console.error(error)
  } finally {
    isLoading.value = false
  }
}

onMounted(() => {
  fetchPatientDetailData()
})

</script>

<template>
  <!-- Loading Spinner -->
  <div v-if="isLoading" class="loading-spinner">
    <span class="spinner"></span>
  </div>

  <!-- Patient Detail Content -->
  <div v-else class="patient-detail-container">
    <h1>Activity</h1>
    <hr>
    <RecentBalanceTestsComponent
      v-if="patientDetail?.data?.recentBalanceTests"
      :recentBalanceTests="patientDetail.data.recentBalanceTests"
    />
    <RecentExercisesComponent
      v-if="patientDetail?.data?.recentExercises"
      :recentExercises="patientDetail.data.recentExercises"
    />
  </div>
</template>

<style scoped>
.patient-detail-container {
  min-height: 600px;
  margin: 0 auto 0 auto;
  width: 90%;
  display: flex;
  flex-direction: column;
  hr{
    margin: 20px 0 0 0;
  }
}

/*Loading spinner*/
.loading-spinner {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;

  .spinner {
    width: 40px;
    height: 40px;
    border: 4px solid #ccc;
    border-top-color: #2b6cb0;
    border-radius: 50%;
    animation: spin 0.8s linear infinite;
  }
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}
</style>
