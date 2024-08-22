import React, { ChangeEvent, SyntheticEvent, useEffect, useState } from 'react'
import { searchCompanies } from '../../api'
import { CompanySearch } from '../../company'
import CardList from '../../Components/CardList/CardList'
import ListPortfolio from '../../Components/Portfolio/ListPortfolio/ListPortfolio'
import Search from '../../Components/Search/Search'
import { PortfolioGet } from '../../Models/Portfolio'
import { portfolioDeleteAPI, portfolioGetAPI, portfolioPostAPI } from '../../Services/PortfolioService'
import { toast } from 'react-toastify'

interface Props {}

const SearchPage = (props: Props) => {
  const [search, setSearch] = useState<string>('')
  const [portfolioValues, setPortfolioValues] = useState<PortfolioGet[] | null>([])
  const [searchResult, setSearchResult] = useState<CompanySearch[]>([])
  const [serverError, setServerError] = useState<string | null>(null)

  useEffect(() => {
    getPortfolio()
  }, [])

  const handleSearchChange = (e: ChangeEvent<HTMLInputElement>) => {
    setSearch(e.target.value)
  }

  const getPortfolio = () => {
    portfolioGetAPI()
    .then((res) => {
      if(res) {
        setPortfolioValues(res?.data)
      }
    })
    .catch((e) => {
      setPortfolioValues(null)
    })
  }

  const onSearchSubmit = async (e: SyntheticEvent) => {
    e.preventDefault()

    const result = await searchCompanies(search)
    if (typeof result === 'string') {
      setServerError(result)
    } else if (Array.isArray(result.data)) {
      setSearchResult(result.data) 
    }
    console.log(searchResult)
  }

  const onPortfolioCreate = (e: any) => {
    e.preventDefault()

    console.log(e.target[0].value)
    portfolioPostAPI(e.target[0].value)
    .then((res) => {
      if(res?.status === 201) {
        toast.success("Successfully added stock to portfolio")
        getPortfolio()
      }
    })
    .catch((e) => {
      toast.warning("Could not add stock to portfolio")
    })
  }

  const onPortfolioDelete = (e: any) => {
    e.preventDefault()

    portfolioDeleteAPI(e.target[0].value)
    .then((res) => {
      if(res?.status === 204){
        toast.success("Successfully removed stock from portfolio")
        getPortfolio()
      }
    })
    .catch((e) => {
      toast.warning("Error while deleting stock from portfolio")
    })
  }
  return (
    <>
      <Search
        onSearchSubmit={onSearchSubmit}
        search={search}
        handleSearchChange={handleSearchChange}
      />
      {serverError && <h1>{serverError}</h1>}
      <ListPortfolio
        portfolioValues={portfolioValues!}
        onPortfolioDelete={onPortfolioDelete}
      />
      <CardList
        searchResults={searchResult}
        onPortfolioCreate={onPortfolioCreate}
      />
    </>
  )
}

export default SearchPage
