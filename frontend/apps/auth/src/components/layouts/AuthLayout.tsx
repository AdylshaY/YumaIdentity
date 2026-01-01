import { ShieldCheck } from 'lucide-react';
import { ReactNode } from 'react';

interface AuthLayoutProps {
  children: ReactNode;
}

export const AuthLayout = ({ children }: AuthLayoutProps) => {
  return (
    <div className='flex min-h-screen flex-col items-center justify-center bg-gray-50 p-4'>
      <div className='mb-8 flex items-center gap-2'>
        <div className='flex h-10 w-10 items-center justify-center rounded-lg bg-primary text-primary-foreground'>
          <ShieldCheck className='h-6 w-6' />
        </div>
        <h1 className='text-3xl font-bold tracking-tight text-gray-900'>
          YumaIdentity
        </h1>
      </div>

      {children}

      <div className='mt-8 text-center text-sm text-gray-500'>
        &copy; {new Date().getFullYear()} YumaIdentity. All rights reserved.
      </div>
    </div>
  );
};
