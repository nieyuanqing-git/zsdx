using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.SubAreaFileFactory;
using com.Bynonco.LIMS.BLL.SubAreaFileBehavior;

namespace com.Bynonco.LIMS.BLL.SubAreaFileEntity
{
    public class PublicSubAreaFile : SubAreaFileBase
    {
        public PublicSubAreaFile(User operateUser, SubAreaCategory subAreaCategory, SubArea subArea, SubAreaFileFactoryBase subAreaFileFactory)
            : base(operateUser, subAreaCategory, subArea, subAreaFileFactory)
        {
        }
    }
}
