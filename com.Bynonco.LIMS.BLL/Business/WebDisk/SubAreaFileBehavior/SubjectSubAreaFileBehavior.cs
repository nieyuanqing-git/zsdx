using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.SubAreaFileBehavior
{
    public class SubjectSubAreaFileBehavior : SubAreaFileBehaviorBase
    {
        public SubjectSubAreaFileBehavior(User operateUser)
            : base(operateUser)
        {
        }
        public override bool AddValidates(Model.SubAreaFile subAreaFile, ref string message)
        {
            bool result = true;
            if (operateUser.IsWebDiskAdmin) return true;
            if (subAreaFile.Parent != null)
                result = subAreaFile.Parent.IsOwner(operateUser);
            if (!result) message = string.Format("只能在自己创建的文件夹下上传文件,文件夹【{0}】的创建者是【{1}】", subAreaFile.FileName, subAreaFile.OwnerName);
            return result;
        }

        public override bool EditValidates(Model.SubAreaFile subAreaFile, ref string message)
        {
            if (operateUser.IsWebDiskAdmin) return true;
            bool result = subAreaFile.IsOwner(operateUser);
            if (!result && subAreaFile.Parent != null)
                result = subAreaFile.Parent.IsOwner(operateUser);
            if (!result) result = subAreaFile.SubArea.IsAdmin(operateUser);
            if (!result) message = string.Format("只能修改自己创建的文件或者自己创建的文件夹下的文件,{0}【{1}】的创建者是【{2}】", subAreaFile.IsFold ? "文件夹" : "文件", subAreaFile.FileName, subAreaFile.OwnerName);
            return result;
        }

        public override bool DeleteValidates(Model.SubAreaFile subAreaFile, ref string message)
        {
            if (operateUser.IsWebDiskAdmin) return true;
            bool result = subAreaFile.IsOwner(operateUser);
            if (!result && subAreaFile.Parent != null)
                result = subAreaFile.Parent.IsOwner(operateUser);
            if (!result) result = subAreaFile.SubArea.IsAdmin(operateUser);
            if (!result) message = string.Format("只能删除自己创建的文件或者自己创建的文件夹下的文件,{0}【{1}】的创建者是【{2}】", subAreaFile.IsFold ? "文件夹" : "文件", subAreaFile.FileName, subAreaFile.OwnerName);
            return result;
        }

        public override bool MoveValidates(Model.SubAreaFile sourceSubAreaFile, Model.SubAreaFile targetSubAreaFile, ref string message)
        {
            if (operateUser.IsWebDiskAdmin) return true;
            bool isSourceSubAreaFileOwner = false;
            if (IsSubAreaFileOwner(targetSubAreaFile)) return false;
            var result = base.MoveValidates(sourceSubAreaFile, targetSubAreaFile, ref message);
            if (!result) return false;
            isSourceSubAreaFileOwner = IsSubAreaFileOwner(sourceSubAreaFile);
            if (!isSourceSubAreaFileOwner) return false;
            return isSourceSubAreaFileOwner;
        }
    }
}
