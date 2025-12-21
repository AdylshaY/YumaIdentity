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
import { registerSchema, type RegisterFormData } from "../lib/validations";
import { oauthApi, getErrorMessage } from "../lib/api";
import {
  parseOAuthParams,
  storeOAuthParams,
  getStoredOAuthParams,
  type OAuthParams,
} from "../lib/oauth";

export function RegisterPage() {
  const [searchParams] = useSearchParams();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);
  const [oauthParams, setOauthParams] = useState<OAuthParams | null>(null);

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<RegisterFormData>({
    resolver: zodResolver(registerSchema),
    defaultValues: {
      firstName: "",
      lastName: "",
      email: "",
      password: "",
      confirmPassword: "",
    },
  });

  // Parse and store OAuth params on mount
  useEffect(() => {
    const params = parseOAuthParams(searchParams);
    if (params) {
      storeOAuthParams(params);
      setOauthParams(params);
    } else {
      const stored = getStoredOAuthParams();
      if (stored) {
        setOauthParams(stored);
      }
    }
  }, [searchParams]);

  const onSubmit = async (data: RegisterFormData) => {
    setIsLoading(true);
    setError(null);

    try {
      await oauthApi.register({
        email: data.email,
        password: data.password,
        firstName: data.firstName,
        lastName: data.lastName,
      });

      setSuccess(true);
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setIsLoading(false);
    }
  };

  // Build login URL with preserved OAuth params
  const loginUrl = oauthParams
    ? `/login?client_id=${oauthParams.clientId}&redirect_uri=${encodeURIComponent(oauthParams.redirectUri)}&code_challenge=${oauthParams.codeChallenge}&code_challenge_method=${oauthParams.codeChallengeMethod}${oauthParams.state ? `&state=${oauthParams.state}` : ""}${oauthParams.scope ? `&scope=${oauthParams.scope}` : ""}`
    : "/login";

  if (success) {
    return (
      <AuthLayout
        title="Check your email"
        description="We've sent you a verification link"
      >
        <div className="space-y-4 text-center">
          <p className="text-sm text-muted-foreground">
            Please check your email and click the verification link to activate
            your account. Once verified, you can sign in.
          </p>
          <Button asChild className="w-full">
            <Link to={loginUrl}>Back to Sign in</Link>
          </Button>
        </div>
      </AuthLayout>
    );
  }

  return (
    <AuthLayout
      title="Create an account"
      description="Enter your details to get started"
    >
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        {error && (
          <Alert variant="destructive">
            <AlertDescription>{error}</AlertDescription>
          </Alert>
        )}

        <div className="grid grid-cols-2 gap-4">
          <div className="space-y-2">
            <Label htmlFor="firstName">First name</Label>
            <Input
              id="firstName"
              placeholder="John"
              autoComplete="given-name"
              disabled={isLoading}
              {...register("firstName")}
            />
            {errors.firstName && (
              <p className="text-sm text-destructive">{errors.firstName.message}</p>
            )}
          </div>

          <div className="space-y-2">
            <Label htmlFor="lastName">Last name</Label>
            <Input
              id="lastName"
              placeholder="Doe"
              autoComplete="family-name"
              disabled={isLoading}
              {...register("lastName")}
            />
            {errors.lastName && (
              <p className="text-sm text-destructive">{errors.lastName.message}</p>
            )}
          </div>
        </div>

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
          <Label htmlFor="password">Password</Label>
          <Input
            id="password"
            type="password"
            placeholder="••••••••"
            autoComplete="new-password"
            disabled={isLoading}
            {...register("password")}
          />
          {errors.password && (
            <p className="text-sm text-destructive">{errors.password.message}</p>
          )}
        </div>

        <div className="space-y-2">
          <Label htmlFor="confirmPassword">Confirm password</Label>
          <Input
            id="confirmPassword"
            type="password"
            placeholder="••••••••"
            autoComplete="new-password"
            disabled={isLoading}
            {...register("confirmPassword")}
          />
          {errors.confirmPassword && (
            <p className="text-sm text-destructive">
              {errors.confirmPassword.message}
            </p>
          )}
        </div>

        <Button type="submit" className="w-full" disabled={isLoading}>
          {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
          Create account
        </Button>

        <div className="text-center text-sm">
          <span className="text-muted-foreground">Already have an account? </span>
          <Link to={loginUrl} className="text-primary hover:underline">
            Sign in
          </Link>
        </div>
      </form>
    </AuthLayout>
  );
}
