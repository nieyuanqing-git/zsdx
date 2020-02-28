using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.Model.Business.CERS.InstrusAndGroups;

namespace com.Bynonco.LIMS.BLL
{
    public class EquipmentGroupBLL : BLLBase<EquipmentGroup>, IEquipmentGroupBLL
    {
        private IDictCodeBLL __dictCodeBLL = null;
        public EquipmentGroupBLL()
        {
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<EquipmentGroup> equipmentGroupList, bool isSupressLazyLoad)
        {
            EquipmentGroupPrivilige equipmentGroupPrivilige = null;
            if (userId.HasValue) equipmentGroupPrivilige = PriviligeFactory.GetEquipmentGroupPrivilige(userId.Value);
            if (equipmentGroupList != null && equipmentGroupList.Count > 0)
            {
                foreach (var EquipmentGroup in equipmentGroupList)
                {
                    EquipmentGroup.IsSupressLazyLoad = false;
                    EquipmentGroup.InitOperate();
                    OperateDecorate(userId, EquipmentGroup, equipmentGroupPrivilige);
                    EquipmentGroup.IsSupressLazyLoad = isSupressLazyLoad;
                }
            }
        }
        private void OperateDecorate(Guid? userId, EquipmentGroup equipmentGroup, EquipmentGroupPrivilige equipmentGroupPrivilige)
        {
            if (equipmentGroup == null) throw new ArgumentException("仪器组为空");
            if (equipmentGroupPrivilige == null)
            {
                equipmentGroup.EditOperate = "";
                return;
            }
            if (!equipmentGroupPrivilige.IsEnableEdit)
            {
                equipmentGroup.EditOperate = "";
            }

        }
        public IList<EquipmentGroup> GetEquipmentGroupListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var equipmentGroupList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, equipmentGroupList, isSupressLazyLoad);
            return equipmentGroupList;
        }
        public IList<EquipmentGroup> GetEquipmentGroupListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var equipmentGroupList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, equipmentGroupList, isSupressLazyLoad);
            return equipmentGroupList;
        }
        public IList<EquipmentGroup> GetEquipmentGroupListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var equipmentGroupList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, equipmentGroupList, isSupressLazyLoad);
            return equipmentGroupList;
        }
        public GridData<EquipmentGroup> GetGridEquipmentGroupListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var equipmentGroupList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, equipmentGroupList == null ? null : equipmentGroupList.Data, isSupressLazyLoad);
            return equipmentGroupList;
        }
        public GridData<EquipmentGroup> GetGridEquipmentGroupListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var equipmentGroupList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, equipmentGroupList == null ? null : equipmentGroupList.Data, isSupressLazyLoad);
            return equipmentGroupList;
        }
        public RootGroupsGroup[] FillGroups()
        {
            var equipmentGroupList = GetEntitiesByExpression("IsDelete=false*IsStop=false",null,"InnerID");
            if (equipmentGroupList == null || equipmentGroupList.Count() == 0) return null;
            RootGroupsGroup[] Groups = new RootGroupsGroup[equipmentGroupList.Count()];
            var schoolCode = __dictCodeBLL.GetDictCodeValueByCode("System", "SchoolCode");
            for (int i = 0; i < equipmentGroupList.Count(); i++)
            {
                Groups[i] = FillGroup(equipmentGroupList[i]);
                if (Groups[i] != null)
                {
                    Groups[i].SchoolCode = schoolCode;
                }
            }
            return Groups;
        }
        private RootGroupsGroup FillGroup(EquipmentGroup equipmentGroup)
        {
            RootGroupsGroup group = new RootGroupsGroup();
            //com.august.Core.Utility.Tools.DeeplyCopy(equipmentGroup, group);


            group.InnerID = equipmentGroup.InnerID;
            group.Name = equipmentGroup.Name;
            group.ShortName = equipmentGroup.InputStr;
            group.Type = equipmentGroup.Type;
            group.Principal = equipmentGroup.AdminNames;
            group.ContactPerson = equipmentGroup.ContactPerson;
            group.Address = equipmentGroup.Address;
            group.ZIPCode = equipmentGroup.ZIPCode;
            group.Telephone = equipmentGroup.Telephone;
            group.Fax = equipmentGroup.Fax;
            group.Email = equipmentGroup.Email;
            if (equipmentGroup.StartDate.HasValue)
            {
                group.StartDateSpecified = true;
                group.StartDate = equipmentGroup.StartDate.Value.Date;
            }
            else group.StartDateSpecified = false; ;
            group.ImageURL = equipmentGroup.ImageURL;
            group.Introduction = equipmentGroup.Introduction;
            group.Strength = equipmentGroup.Strength;
            group.Expert = equipmentGroup.Expert;
            group.Instrument = equipmentGroup.Instrument;
            group.Achievement = equipmentGroup.Achievement;
            group.WebSite = equipmentGroup.WebSite;
            group.UpdateDate = equipmentGroup.UpdateDate;
            group.OtherInfo = equipmentGroup.Remark;
            return group;
        }
    }
}
