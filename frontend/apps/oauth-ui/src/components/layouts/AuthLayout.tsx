import type { ReactNode } from "react";
import { Link } from "react-router-dom";

interface AuthLayoutProps {
  children: ReactNode;
  title: string;
  description?: string;
}

export function AuthLayout({ children, title, description }: AuthLayoutProps) {
  return (
    <div className="min-h-screen bg-background flex flex-col items-center justify-center p-4">
      <div className="w-full max-w-md space-y-8">
        {/* Logo */}
        <div className="text-center">
          <Link to="/" className="inline-block">
            <h1 className="text-3xl font-bold tracking-tight">YumaIdentity</h1>
          </Link>
        </div>

        {/* Card */}
        <div className="bg-card rounded-lg border shadow-sm p-8">
          <div className="space-y-2 text-center mb-6">
            <h2 className="text-2xl font-semibold tracking-tight">{title}</h2>
            {description && (
              <p className="text-sm text-muted-foreground">{description}</p>
            )}
          </div>
          {children}
        </div>

        {/* Footer */}
        <p className="text-center text-xs text-muted-foreground">
          Powered by YumaIdentity OAuth2
        </p>
      </div>
    </div>
  );
}
