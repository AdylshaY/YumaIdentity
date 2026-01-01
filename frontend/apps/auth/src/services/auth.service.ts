import { api } from '../lib/api';
import {
  AuthorizeRequest,
  AuthorizeApiRequest,
  AuthorizeResponse,
  LoginRequest,
  LoginResponse,
  RegisterRequest,
} from '../types/auth';

export const AuthService = {
  authorize: async (params: AuthorizeRequest): Promise<AuthorizeResponse> => {
    // Map snake_case URL params to snake_case API params
    const apiParams: AuthorizeApiRequest = {
      client_id: params.client_id,
      redirect_uri: params.redirect_uri,
      code_challenge: params.code_challenge || '',
      code_challenge_method: params.code_challenge_method || 'S256',
      state: params.state,
      scope: params.scope,
      session_id: params.session_id,
    };

    const { data } = await api.get<AuthorizeResponse>('/OAuth/authorize', {
      params: apiParams,
    });
    return data;
  },

  login: async (data: LoginRequest): Promise<LoginResponse> => {
    const response = await api.post<LoginResponse>('/OAuth/login', data);
    return response.data;
  },

  register: async (data: RegisterRequest): Promise<void> => {
    await api.post('/OAuth/register', data);
  },
};
