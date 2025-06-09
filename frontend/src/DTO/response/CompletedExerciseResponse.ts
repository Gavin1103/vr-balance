import type { ExerciseEnum } from '@/enums/ExerciseEnum.ts'
import type { DifficultyEnum } from '@/enums/DifficultyEnum.ts'

export type CompletedExerciseResponse = {
  exercise: ExerciseEnum,
  earnedPoints: number,
  difficulty: DifficultyEnum,
  completedAt: string
}
