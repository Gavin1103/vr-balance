<script setup lang="ts">
import type { CompletedExerciseResponse } from '@/DTO/response/CompletedExerciseResponse.ts'

const props = defineProps<{
  recentExercises: CompletedExerciseResponse[]
}>()
</script>

<template>
  <div class="recent-exercises-container">
    <h2>Last 10 Exercises</h2>
    <div class="recent-exercises">
      <div
        v-for="exercise in props.recentExercises"
        :key="exercise.completedAt"
        class="exercise-card"
      >
        <h3>{{ exercise.exercise.replace('_', ' ') }}</h3>
        <p><strong>Points:</strong> {{ exercise.earnedPoints }}</p>
        <p><strong>Difficulty:</strong> {{ exercise.difficulty }}</p>
        <p><strong>Completed:</strong> {{ new Date(exercise.completedAt).toLocaleDateString() }}</p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.recent-exercises-container {
  margin: 50px 0;
  width: 100%;
  overflow-x: auto;
  display: flex;
  flex-direction: column;

  .recent-exercises {
    display: flex;
    gap: 16px;
    padding: 10px 0;
    margin: 10px 0;

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
