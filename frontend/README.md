# YumaIdentity Frontend

OAuth2 Authorization UI for YumaIdentity - a modern identity and access management system.

## Overview

This is a **Turborepo monorepo** containing the OAuth2 authentication UI and shared packages. The frontend handles the authorization flow for client applications using the **OAuth2 Authorization Code Flow with PKCE**.

## Architecture

```
frontend/
├── apps/
│   └── oauth-ui/              # OAuth2 Authorization UI (Vite + React 19)
│
├── packages/
│   ├── ui/                    # Shared UI components (shadcn/ui)
│   ├── tailwind-config/       # Shared Tailwind CSS configuration
│   ├── typescript-config/     # Shared TypeScript configurations
│   └── eslint-config/         # Shared ESLint configurations
│
├── package.json               # Root package with Turborepo scripts
├── turbo.json                 # Turborepo pipeline configuration
└── pnpm-workspace.yaml        # pnpm workspace definition
```

## Tech Stack

- **Build Tool**: Vite 7.3
- **Framework**: React 19
- **Language**: TypeScript 5.9
- **Styling**: Tailwind CSS 3.4 + shadcn/ui
- **Forms**: React Hook Form + Zod validation
- **Routing**: React Router 7
- **HTTP Client**: Axios
- **Monorepo**: Turborepo + pnpm

## Prerequisites

- **Node.js** >= 18
- **pnpm** >= 9.0 (`npm install -g pnpm`)

## Getting Started

### 1. Install dependencies

```bash
cd frontend
pnpm install
```

### 2. Configure environment

```bash
# Copy example env file
cp apps/oauth-ui/.env.example apps/oauth-ui/.env

# Edit .env file
# VITE_API_URL=http://localhost:5294
```

### 3. Run development server

```bash
# Run all apps in dev mode
pnpm dev

# Or run specific app
cd apps/oauth-ui
pnpm dev
```

The OAuth UI will be available at **http://localhost:5173**

### 4. Build for production

```bash
pnpm build
```

## OAuth2 Flow

The OAuth UI implements the **Authorization Code Flow with PKCE**:

```
┌──────────────┐     ┌──────────────┐     ┌──────────────┐
│ Client App   │     │  OAuth UI    │     │   Backend    │
└──────┬───────┘     └──────┬───────┘     └──────┬───────┘
       │                    │                    │
       │ 1. Redirect with   │                    │
       │ PKCE params        │                    │
       │───────────────────>│                    │
       │                    │                    │
       │                    │ 2. User login      │
       │                    │───────────────────>│
       │                    │                    │
       │                    │ 3. Session created │
       │                    │<───────────────────│
       │                    │                    │
       │                    │ 4. Get auth code   │
       │                    │───────────────────>│
       │                    │                    │
       │ 5. Redirect with   │                    │
       │ authorization code │                    │
       │<───────────────────│                    │
       │                    │                    │
       │ 6. Exchange code   │                    │
       │ for tokens         │                    │
       │───────────────────────────────────────->│
       │                    │                    │
       │ 7. Access + Refresh│                    │
       │    tokens          │                    │
       │<───────────────────────────────────────-│
```

## Pages

| Route | Description |
|-------|-------------|
| `/login` | User sign in |
| `/register` | New user registration |
| `/forgot-password` | Request password reset |
| `/reset-password` | Set new password (with token) |
| `/verify-email` | Email verification (with token) |
| `/authorize` | OAuth callback handler |

## Adding UI Components

shadcn/ui components are installed in the shared `@repo/ui` package:

```bash
cd packages/ui
pnpm dlx shadcn@latest add [component-name]

# Example
pnpm dlx shadcn@latest add dialog dropdown-menu
```

Components are automatically available to all apps via:

```tsx
import { Button } from "@repo/ui/button.tsx";
import { Input } from "@repo/ui/input.tsx";
```

## Scripts

| Command | Description |
|---------|-------------|
| `pnpm dev` | Start all apps in development mode |
| `pnpm build` | Build all apps for production |
| `pnpm lint` | Run ESLint on all packages |
| `pnpm format` | Format code with Prettier |
| `pnpm check-types` | Run TypeScript type checking |

## Project Structure Details

### apps/oauth-ui

Main OAuth2 authorization UI application.

```
oauth-ui/
├── src/
│   ├── components/       # React components
│   │   └── layouts/      # Page layouts
│   ├── lib/
│   │   ├── api/          # API client and types
│   │   ├── pkce.ts       # PKCE utilities
│   │   ├── oauth.ts      # OAuth flow helpers
│   │   └── validations.ts # Zod schemas
│   ├── pages/            # Route pages
│   ├── App.tsx           # Router configuration
│   └── main.tsx          # Entry point
├── .env                  # Environment variables
└── package.json
```

### packages/ui

Shared React component library using shadcn/ui.

```
ui/
├── src/
│   ├── button.tsx        # Button component
│   ├── input.tsx         # Input component
│   ├── label.tsx         # Label component
│   ├── card.tsx          # Card component
│   ├── alert.tsx         # Alert component
│   ├── form.tsx          # Form components
│   ├── styles.css        # CSS variables (theming)
│   └── lib/
│       └── utils.ts      # cn() utility
└── components.json       # shadcn configuration
```

### packages/tailwind-config

Shared Tailwind CSS configuration with shadcn/ui theme.

### packages/typescript-config

Shared TypeScript configurations:
- `base.json` - Base configuration
- `vite-react.json` - Vite + React apps
- `react-library.json` - React libraries
- `nextjs.json` - Next.js apps

## Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `VITE_API_URL` | Backend API URL | `http://localhost:5294` |

## Related

- [Backend Documentation](../backend/README.md)
- [shadcn/ui Documentation](https://ui.shadcn.com)
- [Turborepo Documentation](https://turbo.build/repo/docs)
