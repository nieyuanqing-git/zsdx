using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.BLL.SubAreaBehavior;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL.Factory
{
    public class SubAreaBehaviorFactory
    {
        public static SubAreaBehaviorBase GetSubAreaBehavior(SubAreaCategoryType subAreaCategoryType)
        {
            switch (subAreaCategoryType)
            {
                case SubAreaCategoryType.Personal:
                    return new PeronalSubAreaBehavior();
                case SubAreaCategoryType.Public:
                    return new PublicSubAreaBehavior();
                case SubAreaCategoryType.Subject:
                    return new SubjectSubAreaBehavior();
                default: throw new Exception("分区分类信息错误,创建分区行为对象失败");
            }
        }
    }
}
