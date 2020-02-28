using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.SubAreaFileEntity;
using com.Bynonco.LIMS.BLL.SubAreaFileBehavior;

namespace com.Bynonco.LIMS.BLL.SubAreaFileFactory
{
    public class SubjectSubAreaFileFactory : SubAreaFileFactoryBase
    {
        public SubjectSubAreaFileFactory(User operateUser, SubAreaCategory subAreaCategory, SubArea subArea)
            : base(operateUser, subAreaCategory, subArea)
        {
        }

        public override SubAreaFileEntity.SubAreaFileBase CreateSubAreaFile()
        {
            return new SubjectSubAreaFile(operateUser, subAreaCategory, subArea,new SubjectSubAreaFileFactory(operateUser,subAreaCategory,subArea));
        }

        public override SubAreaFileBehavior.SubAreaFileBehaviorBase CreateSubAreaFileBehavior()
        {
            return new SubjectSubAreaFileBehavior(operateUser);
        }
    }
}
