export const useOAuthParams = () => {
  const searchParams = new URLSearchParams(window.location.search);
  const clientId = searchParams.get('client_id');
  const redirectUri = searchParams.get('redirect_uri');
  const codeChallenge = searchParams.get('code_challenge');
  const codeChallengeMethod = searchParams.get('code_challenge_method');
  const state = searchParams.get('state');
  const scope = searchParams.get('scope');

  const validateParams = () => {
    if (!clientId || !redirectUri || !codeChallenge || !codeChallengeMethod) {
      throw new Error('Missing required OAuth parameters');
    }
    return {
      clientId,
      redirectUri,
      codeChallenge,
      codeChallengeMethod,
      state,
      scope,
    };
  };

  return {
    clientId,
    redirectUri,
    codeChallenge,
    codeChallengeMethod,
    state,
    scope,
    searchParams,
    validateParams,
  };
};
