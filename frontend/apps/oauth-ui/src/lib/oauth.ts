/**
 * OAuth2 utilities for managing authorization flow
 */

import {
  generateCodeVerifier,
  generateCodeChallenge,
  generateState,
  storePkceParams,
  getPkceParams,
  clearPkceParams,
  validateState,
} from "./pkce";

/**
 * OAuth query parameters passed to the OAuth UI
 */
export interface OAuthParams {
  clientId: string;
  redirectUri: string;
  codeChallenge: string;
  codeChallengeMethod: string;
  state?: string;
  scope?: string;
}

/**
 * Parse OAuth parameters from URL search params
 */
export function parseOAuthParams(searchParams: URLSearchParams): OAuthParams | null {
  const clientId = searchParams.get("client_id");
  const redirectUri = searchParams.get("redirect_uri");
  const codeChallenge = searchParams.get("code_challenge");
  const codeChallengeMethod = searchParams.get("code_challenge_method");

  if (!clientId || !redirectUri || !codeChallenge || !codeChallengeMethod) {
    return null;
  }

  return {
    clientId,
    redirectUri,
    codeChallenge,
    codeChallengeMethod,
    state: searchParams.get("state") || undefined,
    scope: searchParams.get("scope") || undefined,
  };
}

/**
 * Store OAuth parameters in sessionStorage for use across pages
 */
const OAUTH_PARAMS_KEY = "oauth_params";

export function storeOAuthParams(params: OAuthParams): void {
  sessionStorage.setItem(OAUTH_PARAMS_KEY, JSON.stringify(params));
}

export function getStoredOAuthParams(): OAuthParams | null {
  const stored = sessionStorage.getItem(OAUTH_PARAMS_KEY);
  if (!stored) return null;
  try {
    return JSON.parse(stored) as OAuthParams;
  } catch {
    return null;
  }
}

export function clearOAuthParams(): void {
  sessionStorage.removeItem(OAUTH_PARAMS_KEY);
}

/**
 * Build the authorization URL to redirect back to OAuth UI's authorize page after login
 */
export function buildAuthorizeUrl(
  params: OAuthParams,
  sessionId: string
): string {
  // Use OAuth UI's own /authorize page, not the backend API
  const url = new URL(`${window.location.origin}/authorize`);
  url.searchParams.set("client_id", params.clientId);
  url.searchParams.set("redirect_uri", params.redirectUri);
  url.searchParams.set("code_challenge", params.codeChallenge);
  url.searchParams.set("code_challenge_method", params.codeChallengeMethod);
  url.searchParams.set("session_id", sessionId);
  if (params.state) {
    url.searchParams.set("state", params.state);
  }
  if (params.scope) {
    url.searchParams.set("scope", params.scope);
  }
  return url.toString();
}

/**
 * Build the client callback URL with authorization code
 */
export function buildCallbackUrl(
  redirectUri: string,
  code: string,
  state?: string
): string {
  const url = new URL(redirectUri);
  url.searchParams.set("code", code);
  if (state) {
    url.searchParams.set("state", state);
  }
  return url.toString();
}

// Re-export PKCE utilities
export {
  generateCodeVerifier,
  generateCodeChallenge,
  generateState,
  storePkceParams,
  getPkceParams,
  clearPkceParams,
  validateState,
};
