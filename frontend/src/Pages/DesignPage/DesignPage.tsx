import React from 'react'
import { CompanyKeyMetrics } from '../../company'
import RatioList from '../../Components/RatioList/RatioList'
import Table from '../../Components/Table/Table'
import { testIncomeStatementData } from '../../Components/Table/testData'

type Props = {}

const tableConfig = [
  {
    label: 'Market Cap',
    render: (company: CompanyKeyMetrics) => company.marketCapTTM,
    subTitle: "Total value of all a company's shares of stock",
  },
]

const DesignPage = (props: Props) => {
  return (
    <>
      <h1>
        Design guide- This is the design guide for FinBullBear. These are
        reusable components of the app with brief instructions on how to use
        them.
      </h1>
      <h2>
        Table - Table takes in a configuration object and company data as
        params. Use the config to style your table.
      </h2>
      <RatioList data={testIncomeStatementData} config={tableConfig} />
      <Table config={tableConfig} data={testIncomeStatementData} />
    </>
  )
}

export default DesignPage
