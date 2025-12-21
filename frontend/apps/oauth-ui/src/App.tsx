import './App.css'

function App() {
  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100 flex items-center justify-center p-4">
      <div className="max-w-md w-full bg-white rounded-2xl shadow-2xl p-8 space-y-6">
        {/* Header */}
        <div className="text-center space-y-2">
          <h1 className="text-3xl font-bold text-gray-900">
            YumaIdentity
          </h1>
          <p className="text-sm text-gray-600">
            OAuth2 + PKCE Implementation
          </p>
        </div>

        {/* Tailwind CSS Test Card */}
        <div className="bg-gradient-to-r from-purple-500 to-pink-500 rounded-xl p-6 text-white space-y-4">
          <div className="flex items-center space-x-3">
            <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
            <h2 className="text-xl font-semibold">Setup Complete!</h2>
          </div>
          
          <div className="space-y-2">
            <div className="flex items-center space-x-2 text-sm">
              <span className="w-2 h-2 bg-green-300 rounded-full"></span>
              <span>Vite + React 19</span>
            </div>
            <div className="flex items-center space-x-2 text-sm">
              <span className="w-2 h-2 bg-green-300 rounded-full"></span>
              <span>TypeScript 5.9</span>
            </div>
            <div className="flex items-center space-x-2 text-sm">
              <span className="w-2 h-2 bg-green-300 rounded-full"></span>
              <span>Tailwind CSS</span>
            </div>
            <div className="flex items-center space-x-2 text-sm">
              <span className="w-2 h-2 bg-green-300 rounded-full"></span>
              <span>Turborepo + pnpm</span>
            </div>
          </div>
        </div>

        {/* Interactive Button Test */}
        <button className="w-full bg-indigo-600 hover:bg-indigo-700 text-white font-medium py-3 px-4 rounded-lg transition-colors duration-200 transform hover:scale-105">
          Tailwind Hover Effect Test
        </button>

        {/* Grid Test */}
        <div className="grid grid-cols-3 gap-3">
          <div className="bg-blue-500 rounded-lg h-16 flex items-center justify-center text-white font-semibold">1</div>
          <div className="bg-purple-500 rounded-lg h-16 flex items-center justify-center text-white font-semibold">2</div>
          <div className="bg-pink-500 rounded-lg h-16 flex items-center justify-center text-white font-semibold">3</div>
        </div>

        {/* Footer */}
        <p className="text-center text-xs text-gray-500">
          All systems operational 🚀
        </p>
      </div>
    </div>
  )
}

export default App
