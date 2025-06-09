<script setup lang="ts">
import { ref } from 'vue'
import type { RegisterPatientDTO } from '@/request/User/RegisterPatientDTO.ts'
import { UserService } from '@/service/UserService.ts'
import { useApiFeedback } from '@/composables/useApiFeedback'

const emit = defineEmits(['close', 'success'])
const userService = new UserService()
const { onSuccess, onError } = useApiFeedback()

const firstName = ref('')
const lastName = ref('')
const username = ref('')
const email = ref('')
const birthDate = ref('')

async function registerPatient() {
  const newPatient: RegisterPatientDTO = {
    email: email.value,
    firstName: firstName.value,
    lastName: lastName.value,
    username: username.value,
    birthDate: birthDate.value,
  }
  try {
    const ApiResponse = await userService.registerPatient(newPatient)
    onSuccess(ApiResponse)
    emit('close')
    emit('success')
  } catch (error: unknown) {
    onError(error)
  }
}
</script>

<template>
  <form @submit.prevent="registerPatient">
    <div class="title-container">
      <h2>Register Patient</h2>
      <div @click="$emit('close')" class="close-icon">X</div>
    </div>

    <label for="firstName">First name:</label>
    <input id="firstName" type="text" required placeholder="John" v-model="firstName" data-cy="firstName"/>

    <label for="lastName">Last name:</label>
    <input id="lastName" type="text" required placeholder="Doe" v-model="lastName" data-cy="lastName"/>

    <label for="username">Username:</label>
    <input id="username" type="text" required placeholder="JohnDoe2003" v-model="username" data-cy="username"/>

    <label for="email">Email:</label>
    <input id="email" type="email" required placeholder="JohnDoe2003@gmail.com" v-model="email" data-cy="email"/>

    <label for="birthDate">Birth date:</label>
    <input id="birthDate" type="date" required v-model="birthDate" data-cy="birthDate"/>

    <button type="submit" data-cy="submit">Submit</button>
  </form>
</template>

<style scoped>
form {
  width: 100%;
  margin: 0 auto;
  max-width: 400px;
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
