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
  client_id: string;
  redirect_uri: string;
  code_challenge: string;
  code_challenge_method: string;
  state?: string;
  scope?: string;
  session_id?: string;
}

export interface AuthorizeResponse {
  requiresAuthentication: boolean;
  requiresConsent: boolean;
  authorizationCode?: string;
  state?: string;
  expiresIn?: number;
}

export interface LoginRequest {
  email: string;
  password: string;
  clientId: string;
}

export interface LoginResponse {
  sessionId: string;
  email: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  clientId?: string;
}
