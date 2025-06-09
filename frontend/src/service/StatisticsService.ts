import { ApiService } from '@/service/ApiService.ts'
import type { ApiResponse } from '@/DTO/response/ApiResponse.ts'
import type { ExerciseStatsResponse } from '@/DTO/response/ExerciseStatsResponse.ts'

export class StatisticsService {
  private ApiService: ApiService

  constructor() {
    this.ApiService = new ApiService()
  }

  public async getAllTimeExerciseStats(): Promise<ApiResponse<ExerciseStatsResponse[]>> {
    return await this.ApiService.get(`statistics/public/exercise`)
  }

  public async getExerciseStatsFromLast30days(): Promise<ApiResponse<ExerciseStatsResponse[]>> {
    return await this.ApiService.get(`statistics/public/exercise/last-30-days`)
  }
}
