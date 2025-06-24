// In script setup
<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router'
import type { ApiResponse } from '@/DTO/response/ApiResponse.ts'
import type { PatientDetailResponse } from '@/DTO/response/PatientDetailResponse.ts'
import { onMounted, ref } from 'vue'
import { UserService } from '@/service/UserService.ts'
import { Trash2 } from 'lucide-vue-next'
import { formatDate } from '@/helper/formatDate.ts'
import type { UserProfileResponse } from '@/DTO/response/UserProfileResponse.ts'
import { useApiFeedback } from '@/composables/useApiFeedback.ts'
import RecentExercisesComponent from '@/components/patientDetailview/RecentExercisesComponent.vue'
import RecentBalanceTestsComponent
  from '@/components/patientDetailview/RecentBalanceTestsComponent.vue'
import BalanceTestChartComponent from '@/components/patientDetailview/BalanceTestChartComponent.vue'

const { onSuccess, onError } = useApiFeedback()
const route = useRoute()
const router = useRouter()
const userId = Number(route.params.patientId)

const patientDetail = ref<ApiResponse<PatientDetailResponse>>()
const isLoading = ref(false)
const userService: UserService = new UserService()

const userToDelete = ref<UserProfileResponse | null>(null)
const showConfirmation = ref(false)

async function fetchPatientDetailData() {
  if (!route.params.patientId || isNaN(userId)) {
    await router.push('/dashboard')
    return
  }

  try {
    isLoading.value = true
    patientDetail.value = await userService.fetchPatientDetail(userId)
    console.log(patientDetail.value.data.downloadLast10Exercises)
  } catch (error) {
    console.error(error)
  } finally {
    isLoading.value = false
  }
}

onMounted(() => {
  fetchPatientDetailData()
})

async function performDelete() {
  if (!userToDelete.value) return

  try {
    const ApiResponse = await userService.deleteUser(userToDelete.value.id)
    onSuccess(ApiResponse)
    await router.push('/patients')
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
</script>

<template>
  <!-- Loading Spinner -->
  <div v-if="isLoading" class="loading-spinner">
    <span class="spinner"></span>
  </div>

  <!-- Patient Detail Content -->
  <div v-else class="patient-detail-container">
    <h1>Patient activity</h1>
    <table
      class="users-table-container"
      v-if="patientDetail && patientDetail.data && patientDetail.data.user"
    >
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
        <tr :key="patientDetail.data.user.username">
          <td>{{ patientDetail.data.user.username }}</td>
          <td>{{ patientDetail.data.user.email }}</td>
          <td>{{ patientDetail.data.user.firstName }}</td>
          <td>{{ patientDetail.data.user.lastName }}</td>
          <td>{{ patientDetail.data.user.role }}</td>
          <td>{{ formatDate(patientDetail.data.user.birthDate) }}</td>
          <td class="action-icons">
            <Trash2
              @click.stop="confirmDeleteUser(patientDetail.data.user)"
              style="color: red"
              class="icon"
            />
          </td>
        </tr>
      </tbody>
    </table>

    <BalanceTestChartComponent
      v-if="patientDetail?.data?.recentBalanceTests"
      :recentBalanceTests="patientDetail.data.recentBalanceTests"
    />

    <RecentBalanceTestsComponent
      v-if="patientDetail?.data?.downloadLast10Exercises"
      :recentBalanceTests="patientDetail.data.downloadLast10Exercises"
    />

    <RecentExercisesComponent
      v-if="patientDetail?.data?.recentExercises"
      :recentExercises="patientDetail.data.recentExercises"
    />

    <!--    Confirmation message-->
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
  </div>
</template>

<style scoped>
.patient-detail-container {
  min-height: 600px;
  margin: 0 auto 0 auto;
  width: 90%;
  display: flex;
  flex-direction: column;

  .users-table-container {
    width: 100%;
    margin: 20px auto 0 auto;
    border-collapse: collapse;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);

    thead {
      th {
        background-color: #f8f9fa;
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
