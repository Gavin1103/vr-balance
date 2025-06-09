<script setup lang="ts">
import { onMounted, ref, watch } from 'vue'
import { ExerciseStatsFilterEnum } from '@/enums/StatisticsEnums.ts'
import { StatisticsService } from '@/service/StatisticsService.ts'
import BarChart from '@/components/chart/BarChart.vue'
import type { ApiResponse } from '@/response/ApiResponse.ts'
import type { ExerciseStatsResponse } from '@/response/ExerciseStatsResponse.ts'

const statisticsService: StatisticsService = new StatisticsService()
const filter = ref("allTime");
const stats = ref<ApiResponse<ExerciseStatsResponse[]>>();
const labels = ref<string[]>([])
const values = ref<number[]>([])

onMounted(() => {
  fetchData()
})

watch(filter, () => {
  fetchData()
})

async function fetchData(): Promise<void> {
  try {
    let response = await statisticsService.getAllTimeExerciseStats()
    if (filter.value === "last30Days") {
      response = await statisticsService.getExerciseStatsFromLast30days()
    }

    stats.value = response
    labels.value = stats.value?.data.map(stat => stat.exercise) || []
    values.value = stats.value?.data.map(stat => stat.count) || []

  } catch (error) {
    console.error('Error fetching exercise stats:', error)
  }
}

</script>

<template>
  <div class="container">
    <div class="title-container">
      <h2>Most completed exercises</h2>
      <select v-model="filter">
        <option
          v-for="(label, key) in ExerciseStatsFilterEnum"
          :key="key"
          :value="key"
        >
          {{ label }}
        </option>
      </select>
    </div>
    <BarChart :labels="labels" :data="values" title="Most Played Exercises" />
  </div>
</template>

<style scoped>
.container {
  margin: 50px auto 0 auto;
  width: 90%;
  display: flex;
  flex-direction: column;

  .title-container {
    width: 100%;
    display: flex;
    justify-content: space-between;

    select {
      background-color: #7393b3;
      padding: 16px;
      color: white;
      font-weight: bolder;
      border: none;
      border-radius: 5px;
      min-width: 125px;
    }

    select:hover {
      cursor: pointer;
      background-color: #5d7a9e;
    }
  }
}
</style>
