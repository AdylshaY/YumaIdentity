import { Link } from "react-router-dom";
import { XCircle } from "lucide-react";
import { Button } from "@repo/ui/button.tsx";
import { AuthLayout } from "../components/layouts/AuthLayout";

export function NotFoundPage() {
  return (
    <AuthLayout title="Page not found" description="The page you're looking for doesn't exist">
      <div className="flex flex-col items-center justify-center py-8 space-y-4">
        <XCircle className="h-12 w-12 text-muted-foreground" />
        <p className="text-sm text-muted-foreground text-center">
          Please check the URL or go back to the sign in page.
        </p>
        <Button asChild variant="outline">
          <Link to="/login">Go to Sign in</Link>
        </Button>
      </div>
    </AuthLayout>
  );
}
