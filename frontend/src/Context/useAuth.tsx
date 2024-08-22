import React, { createContext, useEffect, useState } from 'react'
import { UserProfile } from '../Models/User'
import { useNavigate } from 'react-router'
import { toast } from 'react-toastify'
import { loginAPI, registerAPI } from '../Services/AuthService'
import axios from 'axios'

type UserContextType = {
  user: UserProfile | null
  token: string | null
  registerUser: (
    username: string,
    email: string,
    password: string,
    confirmPassword: string
  ) => void
  loginUser: (username: string, password: string) => void
  logOut: () => void
  isLoggedIn: () => boolean
}

type Props = {
  children: React.ReactNode
}

const UserContext = createContext<UserContextType>({} as UserContextType)

export const UserProvider = ({ children }: Props) => {
  const navigate = useNavigate()
  const [token, setToken] = useState<string | null>(null)
  const [user, setUser] = useState<UserProfile | null>(null)
  const [isReady, setIsReady] = useState<boolean>(false)

  useEffect(() => {
    const user = localStorage.getItem('user')
    const token = localStorage.getItem('token')

    if (user && token) {
      setUser(JSON.parse(user))
      setToken(token)
      axios.defaults.headers.common['Authorization'] = 'Bearer ' + token
    }
    setIsReady(true)
  }, [])

  const registerUser = async (
    username: string,
    email: string,
    password: string,
    confirmPassword: string
  ) => {
    await registerAPI(username, email, password, confirmPassword)
      .then((res) => {
        if (res) {
          localStorage.setItem('token', res?.data.token)
          const userObj = {
            userName: res?.data.userName,
            email: res?.data.email,
          }
          localStorage.setItem('user', JSON.stringify(userObj))

          setToken(res?.data.token!)
          setUser(userObj!)

          toast.success('Login success')
          navigate('/search')
        }
      })
      .catch((e) => toast.warning('Server error has occurred'))
  }

  const loginUser = async (username: string, password: string) => {
    await loginAPI(username, password)
      .then((res) => {
        if (res) {
          localStorage.setItem('token', res?.data.token)

          const userObj = {
            userName: res?.data.userName,
            email: res?.data.email,
          }

          localStorage.setItem('user', JSON.stringify(userObj!))

          axios.defaults.headers.common["Authorization"] = `Bearer ${res.data.token}`;

          setToken(res?.data.token!)
          setUser(userObj!)

          toast.success('Login success!')
          navigate('/search')
        }
      })
      .catch((e) => toast.warning('Error has occurred in server: ' + e))
  }

  const isLoggedIn = () => {
    return !!user
  }

  const logOut = () => {
    localStorage.removeItem('token')
    localStorage.removeItem('user')
    delete axios.defaults.headers.common["Authorization"];
    setUser(null)
    setToken('')
    navigate('/')
  }

  return (
    <UserContext.Provider
      value={{ loginUser, user, token, logOut, isLoggedIn, registerUser }}
    >
      {isReady ? children : null}
    </UserContext.Provider>
  )
}

export const useAuth = () => React.useContext(UserContext)
