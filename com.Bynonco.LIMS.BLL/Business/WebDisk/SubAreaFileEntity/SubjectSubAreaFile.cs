using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.BLL.SubAreaFileEntity;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.SubAreaFileBehavior;
using com.Bynonco.LIMS.BLL.SubAreaFileFactory;

namespace com.Bynonco.LIMS.BLL.SubAreaFileEntity
{
    public class SubjectSubAreaFile:SubAreaFileBase
    {
        public SubjectSubAreaFile(User operateUser, SubAreaCategory subAreaCategory, SubArea subArea, SubAreaFileFactoryBase subAreaFileFactory)
            : base(operateUser, subAreaCategory, subArea, subAreaFileFactory)
        {
        }
       
    }
}
