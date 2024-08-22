import React from 'react'
import { testIncomeStatementData } from './testData'

// const data = testIncomeStatementData

// type Company = (typeof data)[0]

// const configs = [
//   { label: 'Year', render: (company: Company) => company.acceptedDate },
//   {
//     label: 'Cost of Revenue',
//     render: (company: Company) => company.costOfRevenue,
//   },
// ]

type Props = {
  config: any
  data: any
}

const Table = ({ config, data }: Props) => {
  const renderedHeaders = config.map((col: any) => {
    return (
      <th
        key={col.label}
        className='p-4 text-left text-xs font-medium text-gray-500 uppercase tracking-wider'
      >
        {col.label}
      </th>
    )
  })

  const renderedRows = data.map((val: any) => {
    return (
      <tr key={val.cik}>
        {config.map((rowData: any, index: any) => {
          return (
            <td
              key={index}
              className='p-4 whitespace-nowrap text-sm font-normal text-gray-900'
            >
              {rowData.render(val)}
            </td>
          )
        })}
      </tr>
    )
  })
  return (
    <div className='bg-white shadow rounded-lg p-4 sm:p-6 xl:p-8'>
      <table>
        <thead>{renderedHeaders}</thead>
        <tbody>{renderedRows}</tbody>
      </table>
    </div>
  )
}

export default Table
