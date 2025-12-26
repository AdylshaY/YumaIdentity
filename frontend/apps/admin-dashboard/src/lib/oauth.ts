import {
  generateCodeVerifier,
  generateCodeChallenge,
  generateState,
  storePkceParams,
} from './pkce';

const OAUTH_URL = import.meta.env.VITE_OAUTH_URL;
const CLIENT_ID = import.meta.env.VITE_OAUTH_CLIENT_ID;
const REDIRECT_URI = import.meta.env.VITE_OAUTH_REDIRECT_URI;

/**
 * Initiate OAuth2 Authorization Code Flow with PKCE
 * Redirects the user to the OAuth authorization server
 */
export async function initiateOAuthFlow(): Promise<void> {
  const codeVerifier = generateCodeVerifier();
  const codeChallenge = await generateCodeChallenge(codeVerifier);
  const state = generateState();

  storePkceParams({ codeVerifier, state });

  const params = new URLSearchParams({
    response_type: 'code',
    client_id: CLIENT_ID,
    redirect_uri: REDIRECT_URI,
    scope: 'openid profile email',
    state,
    code_challenge: codeChallenge,
    code_challenge_method: 'S256',
  });

  window.location.href = `${OAUTH_URL}/authorize?${params.toString()}`;
}

/**
 * Check if current URL has OAuth callback parameters
 */
export function isOAuthCallback(): boolean {
  const params = new URLSearchParams(window.location.search);
  return params.has('code') && params.has('state');
}

/**
 * Get OAuth callback parameters from URL
 */
export function getCallbackParams(): { code: string; state: string } | null {
  const params = new URLSearchParams(window.location.search);
  const code = params.get('code');
  const state = params.get('state');

  if (code && state) {
    return { code, state };
  }
  return null;
}
