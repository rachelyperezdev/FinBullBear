export type PortfolioPost = {
    symbol: string,
    companyName: string,
    purchase: number,
    lastDiv: number,
    industry: string,
    marketCap: number
}

export type PortfolioGet = {
    id: number,
    symbol: string,
    companyName: string,
    purchase: number,
    lastDiv: number,
    industry: string,
    marketCap: number,
    comments: any
}