// ==========================================
// OAuth API Request/Response Types
// ==========================================

// ----- Login -----
export interface LoginRequest {
  email: string;
  password: string;
  clientId: string;
}

export interface LoginResponse {
  sessionId: string;
  email: string;
}

// ----- Register -----
export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
}

export interface RegisterResponse {
  message: string;
  userId: string;
}

// ----- Authorize -----
export interface AuthorizeParams {
  client_id: string;
  redirect_uri: string;
  code_challenge: string;
  code_challenge_method: string;
  state?: string;
  scope?: string;
  session_id?: string;
}

export interface AuthorizeResponse {
  requiresAuthentication: boolean;
  requiresConsent: boolean;
  authorizationCode?: string;
  state?: string;
  expiresIn: number;
}

// ----- Token -----
export interface TokenRequest {
  grant_type: "authorization_code" | "refresh_token";
  code?: string;
  code_verifier?: string;
  client_id: string;
  client_secret?: string;
  redirect_uri?: string;
  refresh_token?: string;
}

export interface TokenResponse {
  access_token: string;
  refresh_token: string;
  token_type: "Bearer";
  expires_in: number;
}

// ----- Email Verification -----
export interface VerifyEmailRequest {
  token: string;
  email: string;
}

export interface VerifyEmailResponse {
  message: string;
}

// ----- Forgot Password -----
export interface ForgotPasswordRequest {
  email: string;
}

export interface ForgotPasswordResponse {
  message: string;
}

// ----- Reset Password -----
export interface ResetPasswordRequest {
  token: string;
  email: string;
  newPassword: string;
}

export interface ResetPasswordResponse {
  message: string;
}

// ----- User Info -----
export interface UserInfo {
  sub: string;
  email: string;
  email_verified: boolean;
  name?: string;
  given_name?: string;
  family_name?: string;
}

// ----- Logout -----
export interface LogoutRequest {
  sessionId: string;
}

export interface LogoutResponse {
  message: string;
}
