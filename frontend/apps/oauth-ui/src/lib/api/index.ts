import { api } from "./client";
import type {
  LoginRequest,
  LoginResponse,
  RegisterRequest,
  RegisterResponse,
  AuthorizeParams,
  AuthorizeResponse,
  ForgotPasswordRequest,
  ForgotPasswordResponse,
  ResetPasswordRequest,
  ResetPasswordResponse,
  VerifyEmailRequest,
  VerifyEmailResponse,
  LogoutRequest,
  LogoutResponse,
} from "./types";

/**
 * OAuth API endpoints
 */
export const oauthApi = {
  /**
   * Login user (internal endpoint for OAuth UI)
   */
  login: async (data: LoginRequest): Promise<LoginResponse> => {
    const response = await api.post<LoginResponse>("/api/oauth/login", data);
    return response.data;
  },

  /**
   * Register new user
   */
  register: async (data: RegisterRequest): Promise<RegisterResponse> => {
    const response = await api.post<RegisterResponse>("/api/oauth/register", data);
    return response.data;
  },

  /**
   * Get authorization (check if authenticated)
   */
  authorize: async (params: AuthorizeParams): Promise<AuthorizeResponse> => {
    const response = await api.get<AuthorizeResponse>("/api/oauth/authorize", {
      params,
    });
    return response.data;
  },

  /**
   * Request password reset email
   */
  forgotPassword: async (data: ForgotPasswordRequest): Promise<ForgotPasswordResponse> => {
    const response = await api.post<ForgotPasswordResponse>(
      "/api/oauth/forgot-password",
      data
    );
    return response.data;
  },

  /**
   * Reset password with token
   */
  resetPassword: async (data: ResetPasswordRequest): Promise<ResetPasswordResponse> => {
    const response = await api.post<ResetPasswordResponse>(
      "/api/oauth/reset-password",
      data
    );
    return response.data;
  },

  /**
   * Verify email with token
   */
  verifyEmail: async (data: VerifyEmailRequest): Promise<VerifyEmailResponse> => {
    const response = await api.post<VerifyEmailResponse>(
      "/api/oauth/verify-email",
      data
    );
    return response.data;
  },

  /**
   * Logout (clear session)
   */
  logout: async (data: LogoutRequest): Promise<LogoutResponse> => {
    const response = await api.post<LogoutResponse>("/api/oauth/logout", data);
    return response.data;
  },
};

export * from "./types";
export { api, getErrorMessage, isApiError } from "./client";
