<script setup lang="ts">
import { ref, computed } from 'vue'
import { Line } from 'vue-chartjs'
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  LineElement,
  CategoryScale,
  LinearScale,
  PointElement
} from 'chart.js'

import type { BalanceTestResponse } from '@/DTO/response/BalanceTestResponse.ts'
import type { Vector3DTO } from '@/DTO/Vector3DTO.ts'

ChartJS.register(Title, Tooltip, Legend, LineElement, CategoryScale, LinearScale, PointElement)

const props = defineProps<{
  recentBalanceTests: BalanceTestResponse[]
}>()


const selectedPhase = ref<'phase_1' | 'phase_2' | 'phase_3' | 'phase_4'>('phase_1')

const chartData = computed(() => {
  const datasets = props.recentBalanceTests.map((test, index) => {
    const raw = test[selectedPhase.value]
    const label = new Date(test.completedAt).toLocaleDateString()

    let phaseData: Vector3DTO[] = []

    if (typeof raw === 'string') {
      try {
        phaseData = JSON.parse(raw)
      } catch (e) {
        console.warn(`Invalid JSON in ${selectedPhase.value}`, e)
      }
    } else if (Array.isArray(raw)) {
      phaseData = raw
    }

    return {
      label,
      data: phaseData.map((v: Vector3DTO) => v.y),
      fill: false,
      borderColor: `hsl(${index * 60}, 70%, 50%)`,
      tension: 0.3
    }
  })

  const maxLength = Math.max(...datasets.map(d => d.data.length))
  const labels = Array.from({ length: maxLength }, (_, i) => `Step ${i + 1}`)

  return {
    labels,
    datasets
  }
})
</script>

<template>
  <div class="balance-chart-container">
    <div class="controls">
      <h2>Last 5 balance tests</h2>
      <select id="phase" v-model="selectedPhase">
        <option value="phase_1">Phase 1</option>
        <option value="phase_2">Phase 2</option>
        <option value="phase_3">Phase 3</option>
        <option value="phase_4">Phase 4</option>
      </select>
    </div>

    <Line
      :data="chartData"
      :options="{
        responsive: true,
        plugins: {
          legend: {
            position: 'top'
          },
          title: {
            display: true,
            text: 'Balance Test - ' + selectedPhase
          }
        }
      }"
    />
  </div>
</template>

<style scoped>
.balance-chart-container {
  width: 100%;
  margin: 50px 0;
}

.controls {
  margin-bottom: 20px;
  display: flex;
  align-items: center;
  justify-content: space-between;

  select {
    margin: 10px 0;
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
</style>
