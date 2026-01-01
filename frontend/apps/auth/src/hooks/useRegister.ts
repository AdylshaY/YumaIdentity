import { useMutation } from '@tanstack/react-query';
import { AuthService } from '../services/auth.service';
import { RegisterRequest } from '../types/auth';
import { useNavigate } from 'react-router-dom';
import { useOAuthParams } from './useOAuthParams';

export const useRegister = () => {
  const navigate = useNavigate();
  const { searchParams } = useOAuthParams();

  const mutation = useMutation({
    mutationFn: (data: RegisterRequest) => AuthService.register(data),
    onSuccess: () => {
      // Registration successful, redirect to login
      // Preserve query parameters for OAuth flow
      navigate(`/login?${searchParams.toString()}`);
    },
  });

  return {
    register: mutation.mutateAsync,
    isLoading: mutation.isPending,
    error: mutation.error
      ? mutation.error instanceof Error
        ? mutation.error.message
        : 'An error occurred'
      : null,
  };
};
