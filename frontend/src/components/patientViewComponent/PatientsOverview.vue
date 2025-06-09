<script setup lang="ts">
import { ref, onMounted, watch, defineProps } from 'vue'
import { UserService } from '@/service/UserService.ts'
import type { UserProfileResponse } from '@/DTO/response/UserProfileResponse.ts'
import { Trash2, Eye } from 'lucide-vue-next'
import { useApiFeedback } from '@/composables/useApiFeedback.ts'
import router from '@/router/BaseRouter.ts'
import { formatDate } from '@/helper/formatDate.ts'

const userService: UserService = new UserService()
const users = ref<UserProfileResponse[]>([])
const userToDelete = ref<UserProfileResponse | null>(null)
const showConfirmation = ref(false)

const props = defineProps<{
  refreshTrigger: number
}>()

const { onSuccess, onError } = useApiFeedback()

let page = ref<number>(0)
let size = ref<number>(5)
const hasMore = ref(true)
const isLoading = ref(false)

onMounted(() => {
  fetchPatients()
})

watch(() => props.refreshTrigger, resetAndFetch)

async function fetchPatients(append = false) {
  if (!hasMore.value) return

  isLoading.value = true
  try {
    const response = await userService.getAllPatients(page.value, size.value)
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

async function performDelete() {
  if (!userToDelete.value) return

  try {
    const ApiResponse = await userService.deleteUser(userToDelete.value.id)
    onSuccess(ApiResponse)
    resetAndFetch()
  } catch (error) {
    onError(error)
  } finally {
    showConfirmation.value = false
    userToDelete.value = null
  }
}

function confirmDeleteUser(user: UserProfileResponse) {
  userToDelete.value = user
  showConfirmation.value = true
}

function viewUserDetail(userId: number) {
  router.push(`/patient-detail/${userId}`)
}
</script>

<template>
  <table class="users-table-container" v-if="users.length > 0">
    <thead>
      <tr>
        <th>Username</th>
        <th>Email</th>
        <th>First name</th>
        <th>Last name</th>
        <th>Role</th>
        <th>Birth date</th>
        <th>Actions</th>
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
        <td class="action-icons">
          <Eye @click.stop="viewUserDetail(user.id)" style="color: #7393b3" class="icon" />
          <Trash2 @click.stop="confirmDeleteUser(user)" style="color: red" class="icon" />
        </td>
      </tr>
    </tbody>
  </table>
  <p v-if="hasMore && !isLoading" @click="showMore" class="more-button">Show more...</p>
  <p v-else-if="!hasMore">No more data to load.</p>
  <p v-if="isLoading">Loading...</p>

  <div v-if="showConfirmation" class="confirm-overlay">
    <div class="confirm-box">
      <p>
        Are you sure you want to delete <strong>{{ userToDelete?.username }}</strong
        >?
      </p>
      <div class="buttons">
        <button class="confirm" @click="performDelete">Yes</button>
        <button class="cancel" @click="showConfirmation = false">Cancel</button>
      </div>
    </div>
  </div>
</template>

<style lang="css" scoped>
.users-table-container {
  width: 100%;
  margin: 20px auto 0 auto;
  border-collapse: collapse;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);

  thead {
    th {
      background-color: #f4f4f4;
    }
  }

  tbody {
    tr {
      .action-icons {
        height: 100%;
        display: flex;
        justify-content: space-evenly;

        .icon {
          transition:
            transform 0.4s ease,
            color 0.4s ease;

          &:hover {
            cursor: pointer;
            transform: scale(1.2);
          }
        }
      }
    }
  }

  th,
  td {
    border: 1px solid #ddd;
    padding: 12px;
    text-align: left;
  }

  tbody tr:hover {
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

/*Delete confirmation message*/
.confirm-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  background-color: rgba(0, 0, 0, 0.6);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 999;

  .confirm-box {
    background: white;
    padding: 25px;
    border-radius: 10px;
    text-align: center;
    max-width: 350px;
    width: 90%;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.3);

    .buttons {
      margin-top: 15px;
      display: flex;
      justify-content: space-around;

      button {
        padding: 10px 10px;
        border: none;
        border-radius: 5px;
        font-weight: bold;
        cursor: pointer;
        width: 100px;
      }

      .cancel {
        background-color: #ccc;
        color: black;
      }

      .confirm {
        background-color: red;
        color: white;
      }
    }
  }
}

</style>
