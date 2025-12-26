import { useEffect, useState } from 'react';
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@repo/ui/card.tsx';
import { Users, AppWindow, Shield, Activity } from 'lucide-react';
import { adminApi } from '../lib/api';

interface Stats {
  totalUsers: number;
  totalApplications: number;
  totalRoles: number;
  activeUsers: number;
}

export function DashboardPage() {
  const [stats, setStats] = useState<Stats | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadStats = async () => {
      try {
        const response = await adminApi.getStats();
        setStats(response.data);
      } catch (error) {
        console.error('Failed to load stats:', error);
        setStats({
          totalUsers: 0,
          totalApplications: 0,
          totalRoles: 0,
          activeUsers: 0,
        });
      } finally {
        setLoading(false);
      }
    };

    loadStats();
  }, []);

  const statCards = [
    {
      title: 'Total Users',
      value: stats?.totalUsers ?? 0,
      icon: Users,
      description: 'Registered users',
    },
    {
      title: 'Applications',
      value: stats?.totalApplications ?? 0,
      icon: AppWindow,
      description: 'OAuth applications',
    },
    {
      title: 'Roles',
      value: stats?.totalRoles ?? 0,
      icon: Shield,
      description: 'User roles',
    },
    {
      title: 'Active Users',
      value: stats?.activeUsers ?? 0,
      icon: Activity,
      description: 'Last 30 days',
    },
  ];

  return (
    <div className='space-y-6'>
      <div>
        <h1 className='text-3xl font-bold tracking-tight'>Dashboard</h1>
        <p className='text-muted-foreground'>
          Overview of your identity management system
        </p>
      </div>

      <div className='grid gap-4 md:grid-cols-2 lg:grid-cols-4'>
        {statCards.map((stat) => (
          <Card key={stat.title}>
            <CardHeader className='flex flex-row items-center justify-between pb-2'>
              <CardTitle className='text-sm font-medium'>
                {stat.title}
              </CardTitle>
              <stat.icon className='h-4 w-4 text-muted-foreground' />
            </CardHeader>
            <CardContent>
              <div className='text-2xl font-bold'>
                {loading ? '-' : stat.value}
              </div>
              <p className='text-xs text-muted-foreground'>
                {stat.description}
              </p>
            </CardContent>
          </Card>
        ))}
      </div>

      <div className='grid gap-4 md:grid-cols-2'>
        <Card>
          <CardHeader>
            <CardTitle>Recent Activity</CardTitle>
            <CardDescription>Latest actions in the system</CardDescription>
          </CardHeader>
          <CardContent>
            <p className='text-sm text-muted-foreground'>
              No recent activity to display
            </p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Quick Actions</CardTitle>
            <CardDescription>Common administrative tasks</CardDescription>
          </CardHeader>
          <CardContent className='space-y-2'>
            <a
              href='/dashboard/users'
              className='block text-sm text-primary hover:underline'
            >
              → Manage Users
            </a>
            <a
              href='/dashboard/applications'
              className='block text-sm text-primary hover:underline'
            >
              → Manage Applications
            </a>
            <a
              href='/dashboard/roles'
              className='block text-sm text-primary hover:underline'
            >
              → Manage Roles
            </a>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
