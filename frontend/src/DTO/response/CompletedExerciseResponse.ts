import type { ExerciseEnum } from '@/enums/ExerciseEnum.ts'
import type { DifficultyEnum } from '@/enums/DifficultyEnum.ts'

export type CompletedExerciseResponse = {
  id: number
  exercise: ExerciseEnum,
  earnedPoints: number,
  difficulty: DifficultyEnum,
  completedAt: string
}
