import type { UserProfileResponse } from '@/DTO/response/UserProfileResponse.ts'
import type { CompletedExerciseResponse } from '@/DTO/response/CompletedExerciseResponse.ts'
import type { BalanceTestResponse } from '@/DTO/response/BalanceTestResponse.ts'

export type PatientDetailResponse = {
  user: UserProfileResponse
  recentExercises: CompletedExerciseResponse[],
  downloadLast10Exercises: CompletedExerciseResponse[],
  recentBalanceTests: BalanceTestResponse[],
}
