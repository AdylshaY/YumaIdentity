import axios from "axios";

const API_URL = import.meta.env.VITE_API_URL;

export const apiClient = axios.create({
  baseURL: API_URL,
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true,
});

// Add auth token to requests
apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem("access_token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Handle token refresh on 401
apiClient.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        const refreshToken = localStorage.getItem("refresh_token");
        if (refreshToken) {
          const response = await axios.post(`${API_URL}/oauth/token`, {
            grant_type: "refresh_token",
            refresh_token: refreshToken,
            client_id: import.meta.env.VITE_OAUTH_CLIENT_ID,
          });

          const { access_token, refresh_token } = response.data;
          localStorage.setItem("access_token", access_token);
          localStorage.setItem("refresh_token", refresh_token);

          originalRequest.headers.Authorization = `Bearer ${access_token}`;
          return apiClient(originalRequest);
        }
      } catch {
        // Refresh failed, redirect to login
        localStorage.removeItem("access_token");
        localStorage.removeItem("refresh_token");
        window.location.href = "/";
      }
    }

    return Promise.reject(error);
  }
);

export interface TokenResponse {
  access_token: string;
  refresh_token: string;
  token_type: string;
  expires_in: number;
}

export interface User {
  id: string;
  email: string;
  userName: string;
  firstName: string;
  lastName: string;
  emailConfirmed: boolean;
  roles: string[];
  createdAt: string;
  updatedAt: string;
}

export interface Application {
  id: string;
  clientId: string;
  clientName: string;
  clientType: string;
  redirectUris: string[];
  allowedScopes: string[];
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface Role {
  id: string;
  name: string;
  description: string;
  createdAt: string;
}

// Token exchange
export async function exchangeCodeForToken(
  code: string,
  codeVerifier: string
): Promise<TokenResponse> {
  const response = await axios.post<TokenResponse>(`${API_URL}/api/oauth/token`, {
    grant_type: "authorization_code",
    code,
    code_verifier: codeVerifier,
    client_id: import.meta.env.VITE_OAUTH_CLIENT_ID,
    redirect_uri: import.meta.env.VITE_OAUTH_REDIRECT_URI,
  });
  return response.data;
}

// Admin API endpoints
export const adminApi = {
  // Users
  getUsers: () => apiClient.get<User[]>("/admin/users"),
  getUser: (id: string) => apiClient.get<User>(`/admin/users/${id}`),
  createUser: (data: Partial<User> & { password: string }) =>
    apiClient.post<User>("/admin/users", data),
  updateUser: (id: string, data: Partial<User>) =>
    apiClient.put<User>(`/admin/users/${id}`, data),
  deleteUser: (id: string) => apiClient.delete(`/admin/users/${id}`),

  // Applications
  getApplications: () => apiClient.get<Application[]>("/admin/applications"),
  getApplication: (id: string) =>
    apiClient.get<Application>(`/admin/applications/${id}`),
  createApplication: (data: Partial<Application>) =>
    apiClient.post<Application>("/admin/applications", data),
  updateApplication: (id: string, data: Partial<Application>) =>
    apiClient.put<Application>(`/admin/applications/${id}`, data),
  deleteApplication: (id: string) =>
    apiClient.delete(`/admin/applications/${id}`),

  // Roles
  getRoles: () => apiClient.get<Role[]>("/admin/roles"),
  getRole: (id: string) => apiClient.get<Role>(`/admin/roles/${id}`),
  createRole: (data: Partial<Role>) =>
    apiClient.post<Role>("/admin/roles", data),
  updateRole: (id: string, data: Partial<Role>) =>
    apiClient.put<Role>(`/admin/roles/${id}`, data),
  deleteRole: (id: string) => apiClient.delete(`/admin/roles/${id}`),

  // User Roles
  assignRole: (userId: string, roleId: string) =>
    apiClient.post(`/admin/users/${userId}/roles/${roleId}`),
  removeRole: (userId: string, roleId: string) =>
    apiClient.delete(`/admin/users/${userId}/roles/${roleId}`),

  // Dashboard stats
  getStats: () =>
    apiClient.get<{
      totalUsers: number;
      totalApplications: number;
      totalRoles: number;
      activeUsers: number;
    }>("/admin/stats"),
};
