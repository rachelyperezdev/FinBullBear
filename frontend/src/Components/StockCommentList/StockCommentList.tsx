import React, { useEffect, useState } from 'react'
import { commentGetAPI } from '../../Services/CommentService'
import StockCommentListItem from '../StockCommentListItem/StockCommentListItem'
import { CommentGet } from '../../Models/Comment'

type Props = {
    comments: CommentGet[]
}

const StockCommentList = ({ comments }: Props) => {
    return(<>{
        comments ? <>{comments.map((comment, index) => {
            return <StockCommentListItem key={index} comment={comment} />
        })}</> : ''
    }
    </>)
}

export default StockCommentList