import React, { useEffect, useState } from 'react'
import { useOutletContext } from 'react-router'
import { getIncomeStatement } from '../../api'
import { CompanyIncomeStatement } from '../../company'
import {
  formatLargeMonetaryNumber,
  formatRatio,
} from '../../Helpers/NumbersFormatting'
import Spinner from '../Spinner/Spinner'
import Table from '../Table/Table'

interface Props {}

const configs = [
  {
    label: 'Date',
    render: (company: CompanyIncomeStatement) => company.date,
  },
  {
    label: 'Revenue',
    render: (company: CompanyIncomeStatement) =>
      formatLargeMonetaryNumber(company.revenue),
  },
  {
    label: 'Cost Of Revenue',
    render: (company: CompanyIncomeStatement) =>
      formatLargeMonetaryNumber(company.costOfRevenue),
  },
  {
    label: 'Depreciation',
    render: (company: CompanyIncomeStatement) =>
      formatLargeMonetaryNumber(company.depreciationAndAmortization),
  },
  {
    label: 'Operating Income',
    render: (company: CompanyIncomeStatement) =>
      formatLargeMonetaryNumber(company.operatingIncome),
  },
  {
    label: 'Income Before Taxes',
    render: (company: CompanyIncomeStatement) =>
      formatLargeMonetaryNumber(company.incomeBeforeTax),
  },
  {
    label: 'Net Income',
    render: (company: CompanyIncomeStatement) =>
      formatLargeMonetaryNumber(company.netIncome),
  },
  {
    label: 'Net Income Ratio',
    render: (company: CompanyIncomeStatement) =>
      formatRatio(company.netIncomeRatio),
  },
  {
    label: 'Earnings Per Share',
    render: (company: CompanyIncomeStatement) => formatRatio(company.eps),
  },
  {
    label: 'Earnings Per Diluted',
    render: (company: CompanyIncomeStatement) =>
      formatRatio(company.epsdiluted),
  },
  {
    label: 'Gross Profit Ratio',
    render: (company: CompanyIncomeStatement) =>
      formatRatio(company.grossProfitRatio),
  },
  {
    label: 'Opearting Income Ratio',
    render: (company: CompanyIncomeStatement) =>
      formatRatio(company.operatingIncomeRatio),
  },
  {
    label: 'Income Before Taxes Ratio',
    render: (company: CompanyIncomeStatement) =>
      formatRatio(company.incomeBeforeTaxRatio),
  },
]

const IncomeStatement = (props: Props) => {
  // tabla de income
  // estructura de la tabla (config) [√]
  // data de la tabla API [√]

  // get the ticker via the outlet context [√]
  // set the income statement data [√]
  // fetch and render once (useEffect) [√]

  // pass the config and data to Table

  const ticker = useOutletContext<string>()
  const [incomeStatement, setIncomeStatement] =
    useState<CompanyIncomeStatement[]>()

  useEffect(() => {
    const fetchIncomeStament = async () => {
      const result = await getIncomeStatement(ticker!)
      setIncomeStatement(result?.data)
    }
    fetchIncomeStament()
  }, [])

  return (
    <>
      {incomeStatement ? (
        <>
          <Table config={configs} data={incomeStatement} />
        </>
      ) : (
        <Spinner />
      )}
    </>
  )
}

export default IncomeStatement
