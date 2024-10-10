import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App.jsx'

import { AuthProvider } from './Context/AuthContext.jsx'
import './index.css'
import { BrowserRouter as Router } from 'react-router-dom';

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <Router>
   <AuthProvider>
    <App />
    </AuthProvider> 
    </Router>
  </StrictMode>,
)
