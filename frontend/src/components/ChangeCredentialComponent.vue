<script setup lang="ts">
import { ref } from 'vue'
import { useToast } from 'primevue/usetoast'
import { useApiFeedback } from '@/composables/useApiFeedback.ts'
import type { ApiResponse } from '@/DTO/response/ApiResponse.ts'

/**
 * Props:
 * @param {string} label - The name of the credential being changed (e.g., "password", "pincode").
 * @param {Function} submitFunction - The async function (API call) responsible for submitting the new credentials to the backend.
 */
interface Props {
  label: string
  submitFunction: (data: {
    current: string
    new: string
    repeat: string
  }) => Promise<ApiResponse<null>>
}

const props = defineProps<Props>()

// Emits the close event to the parent component
const emit = defineEmits(['close'])

const toast = useToast()
const { onSuccess, onError } = useApiFeedback()

// Form fields
const current = ref('')
const newCredential = ref('')
const repeat = ref('')

// TODO: validate new password on characters and length
// TODO: validate new pincode on characters and length
/**
 * Validates the form inputs and submits the data.
 * Shows error messages when the new credential matches the current one,
 * or when the repeated input does not match the new one.
 */
async function handleSubmit() {
  if (newCredential.value !== repeat.value) {
    toast.add({
      severity: 'error',
      summary: 'Mismatch',
      detail: `New ${props.label} and repeat ${props.label} do not match`,
      life: 5000,
    })
    return
  }

  if (newCredential.value === current.value) {
    toast.add({
      severity: 'error',
      summary: 'Invalid',
      detail: `New ${props.label} must be different from the current ${props.label}`,
      life: 5000,
    })
    return
  }

  try {
    const result = await props.submitFunction({
      current: current.value,
      new: newCredential.value,
      repeat: repeat.value,
    })
    onSuccess(result)
    emit('close')
  } catch (error) {
    onError(error)
  }
}
</script>

<template>
  <form @submit.prevent="handleSubmit">
    <div class="title-container">
      <h2>Change {{ label }}</h2>
      <div @click="$emit('close')" class="close-icon">X</div>
    </div>

    <label :for="'current-' + label">Current {{ label }}:</label>
    <input
      :id="'current-' + label"
      type="text"
      required
      v-model="current"
      :placeholder="'current ' + label"
    />

    <label :for="'new-' + label">New {{ label }}:</label>
    <input
      :id="'new-' + label"
      type="text"
      required
      v-model="newCredential"
      :placeholder="'new' + label"
    />

    <label :for="'repeat-' + label">Repeat new {{ label }}:</label>
    <input
      :id="'repeat-' + label"
      type="text"
      required
      v-model="repeat"
      :placeholder="'repeat ' + label"
    />

    <button type="submit">Change {{ label }}</button>
  </form>
</template>

<style scoped>
form {
  max-width: 400px;
  margin: 40px auto;
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
