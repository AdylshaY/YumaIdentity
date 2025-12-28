import { Routes, Route } from "react-router-dom";
import { Button } from "@workspace/ui/components/button";

function HomePage() {
    return (
        <div className="min-h-screen flex items-center justify-center bg-background">
            <div className="text-center space-y-6">
                <h1 className="text-4xl font-bold text-foreground">
                    YumaIdentity Auth
                </h1>
                <p className="text-muted-foreground">
                    PKCE Authentication Flow - Login, Register & More
                </p>
                <div className="flex gap-4 justify-center">
                    <Button>Login</Button>
                    <Button variant="outline">Register</Button>
                </div>
            </div>
        </div>
    );
}

export default function App() {
    return (
        <Routes>
            <Route path="/" element={<HomePage />} />
        </Routes>
    );
}
