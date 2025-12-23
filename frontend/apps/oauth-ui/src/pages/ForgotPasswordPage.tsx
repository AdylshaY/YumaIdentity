import { useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Link } from "react-router-dom";
import { Loader2, ArrowLeft } from "lucide-react";

import { Button } from "@repo/ui/button.tsx";
import { Input } from "@repo/ui/input.tsx";
import { Label } from "@repo/ui/label.tsx";
import { Alert, AlertDescription } from "@repo/ui/alert.tsx";

import { AuthLayout } from "../components/layouts/AuthLayout";
import { forgotPasswordSchema, type ForgotPasswordFormData } from "../lib/validations";
import { oauthApi, getErrorMessage } from "../lib/api";

export function ForgotPasswordPage() {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<ForgotPasswordFormData>({
    resolver: zodResolver(forgotPasswordSchema),
    defaultValues: {
      email: "",
    },
  });

  const onSubmit = async (data: ForgotPasswordFormData) => {
    setIsLoading(true);
    setError(null);

    try {
      await oauthApi.forgotPassword({
        email: data.email,
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
        title="Check your email"
        description="We've sent you a password reset link"
      >
        <div className="space-y-4 text-center">
          <p className="text-sm text-muted-foreground">
            If an account exists with that email, you will receive a password
            reset link shortly. Please check your inbox and spam folder.
          </p>
          <Button asChild variant="outline" className="w-full">
            <Link to="/login">
              <ArrowLeft className="mr-2 h-4 w-4" />
              Back to Sign in
            </Link>
          </Button>
        </div>
      </AuthLayout>
    );
  }

  return (
    <AuthLayout
      title="Forgot password?"
      description="Enter your email to receive a reset link"
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

        <Button type="submit" className="w-full" disabled={isLoading}>
          {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
          Send reset link
        </Button>

        <Button asChild variant="ghost" className="w-full">
          <Link to="/login">
            <ArrowLeft className="mr-2 h-4 w-4" />
            Back to Sign in
          </Link>
        </Button>
      </form>
    </AuthLayout>
  );
}
