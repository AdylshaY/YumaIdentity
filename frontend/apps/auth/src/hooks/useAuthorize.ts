import { useEffect, useState } from 'react';
import { AuthService } from '../services/auth.service';
import { AuthorizeRequest } from '../types/auth';
import { useOAuthParams } from './useOAuthParams';
import { handleOAuthRedirect } from '../lib/oauth';

export const useAuthorize = () => {
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const { validateParams, searchParams } = useOAuthParams();

  useEffect(() => {
    const handleAuthorize = async () => {
      try {
        const params = validateParams();

        // Try to get session from storage
        const sessionId = sessionStorage.getItem('yuma_session_id');

        const authorizeParams: AuthorizeRequest = {
          client_id: params.clientId,
          redirect_uri: params.redirectUri,
          code_challenge: params.codeChallenge,
          code_challenge_method: params.codeChallengeMethod,
          state: params.state || undefined,
          scope: params.scope || undefined,
          session_id: sessionId || undefined,
          response_type: 'code',
        };

        const authResponse = await AuthService.authorize(authorizeParams);

        if (authResponse.requiresAuthentication) {
          // Redirect to login with current params
          const loginUrl = new URL('/login', window.location.origin);
          searchParams.forEach((value, key) => {
            loginUrl.searchParams.append(key, value);
          });
          window.location.href = loginUrl.toString();
        } else {
          handleOAuthRedirect(params.redirectUri, authResponse, params.state);
        }
      } catch (err) {
        setError(err instanceof Error ? err.message : 'An error occurred');
        setIsLoading(false);
      }
    };

    handleAuthorize();
  }, []);

  return { isLoading, error };
};
