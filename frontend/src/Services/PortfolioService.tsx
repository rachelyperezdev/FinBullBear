import axios from "axios"
import { handleError } from "../Helpers/ErrorHandler"
import { PortfolioGet, PortfolioPost } from "../Models/Portfolio"

const api = "https://localhost:7212/api/portfolio/"

export const portfolioPostAPI = async (symbol: string) => {
    try{
        const data = await axios.post<PortfolioPost>(api + `?Symbol=${symbol}`)
        return data
    } catch(e){
        handleError(e)
    }
}

export const portfolioDeleteAPI = async (symbol: string) => {
    try{
        const data = await axios.delete<PortfolioPost>(api + `?Symbol=${symbol}`)
        return data
    } catch(e) {
        handleError(e)
    }
}

export const portfolioGetAPI = async () => {
    try{
        const data = await axios.get<PortfolioGet[]>(api)
        return data
    } catch(e){
        handleError(e)
    }
}