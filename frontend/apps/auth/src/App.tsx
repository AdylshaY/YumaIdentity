import { QueryProvider } from './providers/QueryProvider';
import { AppRoutes } from './routes';
import './index.css';

function App() {
  return (
    <QueryProvider>
      <AppRoutes />
    </QueryProvider>
  );
}

export default App;
