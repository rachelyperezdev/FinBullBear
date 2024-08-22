import React from 'react'
import { useAuth } from '../Context/useAuth'
import { Navigate, useLocation } from 'react-router'

type Props = { children: React.ReactNode }

const ProtectedRoute = ({ children } : Props) => {
    const { isLoggedIn } = useAuth()
    const location = useLocation()
  return isLoggedIn() ? <>{ children }</> : <Navigate to="/login" state={{from: location}} replace />
}

export default ProtectedRoute