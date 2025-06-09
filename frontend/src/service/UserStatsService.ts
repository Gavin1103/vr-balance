import { ApiService } from '@/service/ApiService.ts'
import type { ApiResponse } from '@/DTO/response/ApiResponse.ts'
import type { Page } from '@/DTO/response/Page.ts'
import type { UserStatsResponse } from '@/DTO/response/UserStatsResponse.ts'

export class UserStatsService {
  private ApiService: ApiService

  constructor() {
    this.ApiService = new ApiService()
  }

  public async getLeaderboard(
    page: number,
    size: number,
    sortBy: string,
    direction: string,
  ): Promise<ApiResponse<Page<UserStatsResponse>>> {
    return await this.ApiService.get(
      `user-stats/public/leaderboard?page=${page}&size=${size}&sortBy=${sortBy}&direction=${direction}`,
    )
  }
}
