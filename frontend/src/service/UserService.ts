import { ApiService } from '@/service/ApiService.ts'
import type { LoginDTO } from '@/DTO/request/User/LoginDTO.ts'
import type { RegisterPatientDTO } from '@/DTO/request/User/RegisterPatientDTO.ts'
import type { ApiResponse } from '@/DTO/response/ApiResponse.ts'
import type { LoginResponse } from '@/DTO/response/LoginResponse.ts'
import type { UserProfileResponse } from '@/DTO/response/UserProfileResponse.ts'
import type { EditUserDTO } from '@/DTO/request/User/EditUserDTO.ts'
import type { ChangePasswordDTO } from '@/DTO/request/User/ChangePasswordDTO.ts'
import type { Page } from '@/DTO/response/Page.ts'
import type { ChangePincodeDTO } from '@/DTO/request/User/ChangePincodeDTO.ts'
import type { PatientDetailResponse } from '@/DTO/response/PatientDetailResponse.ts'

export class UserService {
  private ApiService: ApiService

  constructor() {
    this.ApiService = new ApiService()
  }

  public async login(credentials: LoginDTO): Promise<ApiResponse<LoginResponse>> {
    return await this.ApiService.post<LoginDTO, ApiResponse<LoginResponse>>(
      'auth/login',
      credentials,
    )
  }

  public async registerPatient(registerData: RegisterPatientDTO): Promise<ApiResponse<null>> {
    return await this.ApiService.post<RegisterPatientDTO, ApiResponse<null>>(
      'auth/register-patient',
      registerData,
    )
  }

  public async getLoggedInUser(): Promise<ApiResponse<UserProfileResponse>> {
    return await this.ApiService.get('user/me')
  }

  public async editUserProfile(newUserData: EditUserDTO): Promise<ApiResponse<null>> {
    return await this.ApiService.post<EditUserDTO, ApiResponse<null>>('user/edit-me', newUserData)
  }

  public async changePassword(data: ChangePasswordDTO): Promise<ApiResponse<null>> {
    return await this.ApiService.post<ChangePasswordDTO, ApiResponse<null>>(
      'auth/change-password',
      data,
    )
  }

  public async changePincode(data: ChangePincodeDTO): Promise<ApiResponse<null>> {
    return await this.ApiService.post<ChangePincodeDTO, ApiResponse<null>>(
      'auth/change-pincode',
      data,
    )
  }

  public async getAllPatients(
    page: number,
    size: number,
  ): Promise<ApiResponse<Page<UserProfileResponse>>> {
    return await this.ApiService.get(`user/get-all-patients?page=${page}&size=${size}`)
  }

  public async deleteUser(userId: number): Promise<ApiResponse<null>> {
    return await this.ApiService.post(`user/delete-user/${userId}`, userId)
  }

  public async fetchPatientDetail(userId: number): Promise<ApiResponse<PatientDetailResponse>> {
    return await this.ApiService.get(`user/get-patient-detail/${userId}`)
  }
}
