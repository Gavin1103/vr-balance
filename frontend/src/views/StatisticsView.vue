<script setup lang="ts">
import LeaderboardComponent from '@/components/statisticsPageComponent/LeaderboardComponent.vue'
import ExerciseStats from '@/components/statisticsPageComponent/ExerciseStats.vue'
</script>

<template>
  <div class="title-container">
    <h1>Statistics</h1>
    <hr />
  </div>
  <LeaderboardComponent />
  <ExerciseStats />
</template>

<style scoped>
.title-container {
  width: 90%;
  margin: auto;

  hr {
    margin: 20px 0 30px 0;
  }
}
</style>
