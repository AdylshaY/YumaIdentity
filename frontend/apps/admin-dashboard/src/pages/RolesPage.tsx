import { useEffect, useState } from 'react';
import {
  Input,
  Card,
  CardContent,
  CardHeader,
  Button,
} from '@repo/ui/index.ts';
import { adminApi, type Role } from '../lib/api';
import { Plus, Search, MoreHorizontal, Loader2 } from 'lucide-react';

export function RolesPage() {
  const [roles, setRoles] = useState<Role[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchQuery, setSearchQuery] = useState('');

  useEffect(() => {
    const loadRoles = async () => {
      try {
        const response = await adminApi.getRoles();
        setRoles(response.data);
      } catch (error) {
        console.error('Failed to load roles:', error);
      } finally {
        setLoading(false);
      }
    };

    loadRoles();
  }, []);

  const filteredRoles = roles.filter(
    (role) =>
      role.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
      role.description?.toLowerCase().includes(searchQuery.toLowerCase())
  );

  return (
    <div className='space-y-6'>
      <div className='flex items-center justify-between'>
        <div>
          <h1 className='text-3xl font-bold tracking-tight'>Roles</h1>
          <p className='text-muted-foreground'>
            Manage user roles and permissions
          </p>
        </div>
        <Button>
          <Plus className='mr-2 h-4 w-4' />
          Add Role
        </Button>
      </div>

      <Card>
        <CardHeader>
          <div className='flex items-center gap-4'>
            <div className='relative flex-1'>
              <Search className='absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground' />
              <Input
                placeholder='Search roles...'
                className='pl-9'
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
              />
            </div>
          </div>
        </CardHeader>
        <CardContent>
          {loading ? (
            <div className='flex justify-center py-8'>
              <Loader2 className='h-8 w-8 animate-spin text-muted-foreground' />
            </div>
          ) : filteredRoles.length === 0 ? (
            <div className='text-center py-8 text-muted-foreground'>
              {searchQuery
                ? 'No roles found matching your search'
                : 'No roles yet'}
            </div>
          ) : (
            <div className='grid gap-4 md:grid-cols-2 lg:grid-cols-3'>
              {filteredRoles.map((role) => (
                <Card key={role.id}>
                  <CardContent className='pt-6'>
                    <div className='flex items-start justify-between'>
                      <div>
                        <h3 className='font-semibold'>{role.name}</h3>
                        <p className='mt-1 text-sm text-muted-foreground'>
                          {role.description || 'No description'}
                        </p>
                        <p className='mt-2 text-xs text-muted-foreground'>
                          Created:{' '}
                          {new Date(role.createdAt).toLocaleDateString()}
                        </p>
                      </div>
                      <Button variant='ghost' size='icon'>
                        <MoreHorizontal className='h-4 w-4' />
                      </Button>
                    </div>
                  </CardContent>
                </Card>
              ))}
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
