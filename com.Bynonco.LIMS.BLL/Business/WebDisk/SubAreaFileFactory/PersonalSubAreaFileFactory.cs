using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.SubAreaFileEntity;
using com.Bynonco.LIMS.BLL.SubAreaFileBehavior;

namespace com.Bynonco.LIMS.BLL.SubAreaFileFactory
{
    public class PersonalSubAreaFileFactory : SubAreaFileFactoryBase
    {
        public PersonalSubAreaFileFactory(User operateUser, SubAreaCategory subAreaCategory, SubArea subArea):base(operateUser,subAreaCategory,subArea)
        {
        }

        public override SubAreaFileEntity.SubAreaFileBase CreateSubAreaFile()
        {
            return new PersonalSubAreaFile(operateUser, subAreaCategory, subArea, new PersonalSubAreaFileFactory(operateUser, subAreaCategory, subArea));
        }

        public override SubAreaFileBehavior.SubAreaFileBehaviorBase CreateSubAreaFileBehavior()
        {
            return new PersonalSubAreaFileBehavior(operateUser);
        }
    }
}
