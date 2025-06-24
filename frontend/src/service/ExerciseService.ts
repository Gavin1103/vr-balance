import { ApiService } from '@/service/ApiService.ts'

export class ExerciseService {
  private ApiService: ApiService

  constructor() {
    this.ApiService = new ApiService()
  }

  public async downloadBalanceTestsResult(balanceTestId: number) {
    const blob = await this.ApiService.downloadFile(`exercise/download-balance-test/${balanceTestId}`)

    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.setAttribute('download', 'balance-test-results.zip')
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    window.URL.revokeObjectURL(url)
  }
}
