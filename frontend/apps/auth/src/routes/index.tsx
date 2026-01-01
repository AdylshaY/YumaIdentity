import { createBrowserRouter, RouterProvider, Navigate } from 'react-router-dom';
import { AuthorizePage } from '../pages/authorize/AuthorizePage';
import { LoginPage } from '../pages/login/LoginPage';
import { RegisterPage } from '../pages/register/RegisterPage';

const router = createBrowserRouter([
  {
    path: '/authorize',
    element: <AuthorizePage />,
  },
  {
    path: '/login',
    element: <LoginPage />,
  },
  {
    path: '/register',
    element: <RegisterPage />,
  },
  {
    path: '*',
    element: <Navigate to="/login" replace />,
  },
]);

export function AppRoutes() {
  return <RouterProvider router={router} />;
}
