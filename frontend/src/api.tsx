import axios, { isAxiosError } from 'axios'
import {
  CompanyBalanceSheet,
  CompanyCashFlow,
  CompanyIncomeStatement,
  CompanyKeyMetrics,
  CompanyProfile,
  CompanySearch,
} from './company'

interface SearchResponse {
  data: CompanySearch[]
}

export const searchCompanies = async (query: string) => {
  try {
    const data = await axios.get<SearchResponse>(
      `https://financialmodelingprep.com/api/v3/search-ticker?query=${query}&limit=10&exchange=NASDAQ&apikey=${
        import.meta.env.VITE_API_KEY
      }`
    )
    return data
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error('error message: ', error.message)
      return error.message
    } else {
      console.error('unexpected error: ', error)
      return 'An expected error has occured.'
    }
  }
}

export const companyProfile = async (query: string) => {
  try {
    const data = axios.get<CompanyProfile[]>(
      `https://financialmodelingprep.com/api/v3/profile/${query}?apikey=${
        import.meta.env.VITE_API_KEY
      }`
    )
    return data
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error('Error: ' + error.message)
    } else {
      console.error('Unexpected error.')
    }
  }
}

export const getKeyMetrics = async (query: string) => {
  try {
    const data = await axios.get<CompanyKeyMetrics[]>(
      `https://financialmodelingprep.com/api/v3/key-metrics-ttm/${query}?apikey=${
        import.meta.env.VITE_API_KEY
      }`
    )
    return data
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error('Error: ' + error)
    } else {
      console.error('Unexpected error: ' + error)
    }
  }
}

export const getIncomeStatement = async (query: string) => {
  try {
    const data = await axios.get<CompanyIncomeStatement[]>(
      `https://financialmodelingprep.com/api/v3/income-statement/${query}?limit=50&apikey=${
        import.meta.env.VITE_API_KEY
      }`
    )
    return data
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error('Error: ' + error)
    } else {
      console.error('Undefined error: ' + error)
    }
  }
}

export const getBalanceSheet = async (query: string) => {
  try {
    const data = await axios.get<CompanyBalanceSheet[]>(
      `https://financialmodelingprep.com/api/v3/balance-sheet-statement/${query}?limit=50&apikey=${
        import.meta.env.VITE_API_KEY
      }`
    )
    return data
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error('Error: ' + error)
    } else {
      console.error('Undefined error: ' + error)
    }
  }
}

export const getCashFlowStatements = async (query: string) => {
  try {
    const data = await axios.get<CompanyCashFlow[]>(
      `https://financialmodelingprep.com/api/v3/cash-flow-statement/${query}?limit=50&apikey=${
        import.meta.env.VITE_API_KEY
      }`
    )
    return data
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error('Error: ' + error)
    } else {
      console.error('Undefined error: ' + error)
    }
  }
}
