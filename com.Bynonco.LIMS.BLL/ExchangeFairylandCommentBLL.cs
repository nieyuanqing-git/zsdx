using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;

namespace com.Bynonco.LIMS.BLL
{
    public class ExchangeFairylandCommentBLL : BLLBase<ExchangeFairylandComment>, IExchangeFairylandCommentBLL
    {
        /// <summary>
        /// 获取某一个评论的层次所有被评论的评论(递归)
        /// </summary>
        /// <param name="model">某一条评论</param>
        /// <param name="models"></param>
        /// <param name="ReplyCommentList"></param>
        public void GetReplyedComments(ExchangeFairylandComment model, List<ExchangeFairylandComment> models, List<ExchangeFairylandComment> ReplyedCommentList)
        {
            if (model.ReferenceId != null)
            {
                var ReplyedComment = models.Find(m => m.Id == model.ReferenceId);
                ReplyedCommentList.Add(ReplyedComment);
                GetReplyedComments(ReplyedComment, models, ReplyedCommentList);
            }
        }

        /// <summary>
        /// 某一条评论，获取所有对这条评论的评论（递归）------主要用于删除
        /// </summary>
        /// <param name="model"></param>
        /// <param name="models"></param>
        /// <param name="ReplyCommentList"></param>
        public void GetReplyComments(ExchangeFairylandComment model, List<ExchangeFairylandComment> models, List<ExchangeFairylandComment> ReplyCommentList)
        {
            var ReplyComments = models.FindAll(m => m.ReferenceId == model.Id);
            if (ReplyComments != null && ReplyComments.Count > 0)
            {
                ReplyCommentList.AddRange(ReplyComments);
                foreach (var item in ReplyComments)
                {
                    GetReplyComments(item, models, ReplyCommentList);
                }
            }
        }
    }
}
