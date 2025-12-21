import { Button } from "@repo/ui/button.tsx"

function App() {
  return (
    <div className="min-h-screen bg-background flex items-center justify-center p-4">
      <div className="w-full max-w-md">
        <div className="text-center space-y-2">
          <h1 className="text-4xl font-bold tracking-tight">YumaIdentity</h1>
          <p className="text-muted-foreground">OAuth2 Authorization</p>
        </div>
        <Button variant='secondary'>Test</Button>
      </div>
    </div>
  )
}

export default App
