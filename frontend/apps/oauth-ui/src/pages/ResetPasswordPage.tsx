import { useState, useEffect } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Link, useSearchParams } from "react-router-dom";
import { Loader2, CheckCircle, XCircle } from "lucide-react";

import { Button } from "@repo/ui/button.tsx";
import { Input } from "@repo/ui/input.tsx";
import { Label } from "@repo/ui/label.tsx";
import { Alert, AlertDescription } from "@repo/ui/alert.tsx";

import { AuthLayout } from "../components/layouts/AuthLayout";
import { resetPasswordSchema, type ResetPasswordFormData } from "../lib/validations";
import { oauthApi, getErrorMessage } from "../lib/api";

export function ResetPasswordPage() {
  const [searchParams] = useSearchParams();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);

  const token = searchParams.get("token");
  const email = searchParams.get("email");

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<ResetPasswordFormData>({
    resolver: zodResolver(resetPasswordSchema),
    defaultValues: {
      password: "",
      confirmPassword: "",
    },
  });

  // Validate token and email are present
  useEffect(() => {
    if (!token || !email) {
      setError("Invalid or missing reset link. Please request a new one.");
    }
  }, [token, email]);

  const onSubmit = async (data: ResetPasswordFormData) => {
    if (!token || !email) {
      setError("Invalid or missing reset link. Please request a new one.");
      return;
    }

    setIsLoading(true);
    setError(null);

    try {
      await oauthApi.resetPassword({
        token,
        email,
        newPassword: data.password,
      });
      setSuccess(true);
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setIsLoading(false);
    }
  };

  if (success) {
    return (
      <AuthLayout
        title="Password reset successful"
        description="Your password has been updated"
      >
        <div className="space-y-4 text-center">
          <div className="flex justify-center">
            <CheckCircle className="h-12 w-12 text-green-500" />
          </div>
          <p className="text-sm text-muted-foreground">
            You can now sign in with your new password.
          </p>
          <Button asChild className="w-full">
            <Link to="/login">Sign in</Link>
          </Button>
        </div>
      </AuthLayout>
    );
  }

  if (!token || !email) {
    return (
      <AuthLayout title="Invalid link" description="This reset link is not valid">
        <div className="space-y-4 text-center">
          <div className="flex justify-center">
            <XCircle className="h-12 w-12 text-destructive" />
          </div>
          <p className="text-sm text-muted-foreground">
            This password reset link is invalid or has expired. Please request a
            new one.
          </p>
          <Button asChild className="w-full">
            <Link to="/forgot-password">Request new link</Link>
          </Button>
        </div>
      </AuthLayout>
    );
  }

  return (
    <AuthLayout
      title="Reset your password"
      description="Enter a new password for your account"
    >
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        {error && (
          <Alert variant="destructive">
            <AlertDescription>{error}</AlertDescription>
          </Alert>
        )}

        <div className="space-y-2">
          <Label htmlFor="password">New password</Label>
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
          <Label htmlFor="confirmPassword">Confirm new password</Label>
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
          Reset password
        </Button>
      </form>
    </AuthLayout>
  );
}
