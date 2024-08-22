import React, { useEffect, useState } from 'react'
import StockCommentForm from './StockCommentForm/StockCommentForm'
import { commentGetAPI, commentPostAPI } from '../../Services/CommentService'
import { toast } from 'react-toastify'
import { CommentGet } from '../../Models/Comment'
import StockCommentList from '../StockCommentList/StockCommentList'
import Spinner from '../Spinner/Spinner'

type Props = {
  stockSymbol: string
}

type CommentFormInputs = {
  title: string,
  content: string
}

const StockComment = ({stockSymbol}: Props) => {
  const [comments, setComments] = useState<CommentGet[] | null>(null)
  const [isLoading, setIsLoading] = useState<boolean>()

  useEffect(() => {
    getComments()
  }, [])

  const handleComment = (e: CommentFormInputs) => {
    commentPostAPI(e.title, e.content, stockSymbol)?.then((res) => {
      if(res){
        toast.success("Comment posted successfully")
        getComments()
      }
    }).catch((e) => toast.error(e))
  }

  const getComments = () => {
    setIsLoading(true)
    commentGetAPI(stockSymbol).then((res) => {
      setComments(res?.data!)
    })
    setIsLoading(false)
  }
  return (
    <div className="flex flex-col">
      {isLoading ? <Spinner /> : <StockCommentList comments={comments!} />}
      <StockCommentForm stockSymbol={stockSymbol} handleComment={handleComment}/>
    </div>
    
  )
}

export default StockComment