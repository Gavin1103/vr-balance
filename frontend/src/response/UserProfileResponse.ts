export type UserProfileResponse = {
  email: string;
  username: string;
  phoneNumber: string;
  firstName: string;
  lastName: string;
  role: 'PATIENT' | 'PHYSIOTHERAPIST' | 'ADMIN';
  birthDate: string;
};
