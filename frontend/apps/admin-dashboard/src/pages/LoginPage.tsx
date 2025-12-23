import { useNavigate } from "react-router-dom";
import { Button } from "@repo/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@repo/ui/card";
import { Shield } from "lucide-react";
import { initiateOAuthFlow } from "../lib/oauth";

export function LoginPage() {
  const navigate = useNavigate();

  const handleLogin = async () => {
    try {
      await initiateOAuthFlow();
    } catch (error) {
      console.error("Failed to initiate OAuth flow:", error);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-background p-4">
      <Card className="w-full max-w-md">
        <CardHeader className="text-center">
          <div className="mx-auto mb-4 flex h-12 w-12 items-center justify-center rounded-full bg-primary">
            <Shield className="h-6 w-6 text-primary-foreground" />
          </div>
          <CardTitle className="text-2xl">Super Admin Dashboard</CardTitle>
          <CardDescription>
            Sign in to access the administration panel
          </CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          <Button className="w-full" size="lg" onClick={handleLogin}>
            Sign in with YumaIdentity
          </Button>
          <p className="text-center text-sm text-muted-foreground">
            You will be redirected to the OAuth authorization server
          </p>
        </CardContent>
      </Card>
    </div>
  );
}
