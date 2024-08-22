import axios from 'axios'
import { UserProfileToken } from '../Models/User'
import { handleError } from '../Helpers/ErrorHandler'

const api = 'https://localhost:7212/api/'

export const loginAPI = async (username: string, password: string) => {
  try {
    const data = await axios.post<UserProfileToken>(api + 'account/login', {
      username: username,
      password: password,
    })
    return data
  } catch (error) {
    handleError(error)
  }
}

export const registerAPI = async (
  username: string,
  email: string,
  password: string,
  confirmPassword: string
) => {
  try {
    const data = await axios.post<UserProfileToken>(api + 'account/register', {
      username: username,
      email: email,
      password: password,
      confirmPassword: confirmPassword,
    })
    return data
  } catch (error) {
    handleError(error)
  }
}
