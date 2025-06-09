<script setup lang="ts">
import { onMounted, ref, watch } from 'vue'
import type { UserStatsResponse } from '@/response/UserStatsResponse.ts'
import { UserStatsService } from '@/service/UserStatsService.ts'
import { SortByEnum } from '@/enums/StatisticsEnums.ts'

const userStatsService: UserStatsService = new UserStatsService()
const userStats = ref<UserStatsResponse[]>([])
const isLoading = ref(false)

let page = ref<number>(0)
let size = ref<number>(5)
const sortBy = ref<string>("totalPoints")
let direction = ref<string>('desc')
const hasMore = ref(true)

onMounted(() => {
  fetchData()
})

watch(sortBy, () => {
  page.value = 0
  hasMore.value = true
  fetchData(false)
})

async function fetchData(append = false) {
  if (!hasMore.value) return

  isLoading.value = true
  try {
    const response = await userStatsService.getLeaderboard(
      page.value,
      size.value,
      sortBy.value,
      direction.value,
    )
    const newStats = response.data.content

    if (append) {
      userStats.value = [...userStats.value, ...newStats]
    } else {
      userStats.value = newStats
    }

    const totalPages = response.data.totalPages
    if (page.value >= totalPages - 1) {
      hasMore.value = false
    }
  } catch (error) {
    console.error('Failed to fetch userStats:', error)
  } finally {
    isLoading.value = false
  }
}

function showMore() {
  if (hasMore.value) {
    page.value++
    fetchData(true)
  }
}
</script>

<template>
  <div class="container">
    <div class="title-container">
      <h2>Leaderboard</h2>
      <select v-model="sortBy">
        <option
          v-for="(label, key) in SortByEnum"
          :key="key"
          :value="key"
        >
          {{ label }}
        </option>
      </select>
    </div>

    <table v-if="userStats.length > 0">
      <thead>
        <tr>
          <th>Total points</th>
          <th>Highest streak</th>
          <th>current streak</th>
          <th>Completed exercises</th>
          <th>Username</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="stats in userStats" :key="stats.username">
          <td>{{ stats.totalPoints }}</td>
          <td>{{ stats.highestStreak }}</td>
          <td>{{ stats.currentStreak }}</td>
          <td>{{ stats.totalExercises }}</td>
          <td>{{ stats.username }}</td>
        </tr>
      </tbody>
    </table>
    <p v-if="hasMore && !isLoading" @click="showMore" class="more-button">Show more...</p>
    <p v-else-if="!hasMore">No more data to load.</p>
    <p v-if="isLoading">Loading...</p>
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

  table {
    width: 100%;
    margin: 20px auto 0 auto;
    border-collapse: collapse;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);

    th,
    td {
      border: 1px solid #ddd;
      padding: 12px;
      text-align: left;
    }

    th {
      background-color: #f4f4f4;
    }

    tbody tr:hover {
      cursor: pointer;
      background-color: #ddd;
    }
  }

  .more-button {
    display: inline-block;
    color: #2b6cb0;
    text-decoration: underline;
    cursor: pointer;
    font-weight: bold;
    font-size: 16px;
    transition: color 0.2s ease;
  }

  .more-button:hover {
    color: #1a4e8a;
    text-decoration: none;
  }

  p {
    margin: 10px 0 0 0;
  }
}
</style>
