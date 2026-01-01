import { useAuthorize } from '../../hooks';
import { Loader2, AlertCircle } from 'lucide-react';
import {
  Alert,
  AlertDescription,
  AlertTitle,
} from '@workspace/ui/components/alert';

export const AuthorizePage = () => {
  const { isLoading, error } = useAuthorize();

  if (error) {
    return (
      <div className='flex min-h-screen items-center justify-center bg-gray-50 p-4'>
        <Alert variant='destructive' className='max-w-md'>
          <AlertCircle className='h-4 w-4' />
          <AlertTitle>Authorization Error</AlertTitle>
          <AlertDescription>{error}</AlertDescription>
        </Alert>
      </div>
    );
  }

  if (isLoading) {
    return (
      <div className='flex min-h-screen flex-col items-center justify-center bg-gray-50'>
        <div className='flex flex-col items-center gap-4'>
          <Loader2 className='h-8 w-8 animate-spin text-primary' />
          <p className='text-sm text-gray-500'>Authorizing...</p>
        </div>
      </div>
    );
  }

  return (
    <div className='flex min-h-screen flex-col items-center justify-center bg-gray-50'>
      <div className='flex flex-col items-center gap-4'>
        <Loader2 className='h-8 w-8 animate-spin text-primary' />
        <p className='text-sm text-gray-500'>Authorizing...</p>
      </div>
    </div>
  );
};
