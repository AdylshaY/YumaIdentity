import { AuthorizeResponse } from '../types/auth';

export const handleOAuthRedirect = (
  redirectUri: string,
  authResponse: AuthorizeResponse,
  state?: string | null
) => {
  if (authResponse.authorizationCode) {
    const redirectUrl = new URL(redirectUri);
    redirectUrl.searchParams.append('code', authResponse.authorizationCode);
    if (state) {
      redirectUrl.searchParams.append('state', state);
    }
    window.location.href = redirectUrl.toString();
  } else if (authResponse.requiresConsent) {
    // TODO: Handle consent screen redirection
    console.log('Consent required');
  } else {
    throw new Error('Authorization failed');
  }
};
