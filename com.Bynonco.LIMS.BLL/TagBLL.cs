using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class TagBLL : BLLBase<Tag>, ITagBLL
    {
        public Tag GenerateDefaultTag(string tagName, ref XTransaction tran)
        {
            var tag = GetFirstOrDefaultEntityByExpression(string.Format("Name=\"{0}\"*IsDelete=false", tagName));
            if (tag == null)
            {
                tag = GetFirstOrDefaultEntityByExpression(string.Format("IsDefault=true*IsDelete=false"));
                if (tag == null)
                {
                    tag = new Tag() { Id = Guid.NewGuid(), Name = tagName, IsDefault = false, IsDelete = false, IsStop = false, ParentId = null };
                    tag.XPath = XPathUtility.GenerateXPath(null, null,
                   (entityId) => { return GetEntitiesByExpression(string.Format("Id=\"{0}\"*IsDelete=false", entityId.ToString())).First(); },
                   null,
                   () => { return GetEntitiesByExpression("IsDelete=false"); });
                   Add(new Tag[] { tag }, ref tran, true);
                }
            }
            return tag;
        }
    }
}