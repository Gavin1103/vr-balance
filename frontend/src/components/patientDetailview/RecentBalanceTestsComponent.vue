<script setup lang="ts">
import type { CompletedExerciseResponse } from '@/DTO/response/CompletedExerciseResponse.ts'
import { Download } from 'lucide-vue-next'
import { ExerciseService } from '@/service/ExerciseService.ts'

const props = defineProps<{
  recentBalanceTests: CompletedExerciseResponse[]
}>()

const exerciseService = new ExerciseService();

async function downloadBalanceTest(balanceTestId: number) {
  console.log(balanceTestId);
  await exerciseService.downloadBalanceTestsResult(balanceTestId)
}
</script>

<template>
  <div class="recent-exercises-container">
    <h2>Last 10 balance tests</h2>
    <div class="recent-exercises">
      <div
        v-for="exercise in props.recentBalanceTests"
        :key="exercise.completedAt"
        class="exercise-card"
      >
        <h3>{{ exercise.exercise }} tests</h3>
        <p><strong>Completed:</strong> {{ new Date(exercise.completedAt).toLocaleString() }}</p>
        <Download class="download-icon" @click="downloadBalanceTest(exercise.id)" />
      </div>
    </div>
  </div>
</template>

<style scoped>
.recent-exercises-container {
  width: 100%;
  overflow-x: auto;
  display: flex;
  flex-direction: column;

  .recent-exercises {
    display: flex;
    gap: 16px;
    padding: 10px 0;
    margin: 10px 0;

    .download-icon {
      transition: transform 0.4s ease, color 0.4s ease;
      margin: 10px 0 0 0;

      &:hover{
        transform: scale(1.2);
        cursor: pointer;
      }
    }

    .exercise-card {
      min-width: 250px;
      padding: 20px;
      background-color: #f8f9fa;
      border: 1px solid #ccc;
      border-radius: 12px;
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
      flex-shrink: 0;

      h3 {
        margin-top: 0;
        color: #2b6cb0;
      }

      p {
        margin: 5px 0;
      }
    }
  }
}
</style>
