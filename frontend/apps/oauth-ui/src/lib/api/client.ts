import axios, { AxiosError } from "axios";

// API base URL - will be configured via environment variable
const API_BASE_URL = import.meta.env.VITE_API_URL || "http://localhost:5294";

export const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true, // Important for session cookies
});

// Error response type
export interface ApiError {
  error: string;
  error_description?: string;
  error_uri?: string;
}

// Type guard for API errors
export function isApiError(error: unknown): error is AxiosError<ApiError> {
  return axios.isAxiosError(error);
}

// Extract error message from API error
export function getErrorMessage(error: unknown): string {
  if (isApiError(error)) {
    const data = error.response?.data;
    if (data?.error_description) {
      return data.error_description;
    }
    if (data?.error) {
      return data.error;
    }
    return error.message;
  }
  if (error instanceof Error) {
    return error.message;
  }
  return "An unexpected error occurred";
}
