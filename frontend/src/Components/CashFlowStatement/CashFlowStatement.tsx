import React, { useEffect, useState } from 'react'
import { useOutletContext } from 'react-router'
import { getCashFlowStatements } from '../../api'
import { CompanyCashFlow } from '../../company'
import { formatLargeMonetaryNumber } from '../../Helpers/NumbersFormatting'
import Spinner from '../Spinner/Spinner'
import Table from '../Table/Table'

const config = [
  {
    label: 'Date',
    render: (company: CompanyCashFlow) => company.date,
  },
  {
    label: 'Operating Cashflow',
    render: (company: CompanyCashFlow) =>
      formatLargeMonetaryNumber(company.operatingCashFlow),
  },
  {
    label: 'Investing Cashflow',
    render: (company: CompanyCashFlow) =>
      formatLargeMonetaryNumber(company.netCashUsedForInvestingActivites),
  },
  {
    label: 'Financing Cashflow',
    render: (company: CompanyCashFlow) =>
      formatLargeMonetaryNumber(
        company.netCashUsedProvidedByFinancingActivities
      ),
  },
  {
    label: 'Cash At End of Period',
    render: (company: CompanyCashFlow) =>
      formatLargeMonetaryNumber(company.cashAtEndOfPeriod),
  },
  {
    label: 'CapEX',
    render: (company: CompanyCashFlow) =>
      formatLargeMonetaryNumber(company.capitalExpenditure),
  },
  {
    label: 'Issuance Of Stock',
    render: (company: CompanyCashFlow) =>
      formatLargeMonetaryNumber(company.commonStockIssued),
  },
  {
    label: 'Free Cash Flow',
    render: (company: CompanyCashFlow) =>
      formatLargeMonetaryNumber(company.freeCashFlow),
  },
]

interface Props {}

const CashFlowStatement = (props: Props) => {
  const ticker = useOutletContext<string>()
  const [cashFlowStatement, setCashFlowStatement] =
    useState<CompanyCashFlow[]>()

  useEffect(() => {
    const fetchCashFlowStatements = async () => {
      const result = await getCashFlowStatements(ticker!)
      setCashFlowStatement(result?.data)
    }
    fetchCashFlowStatements()
  }, [])

  return (
    <>
      {cashFlowStatement ? (
        <>
          <Table config={config} data={cashFlowStatement} />
        </>
      ) : (
        <Spinner />
      )}
    </>
  )
}

export default CashFlowStatement
