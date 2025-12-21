/**
 * PKCE (Proof Key for Code Exchange) utilities for OAuth2 Authorization Code Flow
 * Implements RFC 7636: https://tools.ietf.org/html/rfc7636
 */

/**
 * Generate a cryptographically random code verifier (43-128 characters)
 * Base64URL encoded without padding
 */
export function generateCodeVerifier(): string {
  const array = new Uint8Array(32);
  crypto.getRandomValues(array);
  return base64UrlEncode(array);
}

/**
 * Generate code challenge from verifier using SHA-256
 * Base64URL encoded without padding
 */
export async function generateCodeChallenge(verifier: string): Promise<string> {
  const encoder = new TextEncoder();
  const data = encoder.encode(verifier);
  const hash = await crypto.subtle.digest("SHA-256", data);
  return base64UrlEncode(new Uint8Array(hash));
}

/**
 * Generate a random state parameter for CSRF protection
 */
export function generateState(): string {
  const array = new Uint8Array(16);
  crypto.getRandomValues(array);
  return base64UrlEncode(array);
}

/**
 * Base64URL encode (RFC 4648) without padding
 */
function base64UrlEncode(array: Uint8Array): string {
  const base64 = btoa(String.fromCharCode(...array));
  return base64.replace(/\+/g, "-").replace(/\//g, "_").replace(/=/g, "");
}

/**
 * PKCE Storage keys
 */
const STORAGE_KEYS = {
  CODE_VERIFIER: "oauth_code_verifier",
  STATE: "oauth_state",
  REDIRECT_URI: "oauth_redirect_uri",
} as const;

/**
 * Store PKCE parameters in sessionStorage
 */
export function storePkceParams(params: {
  codeVerifier: string;
  state: string;
  redirectUri?: string;
}): void {
  sessionStorage.setItem(STORAGE_KEYS.CODE_VERIFIER, params.codeVerifier);
  sessionStorage.setItem(STORAGE_KEYS.STATE, params.state);
  if (params.redirectUri) {
    sessionStorage.setItem(STORAGE_KEYS.REDIRECT_URI, params.redirectUri);
  }
}

/**
 * Retrieve stored PKCE parameters from sessionStorage
 */
export function getPkceParams(): {
  codeVerifier: string | null;
  state: string | null;
  redirectUri: string | null;
} {
  return {
    codeVerifier: sessionStorage.getItem(STORAGE_KEYS.CODE_VERIFIER),
    state: sessionStorage.getItem(STORAGE_KEYS.STATE),
    redirectUri: sessionStorage.getItem(STORAGE_KEYS.REDIRECT_URI),
  };
}

/**
 * Clear PKCE parameters from sessionStorage
 */
export function clearPkceParams(): void {
  sessionStorage.removeItem(STORAGE_KEYS.CODE_VERIFIER);
  sessionStorage.removeItem(STORAGE_KEYS.STATE);
  sessionStorage.removeItem(STORAGE_KEYS.REDIRECT_URI);
}

/**
 * Validate that stored state matches the returned state
 */
export function validateState(returnedState: string): boolean {
  const storedState = sessionStorage.getItem(STORAGE_KEYS.STATE);
  return storedState !== null && storedState === returnedState;
}
