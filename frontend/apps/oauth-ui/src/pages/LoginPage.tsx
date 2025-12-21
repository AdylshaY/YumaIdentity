import { useState, useEffect } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Link, useSearchParams } from "react-router-dom";
import { Loader2 } from "lucide-react";

import { Button } from "@repo/ui/button.tsx";
import { Input } from "@repo/ui/input.tsx";
import { Label } from "@repo/ui/label.tsx";
import { Alert, AlertDescription } from "@repo/ui/alert.tsx";

import { AuthLayout } from "../components/layouts/AuthLayout";
import { loginSchema, type LoginFormData } from "../lib/validations";
import { oauthApi, getErrorMessage } from "../lib/api";
import {
  parseOAuthParams,
  storeOAuthParams,
  getStoredOAuthParams,
  buildAuthorizeUrl,
  type OAuthParams,
} from "../lib/oauth";

const API_BASE_URL = import.meta.env.VITE_API_URL || "http://localhost:5294";

export function LoginPage() {
  const [searchParams] = useSearchParams();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [oauthParams, setOauthParams] = useState<OAuthParams | null>(null);

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginFormData>({
    resolver: zodResolver(loginSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  });

  // Parse and store OAuth params on mount
  useEffect(() => {
    const params = parseOAuthParams(searchParams);
    if (params) {
      storeOAuthParams(params);
      setOauthParams(params);
    } else {
      // Check if we have stored params (e.g., after registration)
      const stored = getStoredOAuthParams();
      if (stored) {
        setOauthParams(stored);
      }
    }
  }, [searchParams]);

  const onSubmit = async (data: LoginFormData) => {
    if (!oauthParams) {
      setError("Missing OAuth parameters. Please start the authorization flow again.");
      return;
    }

    setIsLoading(true);
    setError(null);

    try {
      // Call login endpoint
      const response = await oauthApi.login({
        email: data.email,
        password: data.password,
        clientId: oauthParams.clientId,
      });

      // After successful login, redirect back to authorize with session_id
      const authorizeUrl = buildAuthorizeUrl(API_BASE_URL, oauthParams, response.sessionId);
      
      // Redirect to authorize endpoint
      window.location.href = authorizeUrl;
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setIsLoading(false);
    }
  };

  // Build register URL with preserved OAuth params
  const registerUrl = oauthParams
    ? `/register?client_id=${oauthParams.clientId}&redirect_uri=${encodeURIComponent(oauthParams.redirectUri)}&code_challenge=${oauthParams.codeChallenge}&code_challenge_method=${oauthParams.codeChallengeMethod}${oauthParams.state ? `&state=${oauthParams.state}` : ""}${oauthParams.scope ? `&scope=${oauthParams.scope}` : ""}`
    : "/register";

  return (
    <AuthLayout
      title="Sign in"
      description="Enter your credentials to access your account"
    >
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        {error && (
          <Alert variant="destructive">
            <AlertDescription>{error}</AlertDescription>
          </Alert>
        )}

        <div className="space-y-2">
          <Label htmlFor="email">Email</Label>
          <Input
            id="email"
            type="email"
            placeholder="name@example.com"
            autoComplete="email"
            disabled={isLoading}
            {...register("email")}
          />
          {errors.email && (
            <p className="text-sm text-destructive">{errors.email.message}</p>
          )}
        </div>

        <div className="space-y-2">
          <div className="flex items-center justify-between">
            <Label htmlFor="password">Password</Label>
            <Link
              to="/forgot-password"
              className="text-sm text-primary hover:underline"
            >
              Forgot password?
            </Link>
          </div>
          <Input
            id="password"
            type="password"
            placeholder="••••••••"
            autoComplete="current-password"
            disabled={isLoading}
            {...register("password")}
          />
          {errors.password && (
            <p className="text-sm text-destructive">{errors.password.message}</p>
          )}
        </div>

        <Button type="submit" className="w-full" disabled={isLoading}>
          {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
          Sign in
        </Button>

        <div className="text-center text-sm">
          <span className="text-muted-foreground">Don&apos;t have an account? </span>
          <Link to={registerUrl} className="text-primary hover:underline">
            Sign up
          </Link>
        </div>
      </form>
    </AuthLayout>
  );
}
