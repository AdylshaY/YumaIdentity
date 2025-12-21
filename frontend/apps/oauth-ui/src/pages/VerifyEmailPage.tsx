import { useState, useEffect } from "react";
import { Link, useSearchParams } from "react-router-dom";
import { Loader2, CheckCircle, XCircle } from "lucide-react";

import { Button } from "@repo/ui/button.tsx";

import { AuthLayout } from "../components/layouts/AuthLayout";
import { oauthApi, getErrorMessage } from "../lib/api";

export function VerifyEmailPage() {
  const [searchParams] = useSearchParams();
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);

  const token = searchParams.get("token");
  const email = searchParams.get("email");

  useEffect(() => {
    const verifyEmail = async () => {
      if (!token || !email) {
        setError("Invalid verification link. Please request a new one.");
        setIsLoading(false);
        return;
      }

      try {
        await oauthApi.verifyEmail({ token, email });
        setSuccess(true);
      } catch (err) {
        setError(getErrorMessage(err));
      } finally {
        setIsLoading(false);
      }
    };

    verifyEmail();
  }, [token, email]);

  if (isLoading) {
    return (
      <AuthLayout title="Verifying email" description="Please wait...">
        <div className="flex justify-center py-8">
          <Loader2 className="h-8 w-8 animate-spin text-primary" />
        </div>
      </AuthLayout>
    );
  }

  if (success) {
    return (
      <AuthLayout
        title="Email verified!"
        description="Your email has been successfully verified"
      >
        <div className="space-y-4 text-center">
          <div className="flex justify-center">
            <CheckCircle className="h-12 w-12 text-green-500" />
          </div>
          <p className="text-sm text-muted-foreground">
            You can now sign in to your account.
          </p>
          <Button asChild className="w-full">
            <Link to="/login">Sign in</Link>
          </Button>
        </div>
      </AuthLayout>
    );
  }

  return (
    <AuthLayout
      title="Verification failed"
      description="We couldn't verify your email"
    >
      <div className="space-y-4 text-center">
        <div className="flex justify-center">
          <XCircle className="h-12 w-12 text-destructive" />
        </div>
        <p className="text-sm text-muted-foreground">
          {error || "This verification link is invalid or has expired."}
        </p>
        <Button asChild className="w-full">
          <Link to="/login">Back to Sign in</Link>
        </Button>
      </div>
    </AuthLayout>
  );
}
