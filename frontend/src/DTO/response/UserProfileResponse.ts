export type UserProfileResponse = {
  id: number,
  email: string;
  username: string;
  phoneNumber: string;
  firstName: string;
  lastName: string;
  role: 'PATIENT' | 'PHYSIOTHERAPIST' | 'ADMIN';
  birthDate: string;
};
