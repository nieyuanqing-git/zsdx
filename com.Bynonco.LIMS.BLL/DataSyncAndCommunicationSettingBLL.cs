using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.BLL.Business.Customer.Factory;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class DataSyncAndCommunicationSettingBLL : IDataSyncAndCommunicationSettingBLL
    {
        private IDictCodeTypeBLL __dictCodeTypeBLL;
        public DataSyncAndCommunicationSettingBLL()
        {
            __dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
        }
        public Model.Business.DataSyncAndCommunicationSetting GetDataSyncSetting()
        {
            string errorMessage = "";
            var dataSyncAndCommunicationSetting = new DataSyncAndCommunicationSetting();
            try
            {
               var zdConfig= __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression(string.Format("Code=ZdSynConfig"));
                if(zdConfig!= null && zdConfig.DictCodes != null && zdConfig.DictCodes.Count > 0)
                {
                    dataSyncAndCommunicationSetting.IsAutoCostDeductSync = zdConfig.DictCodes.First(p => p.Code.Trim() == "IsAutoCostDeductSync").Value.Trim() == "1";
                }

                var dictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=DataSyncSetting");
                if (dictCodeType != null && dictCodeType.DictCodes != null && dictCodeType.DictCodes.Count > 0)
                {
                    dataSyncAndCommunicationSetting.IsAutoUserDataSync = dictCodeType.DictCodes.First(p => p.Code.Trim() == "IsAutoUserDataSync").Value.Trim() == "1";
                    dataSyncAndCommunicationSetting.IsAutoOrganizationSync = dictCodeType.DictCodes.First(p => p.Code.Trim() == "IsAutoOrganizationSync").Value.Trim() == "1";
                    dataSyncAndCommunicationSetting.IsAutoSubjectProjectImcomeSync = dictCodeType.DictCodes.First(p => p.Code.Trim() == "IsAutoSubjectProjectImcomeSync").Value.Trim() == "1";
                    dataSyncAndCommunicationSetting.IsAutoRegisterAppointmentsOfTimeout = dictCodeType.DictCodes.First(p => p.Code.Trim() == "IsAutoRegisterAppointmentsOfTimeout").Value.Trim() == "1";
                    dataSyncAndCommunicationSetting.IsAutoCancelAppoinment = dictCodeType.DictCodes.First(p => p.Code.Trim() == "IsAutoCancelAppoinment").Value.Trim() == "1";
                    dataSyncAndCommunicationSetting.IsAutoGetZjdxUser = dictCodeType.DictCodes.Any(p => p.Code.Trim() == "IsAutoGetZjdxUser") && dictCodeType.DictCodes.First(p => p.Code.Trim() == "IsAutoGetZjdxUser").Value.Trim() == "1";
                    dataSyncAndCommunicationSetting.IsRegisterAppointmentOfTimeoutWhenAutoCancel = dictCodeType.DictCodes.Any(p => p.Code.Trim() == "IsRegisterAppointmentOfTimeoutWhenAutoCancel") && dictCodeType.DictCodes.First(p => p.Code.Trim() == "IsRegisterAppointmentOfTimeoutWhenAutoCancel").Value.Trim() == "1";
                    dataSyncAndCommunicationSetting.IsAutoEquipmentDataSync = dictCodeType.DictCodes.First(p => p.Code.Trim() == "IsAutoEquipmentDataSync").Value.Trim() == "1";
                    //if (CustomerFactory.GetCustomer().GetSchoolName() == HDSFDX.SchoolName)
                    //{
                    //    var autoOpenFundApplyDataSyncHour = dictCodeType.DictCodes.FirstOrDefault(p => p.Code.Trim() == "AutoOpenFundApplyDataSyncHour");
                    //    if (autoOpenFundApplyDataSyncHour != null)
                    //    {
                    //        int hour;
                    //        if (int.TryParse(autoOpenFundApplyDataSyncHour.Value, out hour))
                    //        {
                    //            if (hour >= 0 && hour < 24)
                    //            {
                    //                dataSyncAndCommunicationSetting.AutoOpenFundApplyDataSyncHour = hour;
                    //            }
                    //        }
                    //    }
                    //}
                }
                IList<CollegeUploadSetting> collegeUploadSettingList = new List<CollegeUploadSetting>();
                var collegeUploadSettingDictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=CollegeUploadSetting");
                if (collegeUploadSettingDictCodeType != null)
                {
                    var collegeUploadSettingDictCodeTypeChildren = __dictCodeTypeBLL.GetEntitiesByExpression(string.Format("ParentId=\"{0}\"", collegeUploadSettingDictCodeType.Id));
                    if (collegeUploadSettingDictCodeTypeChildren != null && collegeUploadSettingDictCodeTypeChildren.Count > 0)
                    {
                        foreach (var collegeUploadSettingDictCodeTypeChild in collegeUploadSettingDictCodeTypeChildren)
                        {
                            if (collegeUploadSettingDictCodeTypeChild.DictCodes != null && collegeUploadSettingDictCodeTypeChild.DictCodes.Count > 0)
                            {
                                var collegeUploadSetting = new CollegeUploadSetting();
                                var orgIdDictCode = collegeUploadSettingDictCodeTypeChild.DictCodes.FirstOrDefault(p => p.Code == "OrgId" && !string.IsNullOrWhiteSpace(p.Value) && p.Value.IsGuid());
                                if (orgIdDictCode != null) collegeUploadSetting.CollegeId = new Guid(orgIdDictCode.Value);
                                var userNameDictCode = collegeUploadSettingDictCodeTypeChild.DictCodes.FirstOrDefault(p => p.Code == "UserName" && !string.IsNullOrWhiteSpace(p.Value));
                                if (userNameDictCode != null) collegeUploadSetting.UserName = userNameDictCode.Value;
                                var passwordDictCode = collegeUploadSettingDictCodeTypeChild.DictCodes.FirstOrDefault(p => p.Code == "Password" && !string.IsNullOrWhiteSpace(p.Value));
                                if (passwordDictCode != null) collegeUploadSetting.Password = passwordDictCode.Value;
                                var isAutoUploadEquipmentInfoDictCode = collegeUploadSettingDictCodeTypeChild.DictCodes.FirstOrDefault(p => p.Code == "IsAutoUploadEquipmentInfo" && !string.IsNullOrWhiteSpace(p.Value));
                                if (isAutoUploadEquipmentInfoDictCode != null) collegeUploadSetting.IsAutoUploadEquipmentInfo = isAutoUploadEquipmentInfoDictCode.Value == "1";
                                var uploadEquipmentInfoURLDictCode = collegeUploadSettingDictCodeTypeChild.DictCodes.FirstOrDefault(p => p.Code == "UploadEquipmentInfoURL" && !string.IsNullOrWhiteSpace(p.Value));
                                if (uploadEquipmentInfoURLDictCode != null) collegeUploadSetting.UploadEquipmentInfoURL = uploadEquipmentInfoURLDictCode.Value;
                                if (collegeUploadSetting.IsAutoUploadEquipmentInfo && collegeUploadSetting.Validate(out errorMessage))
                                {
                                    collegeUploadSettingList.Add(collegeUploadSetting);
                                }
                            }
                        }
                    }
                }
                dataSyncAndCommunicationSetting.CollegeUploadSettingList = collegeUploadSettingList;
                return dataSyncAndCommunicationSetting;
            }
            catch (Exception ex) { }
            return dataSyncAndCommunicationSetting;
        }
    }
}
