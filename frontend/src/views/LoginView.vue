<script setup lang="ts">
import { ref } from 'vue'
import type { LoginDTO } from '@/DTO/request/User/LoginDTO.ts'
import { UserService } from '@/service/UserService.ts'
import { useToast } from 'primevue/usetoast'
import { CookieService } from '@/service/CookieService.ts'
import router from '@/router/BaseRouter.ts'
import { handleApiError } from '@/utils/ApiErrorHandler.ts'

const userService = new UserService()
const cookieService = new CookieService()

const email = ref('')
const password = ref('')

const toast = useToast()

async function login() {
  const user: LoginDTO = {
    email: email.value,
    password: password.value,
  }

  try {
    const result = await userService.login(user)
    if (result.status == 200) {
      cookieService.setTokenCookies(result.data.token)
      await router.push({ name: 'dashboard' }).then(() => {
        window.location.reload()
      })
    }
  } catch (error: unknown) {
    onError(error)
  }
}

function onError(error: unknown) {
  const err = handleApiError(error)
  const status = err!.status
  const message = err!.message

  switch (status) {
    case 401:
      toast.add({
        severity: 'error',
        summary: 'Invalid Credentials',
        detail: message,
        life: 3000,
      })
      break
    default:
      toast.add({
        severity: 'error',
        summary: message || 'Something went wrong, please try again',
        life: 3000,
      })
  }
}
</script>

<template>
  <form @submit.prevent="login" class="form">
    <div class="title-container">
      <h2>Login</h2>
    </div>

    <label for="email">E-mailadres:</label>
    <input type="email" id="email" v-model="email" required data-cy="loginEmail" />

    <label for="password">Password:</label>
    <input type="password" id="password" v-model="password" required data-cy="loginPassword" />

    <button type="submit" data-cy="loginSubmit">Login</button>
  </form>
</template>

<style scoped>
form {
  max-width: 400px;
  margin: 100px auto;
  padding: 20px;
  background-color: #f8f8f8;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  display: flex;
  flex-direction: column;
  gap: 12px;

  .title-container {
    display: flex;
    justify-content: space-between;

    .close-icon {
      font-weight: bolder;
      font-size: 30px;
    }

    .close-icon:hover {
      cursor: pointer;
      color: #7393b3;
    }
  }

  input,
  button {
    padding: 10px;
    border-radius: 4px;
    border: 1px solid #ccc;
  }

  button {
    background-color: #7393b3;
    color: white;
    font-weight: bold;
    border: none;
    cursor: pointer;
  }

  button:hover {
    background-color: #5d7a9e;
  }
}
</style>
