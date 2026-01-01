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
    // Map snake_case URL params to PascalCase API params
    const apiParams: AuthorizeApiRequest = {
      ClientId: params.client_id,
      RedirectUri: params.redirect_uri,
      CodeChallenge: params.code_challenge || '',
      CodeChallengeMethod: params.code_challenge_method || 'S256',
      State: params.state,
      Scope: params.scope,
      SessionId: params.session_id,
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
