import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import { Loader2, CheckCircle, XCircle } from "lucide-react";

import { AuthLayout } from "../components/layouts/AuthLayout";
import { oauthApi, getErrorMessage } from "../lib/api";
import {
  getStoredOAuthParams,
  clearOAuthParams,
  buildCallbackUrl,
} from "../lib/oauth";

/**
 * This page handles the OAuth authorize response.
 * After successful login, the user is redirected to /api/oauth/authorize with session_id.
 * The backend returns an authorization code which we then pass to the client's redirect_uri.
 */
export function AuthorizePage() {
  const [searchParams] = useSearchParams();
  const [error, setError] = useState<string | null>(null);
  const [isRedirecting, setIsRedirecting] = useState(false);

  useEffect(() => {
    const handleAuthorize = async () => {
      const oauthParams = getStoredOAuthParams();

      if (!oauthParams) {
        setError("Missing OAuth parameters. Please start the authorization flow again.");
        return;
      }

      const sessionId = searchParams.get("session_id");
      
      if (!sessionId) {
        // No session, redirect to login
        const loginUrl = `/login?client_id=${oauthParams.clientId}&redirect_uri=${encodeURIComponent(oauthParams.redirectUri)}&code_challenge=${oauthParams.codeChallenge}&code_challenge_method=${oauthParams.codeChallengeMethod}${oauthParams.state ? `&state=${oauthParams.state}` : ""}${oauthParams.scope ? `&scope=${oauthParams.scope}` : ""}`;
        window.location.href = loginUrl;
        return;
      }

      try {
        // Call authorize endpoint with session_id
        const response = await oauthApi.authorize({
          client_id: oauthParams.clientId,
          redirect_uri: oauthParams.redirectUri,
          code_challenge: oauthParams.codeChallenge,
          code_challenge_method: oauthParams.codeChallengeMethod,
          state: oauthParams.state,
          scope: oauthParams.scope,
          session_id: sessionId,
        });

        if (response.requiresAuthentication) {
          // Still need to authenticate
          const loginUrl = `/login?client_id=${oauthParams.clientId}&redirect_uri=${encodeURIComponent(oauthParams.redirectUri)}&code_challenge=${oauthParams.codeChallenge}&code_challenge_method=${oauthParams.codeChallengeMethod}${oauthParams.state ? `&state=${oauthParams.state}` : ""}${oauthParams.scope ? `&scope=${oauthParams.scope}` : ""}`;
          window.location.href = loginUrl;
          return;
        }

        if (response.authorizationCode) {
          // Success! Redirect to client with authorization code
          setIsRedirecting(true);
          clearOAuthParams();
          
          const callbackUrl = buildCallbackUrl(
            oauthParams.redirectUri,
            response.authorizationCode,
            response.state || oauthParams.state
          );
          
          window.location.href = callbackUrl;
        } else {
          setError("No authorization code received.");
        }
      } catch (err) {
        setError(getErrorMessage(err));
      }
    };

    handleAuthorize();
  }, [searchParams]);

  if (isRedirecting) {
    return (
      <AuthLayout title="Success!" description="Redirecting you back to the application...">
        <div className="flex flex-col items-center justify-center py-8 space-y-4">
          <CheckCircle className="h-12 w-12 text-green-500" />
          <p className="text-sm text-muted-foreground">
            You will be redirected automatically...
          </p>
        </div>
      </AuthLayout>
    );
  }

  if (error) {
    return (
      <AuthLayout title="Authorization Error" description="Something went wrong">
        <div className="flex flex-col items-center justify-center py-8 space-y-4">
          <XCircle className="h-12 w-12 text-destructive" />
          <p className="text-sm text-destructive text-center">{error}</p>
        </div>
      </AuthLayout>
    );
  }

  return (
    <AuthLayout title="Authorizing..." description="Please wait">
      <div className="flex justify-center py-8">
        <Loader2 className="h-8 w-8 animate-spin text-primary" />
      </div>
    </AuthLayout>
  );
}
