using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;

namespace com.Bynonco.LIMS.BLL
{
    public class WorkGroupBLL : BLLBase<WorkGroup>, IWorkGroupBLL
    {
        public WorkGroup GenerateDefaultWorkGroup(string workGroupName, ref XTransaction tran)
        {
            
            var workGroup = GetFirstOrDefaultEntityByExpression(string.Format("Name=\"{0}\"*IsDelete=false*IsStop=false", workGroupName.Trim()));
            if (workGroup == null)
            {
                workGroup = new WorkGroup()
                {
                    Id = Guid.NewGuid(),
                    Name = workGroupName,
                    IsStop = false,
                    IsAdmin = false,
                    IsDelete = false,
                    ParentId = null
                };
               Add(new WorkGroup[] { workGroup }, ref tran, true);
            }
            return workGroup;
        }
    }
}