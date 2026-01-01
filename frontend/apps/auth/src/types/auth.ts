export interface AuthorizeRequest {
  client_id: string;
  redirect_uri: string;
  response_type: string;
  scope?: string;
  state?: string;
  code_challenge?: string;
  code_challenge_method?: string;
  session_id?: string;
}

export interface AuthorizeApiRequest {
  ClientId: string;
  RedirectUri: string;
  CodeChallenge: string;
  CodeChallengeMethod: string;
  State?: string;
  Scope?: string;
  SessionId?: string;
}

export interface AuthorizeResponse {
  requiresAuthentication: boolean;
  requiresConsent: boolean;
  authorizationCode?: string;
  state?: string;
  expiresIn?: number;
}

export interface LoginRequest {
  Email: string;
  Password: string;
  ClientId: string;
}

export interface LoginResponse {
  sessionId: string;
}

export interface RegisterRequest {
  Email: string;
  Password: string;
  ClientId?: string;
}
