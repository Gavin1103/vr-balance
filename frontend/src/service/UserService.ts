import { ApiService } from '@/service/ApiService.ts'
import type { LoginDTO } from '@/request/User/LoginDTO.ts'
import type { RegisterPatientDTO } from '@/request/User/RegisterPatientDTO.ts'
import type { ApiResponse } from '@/response/ApiResponse.ts'
import type { LoginResponse } from '@/response/LoginResponse.ts'
import type { UserProfileResponse } from '@/response/UserProfileResponse.ts'
import type { EditUserDTO } from '@/request/User/EditUserDTO.ts'
import type { ChangePasswordDTO } from '@/request/User/ChangePasswordDTO.ts'
import type { Page } from '@/response/Page.ts'
import type { ChangePincodeDTO } from '@/request/User/ChangePincodeDTO.ts'

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

  // TODO: use pagination (Already implemented in the backend)
  public async getAllUsers(page: number, size: number): Promise<ApiResponse<Page<UserProfileResponse>>> {
    return await this.ApiService.get(`user/get-all-users?page=${page}&size=${size}`)
  }
}
