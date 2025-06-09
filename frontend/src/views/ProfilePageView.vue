<script setup lang="ts">
import { useAuthenticationStore } from '@/stores/AuthenticationStore.ts'
import { ref } from 'vue'
import type { EditUserDTO } from '@/DTO/request/User/EditUserDTO.ts'
import { UserService } from '@/service/UserService.ts'
import { useApiFeedback } from '@/composables/useApiFeedback'
import ChangeCredentialComponent from '@/components/ChangeCredentialComponent.vue'
import router from '@/router/BaseRouter.ts'

const userService = new UserService()

const authentication = useAuthenticationStore()
const user = ref(authentication.user)
const { onSuccess, onError } = useApiFeedback()

const newEmail = ref(user.value?.email || '')
const newUsername = ref(user.value?.username || '')
const newFirstName = ref(user.value?.firstName || '')
const newLastName = ref(user.value?.lastName || '')
const newPhoneNumber = ref(user.value?.phoneNumber || '')
const newBirthDate = ref(user.value?.birthDate || '')

const isEditing = ref(false)
const activeComponent = ref<'profile' | 'password' | 'pincode'>('profile') //

async function handleEditProfile() {
  const newUserData: EditUserDTO = {
    email: newEmail.value,
    username: newUsername.value,
    firstName: newFirstName.value,
    lastName: newLastName.value,
    phoneNumber: newPhoneNumber.value,
    birthDate: newBirthDate.value,
  }

  try {
    const ApiResponse = await userService.editUserProfile(newUserData)
    onSuccess(ApiResponse)
    isEditing.value = false
  } catch (error) {
    onError(error)
  }
}

function resetForm() {
  isEditing.value = false
  newEmail.value = user.value?.email || ''
  newUsername.value = user.value?.username || ''
  newFirstName.value = user.value?.firstName || ''
  newLastName.value = user.value?.lastName || ''
  newPhoneNumber.value = user.value?.phoneNumber || ''
  newBirthDate.value = user.value?.birthDate || ''
}

function switchComponent(componentName: 'profile' | 'password' | 'pincode') {
  activeComponent.value = componentName
}
</script>
<template>
  <form @submit.prevent="handleEditProfile" v-if="activeComponent === 'profile'">
    <div class="title-container">
      <h2>Profile</h2>
      <div @click="router.push('/dashboard')" class="close-icon">X</div>
    </div>
    <label for="email">E-mail:</label>
    <input
      type="email"
      id="email"
      v-model="newEmail"
      :placeholder="'Add email'"
      :disabled="!isEditing"
      required
    />
    <label for="username">Username:</label>
    <input
      type="text"
      id="username"
      v-model="newUsername"
      :placeholder="'Add username'"
      :disabled="!isEditing"
      required
    />
    <label for="firstName">First name:</label>
    <input
      type="text"
      id="firstName"
      v-model="newFirstName"
      :placeholder="'Add first name'"
      :disabled="!isEditing"
    />
    <label for="lastName">Last name:</label>
    <input
      type="text"
      id="lastName"
      v-model="newLastName"
      :placeholder="'Add last name'"
      :disabled="!isEditing"
    />
    <label for="phoneNumber">phone number:</label>
    <input
      type="text"
      id="phoneNumber"
      v-model="newPhoneNumber"
      :placeholder="'Add phone number'"
      :disabled="!isEditing"
    />

    <label for="birthDate">birth date:</label>
    <input
      type="date"
      id="birthDate"
      v-model="newBirthDate"
      :placeholder="'Add you birth date'"
      :disabled="!isEditing"
      required
    />

    <button type="button" v-if="!isEditing" @click="isEditing = true" data-cy="editProfileButton">
      <strong>Edit</strong>
    </button>

    <button v-if="isEditing" type="submit" data-cy="submitEditProfileButton"><strong>Submit</strong></button>
    <button v-if="isEditing" style="background-color: red" @click="resetForm">
      <strong>Cancel</strong>
    </button>

    <button type="button" v-if="!isEditing" @click="switchComponent('password')">
      <strong>Change password</strong>
    </button>
    <button type="button" v-if="!isEditing" @click="switchComponent('pincode')">
      <strong>Change pincode</strong>
    </button>
  </form>

  <ChangeCredentialComponent
    v-if="activeComponent === 'password'"
    label="password"
    :submitFunction="
      (data) =>
        userService.changePassword({
          currentPassword: data.current,
          newPassword: data.new,
          repeatNewPassword: data.repeat,
        })
    "
    @close="switchComponent('profile')"
  />

  <ChangeCredentialComponent
    v-if="activeComponent === 'pincode'"
    label="pincode"
    :submitFunction="
      (data) =>
        userService.changePincode({
          currentPincode: data.current,
          newPincode: data.new,
          repeatNewPincode: data.repeat,
        })
    "
    @close="switchComponent('profile')"
  />
</template>

<style scoped>
form {
  max-width: 400px;
  margin: 40px auto 0 auto;
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
