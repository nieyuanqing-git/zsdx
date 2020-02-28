using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.BLL.SubAreaFileEntity;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.SubAreaFileBehavior;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.BLL.SubAreaFileFactory;

namespace com.Bynonco.LIMS.BLL.SubAreaFileFactory
{
    public abstract class SubAreaFileFactoryBase
    {
        protected User operateUser;
        protected SubAreaCategory subAreaCategory;
        protected SubArea subArea;
        public SubAreaFileFactoryBase(User operateUser, SubAreaCategory subAreaCategory, SubArea subArea)
        {
            this.operateUser = operateUser;
            this.subAreaCategory = subAreaCategory;
            this.subArea = subArea;
        }
        public abstract SubAreaFileBase CreateSubAreaFile();
        public abstract SubAreaFileBehaviorBase CreateSubAreaFileBehavior();
        public static SubAreaFileBase CreateSubAreaFile(SubAreaCategory subAreaCategory, User operateUser, SubArea subArea)
        {
            if(subAreaCategory==null) return null;
            switch (subAreaCategory.CategoryTypeEnum)
            {
                case SubAreaCategoryType.Personal:
                    return new PersonalSubAreaFile(operateUser, subAreaCategory, subArea, new PersonalSubAreaFileFactory(operateUser, subAreaCategory, subArea));
                case SubAreaCategoryType.Public:
                    return new PublicSubAreaFile(operateUser, subAreaCategory, subArea, new PublicSubAreaFileFactory(operateUser, subAreaCategory, subArea));
                case SubAreaCategoryType.Subject:
                    return new SubjectSubAreaFile(operateUser, subAreaCategory, subArea, new SubjectSubAreaFileFactory(operateUser, subAreaCategory, subArea));
                default: throw new Exception("分区编码类型错误");
            }
        }
        public static SubAreaFileBehaviorBase CreateSubAreaFileBehavior(SubAreaCategory subAreaCategory, User operateUser)
        {
            if (subAreaCategory == null) return null;
            switch (subAreaCategory.CategoryTypeEnum)
            {
                case SubAreaCategoryType.Personal:
                    return new PersonalSubAreaFileBehavior(operateUser);
                case SubAreaCategoryType.Public:
                    return new PublicSubAreaFileBehavior(operateUser);
                case SubAreaCategoryType.Subject:
                    return new SubjectSubAreaFileBehavior(operateUser);
                default: throw new Exception("分区编码类型错误");
            }
        }
    }
}
