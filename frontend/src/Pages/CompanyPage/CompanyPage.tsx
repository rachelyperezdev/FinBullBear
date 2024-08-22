import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router'
import { companyProfile } from '../../api'
import { CompanyProfile } from '../../company'
import CompanyDashboard from '../../Components/CompanyDashboard/CompanyDashboard'
import Sidebar from '../../Components/Sidebar/Sidebar'
import Spinner from '../../Components/Spinner/Spinner'
import Tile from '../../Components/Tile/Tile'
import StockComment from '../../Components/StockComment/StockComment'

interface Props {}

const CompanyPage = (props: Props) => {
  let { ticker } = useParams()
  const [company, setCompany] = useState<CompanyProfile>()

  useEffect(() => {
    const getProfileInit = async () => {
      const result = await companyProfile(ticker!)
      setCompany(result?.data[0])
    }
    getProfileInit()
  }, [])

  return (
    <>
      {company ? (
        <div className='w-full relative flex ct-docs-disable-sidebar-content overflow-x-hidden'>
          <Sidebar />
          <CompanyDashboard ticker={ticker!}>
            <Tile title='Company Name' info={company.companyName} />
            <Tile title='Price' info={'$' + company.price.toString()} />
            <Tile title='Sector' info={company.sector} />
            <Tile title='DCF' info={'$' + company.dcf.toString()} />
            <p className='p-3 mt-1 m-4 bg-white shadow rounded text-medium text-gray-900'>
              {company.description}
            </p>
          </CompanyDashboard>
        </div>
      ) : (
        <Spinner />
      )}
    </>
  )
}

export default CompanyPage
