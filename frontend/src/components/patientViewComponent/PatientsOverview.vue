<script setup lang="ts">
import { ref, onMounted, watch, defineProps } from 'vue'
import { UserService } from '@/service/UserService.ts'
import type { UserProfileResponse } from '@/response/UserProfileResponse.ts'

const userService: UserService = new UserService()
const users = ref<UserProfileResponse[]>([])
const props = defineProps<{
  refreshTrigger: number
}>()

let page = ref<number>(0)
let size = ref<number>(5)
const hasMore = ref(true)
const isLoading = ref(false)

onMounted(() => {
  fetchPatients()
})

watch(() => props.refreshTrigger, resetAndFetch)

// TODO: use pagination (Already implemented in the backend)
async function fetchPatients(append = false) {
  if (!hasMore.value) return

  isLoading.value = true
  try {
    const response = await userService.getAllUsers(page.value, size.value)
    const newUsers = response.data.content

    if (append) {
      users.value = [...users.value, ...newUsers]
    } else {
      users.value = newUsers
    }

    const totalPages = response.data.totalPages
    if (page.value >= totalPages - 1) {
      hasMore.value = false
    }
  } catch (error) {
    console.error('Failed to fetch users:', error)
  } finally {
    isLoading.value = false
  }
}

function formatDate(dateStr: string): string {
  const date = new Date(dateStr)
  return date.toLocaleDateString()
}

function showUserDetail(user: UserProfileResponse) {
  console.log('show information detail of user:', user)
}

function showMore() {
  if (hasMore.value) {
    page.value++
    fetchPatients(true)
  }
}

function resetAndFetch() {
  page.value = 0
  hasMore.value = true
  fetchPatients(false)
}
</script>

<template>
  <table class="users-table" v-if="users.length > 0">
    <thead>
      <tr>
        <th>Username</th>
        <th>Email</th>
        <th>First name</th>
        <th>Last name</th>
        <th>Role</th>
        <th>Birth date</th>
      </tr>
    </thead>
    <tbody>
      <tr @click="showUserDetail(user)" v-for="user in users" :key="user.username">
        <td>{{ user.username }}</td>
        <td>{{ user.email }}</td>
        <td>{{ user.firstName }}</td>
        <td>{{ user.lastName }}</td>
        <td>{{ user.role }}</td>
        <td>{{ formatDate(user.birthDate) }}</td>
      </tr>
    </tbody>
  </table>
  <p v-if="hasMore && !isLoading" @click="showMore" class="more-button">Show more...</p>
  <p v-else-if="!hasMore">No more data to load.</p>
  <p v-if="isLoading">Loading...</p>
</template>

<style scoped>
.users-table {
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
</style>
