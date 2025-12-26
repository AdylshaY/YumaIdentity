import { useEffect, useState, useRef } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { Card, CardContent, CardHeader, CardTitle } from "@repo/ui/card.tsx";
import { Alert, AlertDescription } from "@repo/ui/alert.tsx";
import { getPkceParams, clearPkceParams, validateState } from "../lib/pkce";
import { exchangeCodeForToken } from "../lib/api";
import { useAuth } from "../contexts/AuthContext";
import { Loader2 } from "lucide-react";

export function CallbackPage() {
  const [error, setError] = useState<string | null>(null);
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const { login } = useAuth();
  const processedRef = useRef(false);

  useEffect(() => {
    // Prevent double processing in React StrictMode
    if (processedRef.current) return;
    
    const handleCallback = async () => {
      const code = searchParams.get("code");
      const state = searchParams.get("state");
      const errorParam = searchParams.get("error");
      const errorDescription = searchParams.get("error_description");

      // Handle OAuth errors
      if (errorParam) {
        setError(errorDescription || errorParam);
        return;
      }

      // Validate required parameters
      if (!code || !state) {
        setError("Missing authorization code or state parameter");
        return;
      }

      // Mark as processed
      processedRef.current = true;

      // Validate state to prevent CSRF
      if (!validateState(state)) {
        setError("Invalid state parameter - possible CSRF attack");
        clearPkceParams();
        return;
      }

      // Get stored PKCE params
      const pkceParams = getPkceParams();
      if (!pkceParams.codeVerifier) {
        setError("Missing PKCE code verifier");
        return;
      }

      try {
        // Exchange code for tokens
        const tokens = await exchangeCodeForToken(code, pkceParams.codeVerifier);
        
        // Clear PKCE params
        clearPkceParams();

        // Store tokens and update auth state
        login(tokens.access_token, tokens.refresh_token);

        // Use a small delay to ensure state update is processed
        setTimeout(() => {
          navigate("/dashboard", { replace: true });
        }, 100);
      } catch (err: unknown) {
        console.error("Token exchange failed:", err);
        // Extract error message from axios error
        let errorMessage = "Failed to exchange authorization code for tokens";
        if (err && typeof err === 'object' && 'response' in err) {
          const axiosError = err as { response?: { data?: { message?: string; Message?: string; title?: string; Title?: string } } };
          const data = axiosError.response?.data;
          errorMessage = data?.message || data?.Message || data?.title || data?.Title || errorMessage;
        }
        setError(errorMessage);
        clearPkceParams();
      }
    };

    handleCallback();
  }, [searchParams, navigate, login]);

  if (error) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-background p-4">
        <Card className="w-full max-w-md">
          <CardHeader>
            <CardTitle className="text-destructive">
              Authentication Error
            </CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            <Alert variant="destructive">
              <AlertDescription>{error}</AlertDescription>
            </Alert>
            <a
              href="/"
              className="block text-center text-sm text-primary hover:underline"
            >
              Return to login
            </a>
          </CardContent>
        </Card>
      </div>
    );
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-background p-4">
      <Card className="w-full max-w-md">
        <CardHeader>
          <CardTitle className="text-center">Authenticating...</CardTitle>
        </CardHeader>
        <CardContent className="flex justify-center py-8">
          <Loader2 className="h-8 w-8 animate-spin text-primary" />
        </CardContent>
      </Card>
    </div>
  );
}
