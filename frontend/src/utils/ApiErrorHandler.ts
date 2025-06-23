import axios from 'axios';

/**
 * Represents the structure of an error response returned by the API.
 *
 * @template T - Optional type for the `details` field, default is `null`.
 */
export type ApiErrorResponse<T = null> = {
  status: number;       // HTTP status code (e.g. 400, 401, 500)
  error: string;        // Short error name (e.g. "Bad Request", "Unauthorized")
  message: string;      // Detailed explanation of the error
  timestamp: string;    // Timestamp when the error occurred (ISO format)
  details: T;           // Optional additional data (can be null)
};

/**
 * Handles an unknown error and attempts to extract a typed {@link ApiErrorResponse}
 * from an Axios error response.
 *
 * @param error - The error thrown from an Axios request or any other source.
 * @returns An {@link ApiErrorResponse} object if available, otherwise `null`.
 *
 * @example
 * try {
 *   await axios.get("/api/data");
 * } catch (error) {
 *   const apiError = handleApiError(error);
 *   if (apiError) {
 *     console.error(apiError.message);
 *   }
 * }
 */
export function handleApiError(error: unknown): ApiErrorResponse | null {
  if (axios.isAxiosError(error)) {
    const errData = error.response?.data as ApiErrorResponse;
    return errData ?? null;
  }

  console.error("Unexpected error:", error);
  return null;
}
