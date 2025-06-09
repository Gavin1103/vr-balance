import axios from 'axios'
import { CookieService } from './CookieService.ts'
import type { ApiResponse } from '@/response/ApiResponse.ts'

export class ApiService {
  private readonly baseUrl: string
  private readonly token: string | null
  private cookieService = new CookieService()

  constructor() {
    this.baseUrl = import.meta.env.VITE_API_BASE_URL
    this.token = this.cookieService.getCookie(this.cookieService.accessTokenAlias)
  }

  public getBaseUrl(): string {
    return this.baseUrl
  }

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

  async delete(endpoint: string): Promise<void> {
    try {
      await axios.delete(`${this.baseUrl}/${endpoint}`, this.getHeaders())
    } catch (error) {
      console.error('Error in DELETE request:', error)
      throw error
    }
  }

  private getHeaders() {
    return {
      headers: {
        Authorization: this.token ? `Bearer ${this.token}` : '',
        'Content-Type': 'application/json',
      },
    }
  }
}
