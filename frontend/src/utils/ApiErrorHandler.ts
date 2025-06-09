import axios from 'axios';

export type ApiErrorResponse<T = null> = {
  status: number;
  error: string;
  message: string;
  timestamp: string;
  details: T;
};

export function handleApiError(error: unknown): ApiErrorResponse | null {
  if (axios.isAxiosError(error)) {
    const errData = error.response?.data as ApiErrorResponse;
    return errData ?? null;
  }

  console.error("Unexpected error:", error);
  return null;
}
