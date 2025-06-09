import { useToast } from 'primevue/usetoast'
import { handleApiError } from '@/utils/ApiErrorHandler.ts'
import type { ApiResponse } from '@/response/ApiResponse.ts'

export function useApiFeedback() {
  const toast = useToast()

  function onSuccess(result: ApiResponse<null>) {
    const message = result.message
    const status = result.status

    switch (status) {
      case 200:
        toast.add({
          severity: 'success',
          summary: 'Success',
          detail: message || 'Successfully executed',
          life: 5000,
        })
        break

      case 201:
        toast.add({
          severity: 'success',
          summary: 'Success',
          detail: message || 'Successfully created',
          life: 5000,
        })
        break

      default:
        toast.add({
          severity: 'info',
          summary: 'Info',
          detail: message || 'Action completed',
          life: 5000,
        })
    }
  }

  function onError(error: unknown) {
    const err = handleApiError(error)
    const status = err?.status
    const message = err?.message

    switch (status) {
      case 400:
        toast.add({
          severity: 'error',
          summary: 'Bad Request',
          detail: message,
          life: 5000,
        })
        break
      case 401:
        toast.add({
          severity: 'error',
          summary: 'Unauthorized',
          detail: message || 'You must be logged in',
          life: 5000,
        })
        break
      case 403:
        toast.add({
          severity: 'error',
          summary: 'Forbidden',
          detail: message || 'You are not allowed to perform this action',
          life: 5000,
        })
        break
      case 409:
        toast.add({
          severity: 'error',
          summary: 'Conflict',
          detail: message,
          life: 5000,
        })
        break
      default:
        toast.add({
          severity: 'error',
          summary: 'Error',
          detail: message || 'Something went wrong, please try again.',
          life: 5000,
        })
    }
  }
  return { onSuccess, onError }
}
