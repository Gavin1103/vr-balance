import { useToast } from 'primevue/usetoast'
import { handleApiError } from '@/utils/ApiErrorHandler.ts'
import type { ApiResponse } from '@/DTO/response/ApiResponse.ts'

/**
 * A composable for displaying toast notifications based on API responses.
 *
 * This utility uses PrimeVue's `useToast()` to provide user feedback for both
 * successful and failed API interactions. It standardizes how toasts are shown
 * throughout the application and ensures consistent messaging.
 *
 * ### Usage example:
 * ```ts
 * const { onSuccess, onError } = useApiFeedback()
 *
 * try {
 *   const response = await someApiCall()
 *   onSuccess(response)
 * } catch (error) {
 *   onError(error)
 * }
 * ```
 *
 * @returns {{
 *   onSuccess: (result: ApiResponse<null>) => void,
 *   onError: (error: unknown) => void
 * }}
 * - `onSuccess`: Displays a success or info toast depending on the response status (200, 201, or other).
 * - `onError`: Displays an error toast based on the error's HTTP status code. Covers common codes like 400, 401, 403, 409, and fallback for unknown errors.
 */
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
