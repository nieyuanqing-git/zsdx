using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.SubAreaFileBehavior;
using com.Bynonco.LIMS.BLL.SubAreaFileFactory;

namespace com.Bynonco.LIMS.BLL.SubAreaFileEntity
{
    public abstract class SubAreaFileBase
    {
        protected SubAreaFileBehaviorBase subAreaFileBehavior;
        protected User operateUser;
        protected SubAreaCategory subAreaCategory;
        protected SubArea subArea;
        protected SubAreaFileFactoryBase subAreaFileFactory;
        public SubAreaFileBase(User operateUser, SubAreaCategory subAreaCategory, SubArea subArea, SubAreaFileFactoryBase subAreaFileFactory)
        {
            this.operateUser = operateUser;
            this.subAreaCategory = subAreaCategory;
            this.subAreaFileFactory = subAreaFileFactory;
            this.subAreaFileBehavior = subAreaFileFactory.CreateSubAreaFileBehavior();
            this.subArea = subArea;
        }
        public virtual bool Add(Model.SubAreaFile[] subAreaFiles, ref string message)
        {
            return subAreaFileBehavior.Add(subAreaFiles,ref message);
        }

        public virtual bool Edit(Model.SubAreaFile[] subAreaFiles, ref string message)
        {
            return subAreaFileBehavior.Edit(subAreaFiles,ref message);
        }

        public virtual bool Delete(Guid[] ids, ref string message)
        {
             return subAreaFileBehavior.Delete(ids,ref message);
        }

        public virtual bool Move(Model.SubAreaFile[] sourceSubAreaFiles, Model.SubAreaFile targetSubAreaFile, ref string message)
        {
           return subAreaFileBehavior.Move(sourceSubAreaFiles,targetSubAreaFile,ref message);
        }

        public virtual bool AddValidates(Model.SubAreaFile subAreaFile, ref string message)
        {
            return subAreaFileBehavior.AddValidates(subAreaFile, ref message);
        }

        public virtual bool EditValidates(Model.SubAreaFile subAreaFile, ref string message)
        {
            return subAreaFileBehavior.EditValidates(subAreaFile, ref message);
        }

        public virtual bool DeleteValidates(Model.SubAreaFile subAreaFile, ref string message)
        {
            return subAreaFileBehavior.DeleteValidates(subAreaFile, ref message);
        }

        public virtual bool MoveValidates(Model.SubAreaFile sourceSubAreaFile, Model.SubAreaFile targetSubAreaFile, ref string message)
        {
            return subAreaFileBehavior.MoveValidates(sourceSubAreaFile,targetSubAreaFile,ref message);
        }

        public virtual bool Validates(Model.SubAreaFile subAreaFile, ref string message)
        {
            return subAreaFileBehavior.Validates(subAreaFile,ref message);
        }
    }
}
