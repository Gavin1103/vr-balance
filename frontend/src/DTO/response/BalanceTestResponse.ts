import type { Vector3DTO } from '@/DTO/Vector3DTO.ts'

export type BalanceTestResponse = {
  completedAt: string
  phase_1: Vector3DTO[]
  phase_2: Vector3DTO[]
  phase_3: Vector3DTO[]
  phase_4: Vector3DTO[]
}
