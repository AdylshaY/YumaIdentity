import { Link, Outlet, useLocation } from 'react-router-dom';
import {
  LayoutDashboard,
  Users,
  AppWindow,
  Shield,
  LogOut,
  Menu,
  X,
} from 'lucide-react';
import { useState } from 'react';
import { useAuth } from '../contexts/AuthContext';
import { Button } from '@repo/ui';

const navigation = [
  { name: 'Dashboard', href: '/dashboard', icon: LayoutDashboard },
  { name: 'Users', href: '/dashboard/users', icon: Users },
  { name: 'Applications', href: '/dashboard/applications', icon: AppWindow },
  { name: 'Roles', href: '/dashboard/roles', icon: Shield },
];

export function DashboardLayout() {
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const location = useLocation();
  const { user, logout } = useAuth();

  return (
    <div className='min-h-screen bg-background'>
      {/* Mobile sidebar backdrop */}
      {sidebarOpen && (
        <div
          className='fixed inset-0 z-40 bg-black/50 lg:hidden'
          onClick={() => setSidebarOpen(false)}
        />
      )}

      {/* Sidebar */}
      <aside
        className={`fixed inset-y-0 left-0 z-50 w-64 transform bg-card border-r transition-transform duration-200 ease-in-out lg:translate-x-0 ${
          sidebarOpen ? 'translate-x-0' : '-translate-x-full'
        }`}
      >
        <div className='flex h-16 items-center justify-between px-4 border-b'>
          <h1 className='text-xl font-bold text-foreground'>Admin Dashboard</h1>
          <Button
            variant='ghost'
            size='icon'
            className='lg:hidden'
            onClick={() => setSidebarOpen(false)}
          >
            <X className='h-5 w-5' />
          </Button>
        </div>

        <nav className='flex flex-col gap-1 p-4'>
          {navigation.map((item) => {
            const isActive = location.pathname === item.href;
            return (
              <Link
                key={item.name}
                to={item.href}
                className={`flex items-center gap-3 rounded-lg px-3 py-2 text-sm font-medium transition-colors ${
                  isActive
                    ? 'bg-primary text-primary-foreground'
                    : 'text-muted-foreground hover:bg-accent hover:text-accent-foreground'
                }`}
                onClick={() => setSidebarOpen(false)}
              >
                <item.icon className='h-5 w-5' />
                {item.name}
              </Link>
            );
          })}
        </nav>

        {/* User section */}
        <div className='absolute bottom-0 left-0 right-0 border-t p-4'>
          <div className='flex items-center justify-between'>
            <div className='flex flex-col'>
              <span className='text-sm font-medium text-foreground'>
                {user?.name}
              </span>
              <span className='text-xs text-muted-foreground'>
                {user?.email}
              </span>
            </div>
            <Button variant='ghost' size='icon' onClick={logout}>
              <LogOut className='h-5 w-5' />
            </Button>
          </div>
        </div>
      </aside>

      {/* Main content */}
      <div className='lg:pl-64'>
        {/* Header */}
        <header className='sticky top-0 z-30 flex h-16 items-center gap-4 border-b bg-background px-4'>
          <Button
            variant='ghost'
            size='icon'
            className='lg:hidden'
            onClick={() => setSidebarOpen(true)}
          >
            <Menu className='h-5 w-5' />
          </Button>
          <div className='flex-1' />
          <div className='flex items-center gap-2'>
            <span className='text-sm text-muted-foreground'>
              {user?.roles?.join(', ')}
            </span>
          </div>
        </header>

        {/* Page content */}
        <main className='p-6'>
          <Outlet />
        </main>
      </div>
    </div>
  );
}
