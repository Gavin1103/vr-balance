import axios from 'axios'
import { CookieService } from './CookieService.ts'
import type { ApiResponse } from '@/DTO/response/ApiResponse.ts'

/**
 * ApiService is a centralized service for making HTTP requests to the backend API.
 *
 * It handles:
 * - Base URL resolution from environment variables
 * - Automatic inclusion of JWT tokens (retrieved from cookies)
 * - Consistent header setup for GET, POST, and DELETE requests
 * - Type-safe response handling via generics
 *
 * This service simplifies API interaction and ensures authentication headers are always present when needed.
 */
export class ApiService {
  private readonly baseUrl: string
  private readonly token: string | null
  private cookieService = new CookieService()

  /**
   * Initializes the ApiService with the base URL from environment variables
   * and retrieves the JWT token from cookies using CookieService.
   */
  constructor() {
    this.baseUrl = import.meta.env.VITE_API_BASE_URL
    this.token = this.cookieService.getCookie(this.cookieService.accessTokenAlias)
  }

  /**
   * Returns the base URL for the current environment.
   * Useful for debugging or constructing URLs elsewhere.
   *
   * @returns {string} the API base URL
   */
  public getBaseUrl(): string {
    return this.baseUrl
  }

  /**
   * Sends a GET request to the given endpoint with optional authorization headers.
   *
   * @param endpoint - the relative endpoint (e.g., "api/user/me")
   * @returns {Promise<ApiResponse<T>>} the typed API response
   * @throws will log and rethrow any HTTP or network errors
   */
  async get<T>(endpoint: string): Promise<ApiResponse<T>> {
    try {
      const response = await axios.get<ApiResponse<T>>(
        `${this.baseUrl}/${endpoint}`,
        this.getHeaders(),
      )
      return response.data
    } catch (error) {
      console.error('Error in GET request:', error)
      throw error
    }
  }

  /**
   * Sends a POST request to the given endpoint with a typed request body.
   *
   * @param endpoint - the relative endpoint (e.g., "api/auth/login")
   * @param payload - the request body
   * @returns {Promise<Res>} the typed response
   * @throws will log and rethrow any HTTP or network errors
   */
  async post<Req, Res>(endpoint: string, payload: Req): Promise<Res> {
    try {
      const response = await axios.post<Res>(
        `${this.baseUrl}/${endpoint}`,
        payload,
        this.getHeaders(),
      )
      return response.data
    } catch (error) {
      console.error('Error in POST request:', error)
      throw error
    }
  }

  /**
   * Sends a DELETE request to the given endpoint.
   *
   * @param endpoint - the relative endpoint to delete (e.g., "api/user/5")
   * @returns {Promise<void>}
   * @throws will log and rethrow any HTTP or network errors
   */
  async delete(endpoint: string): Promise<void> {
    try {
      await axios.delete(`${this.baseUrl}/${endpoint}`, this.getHeaders())
    } catch (error) {
      console.error('Error in DELETE request:', error)
      throw error
    }
  }

  /**
   * Constructs the headers object for HTTP requests, including
   * the Authorization header with the JWT token if available.
   *
   * @returns an object with standard headers including Authorization and Content-Type
   */
  private getHeaders() {
    return {
      headers: {
        Authorization: this.token ? `Bearer ${this.token}` : '',
        'Content-Type': 'application/json',
      },
    }
  }

  /**
   * Sends a GET request to download a file (e.g., ZIP, PDF).
   *
   * @param endpoint - the relative endpoint to download from
   * @returns {Promise<Blob>} the file as a Blob
   * @throws will log and rethrow any HTTP or network errors
   */
  async downloadFile(endpoint: string): Promise<Blob> {
    try {
      const response = await axios.get(`${this.baseUrl}/${endpoint}`, {
        responseType: 'blob',
        headers: {
          Authorization: this.token ? `Bearer ${this.token}` : '',
        },
      })
      return response.data
    } catch (error) {
      console.error('Error in file download:', error)
      throw error
    }
  }
}

