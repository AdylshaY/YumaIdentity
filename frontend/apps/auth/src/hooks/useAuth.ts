import { useMutation } from '@tanstack/react-query';
import { AuthService } from '../services/auth.service';
import { LoginRequest, AuthorizeRequest } from '../types/auth';
import { useOAuthParams } from './useOAuthParams';
import { handleOAuthRedirect } from '../lib/oauth';

export const useAuth = () => {
  const { validateParams } = useOAuthParams();

  const mutation = useMutation({
    mutationFn: async (credentials: Omit<LoginRequest, 'clientId'>) => {
      const params = validateParams();

      // 1. Login to get session
      const loginRequest: LoginRequest = {
        ...credentials,
        clientId: params.clientId,
      };

      const loginResponse = await AuthService.login(loginRequest);

      // Save session ID
      sessionStorage.setItem('yuma_session_id', loginResponse.sessionId);

      // 2. Call authorize with session
      const authorizeParams: AuthorizeRequest = {
        client_id: params.clientId,
        redirect_uri: params.redirectUri,
        code_challenge: params.codeChallenge,
        code_challenge_method: params.codeChallengeMethod,
        state: params.state || undefined,
        scope: params.scope || undefined,
        session_id: loginResponse.sessionId,
        response_type: 'code',
      };

      const authResponse = await AuthService.authorize(authorizeParams);

      return {
        authResponse,
        redirectUri: params.redirectUri,
        state: params.state,
      };
    },
    onSuccess: ({ authResponse, redirectUri, state }) => {
      handleOAuthRedirect(redirectUri, authResponse, state);
    },
  });

  return {
    login: mutation.mutateAsync,
    isLoading: mutation.isPending,
    error: mutation.error
      ? mutation.error instanceof Error
        ? mutation.error.message
        : 'An error occurred'
      : null,
  };
};
