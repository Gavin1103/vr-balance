<script setup lang="ts">
import { defineProps, type FunctionalComponent } from 'vue'
import { type RouteLocationRaw, RouterLink } from 'vue-router'
import type { LucideProps } from 'lucide-vue-next'

defineProps<{
  title: string
  to?: RouteLocationRaw | null
  icon: FunctionalComponent<LucideProps>
}>()
</script>

<template>
  <RouterLink v-if="to" class="card" :to="to">
    <div class="card-header">
      <component :is="icon" class="icon" />
    </div>
    <p class="card-title">{{ title }}</p>
  </RouterLink>

  <div v-else class="card">
    <div class="card-header">
      <component :is="icon" class="icon" />
    </div>
    <p class="card-title">{{ title }}</p>
  </div>
</template>

<style scoped>
.card {
  margin: 20px 0 30px 0;
  width: 280px;
  height: 280px;
  background-color: #7393b3;
  border-radius: 16px;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.15);
  padding: 20px 20px 0 20px;
  transition:
    transform 0.2s ease,
    box-shadow 0.2s ease;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  opacity: 0.9;
  gap: 16px;
  text-decoration: none;

  &:hover {
    transform: translateY(-4px);
    box-shadow: 0 6px 20px rgba(0, 0, 0, 0.2);
  }

  .card-header {
    width: 100%;
    height: auto;
    display: flex;
    justify-content: center;
    align-items: center;
    border-radius: 10px;
    background-color: white;
    padding: 16px;

    .icon {
      width: 100px;
      height: 100px;
      color: black;
    }
  }

  .card-title {
    font-size: 1.5rem;
    color: white;
    font-weight: bold;
    padding: 0 0 10px 0;
  }
}

.card:hover {
  transform: translateY(-16px);
  opacity: 1;
  box-shadow: 0 6px 16px rgba(0, 0, 0, 0.15);
  cursor: pointer;
}
</style>
