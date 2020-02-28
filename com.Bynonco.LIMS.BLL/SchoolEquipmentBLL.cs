using com.august.DataLink;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace com.Bynonco.LIMS.BLL
{
    public class SchoolEquipmentBLL : BLLBase<SchoolEquipment>, ISchoolEquipmentBLL
    {
        private ILabOrganizationBLL __labOrganizationBLL;
        public SchoolEquipmentBLL()
        {
            __labOrganizationBLL = BLLFactory.CreateLabOrganizationBLL();
        }
        public string BatchSync(string equipmentSyncInfoListJsonData, Guid collegeId, out int totalCount, out int successCount, out int failCount, bool isReturnSimpleLog = false)
        {
            totalCount = 0;
            successCount = 0;
            failCount = 0;
            try
            {
                if(string.IsNullOrWhiteSpace(equipmentSyncInfoListJsonData)) throw new ArgumentException("设备信息为空");
                IEnumerable<SPEquipment> equipmentSyncInfoList = Newtonsoft.Json.JavaScriptConvert.DeserializeObject<IList<SPEquipment> >(equipmentSyncInfoListJsonData);
                var spDataSyncLogList = BatchSync(equipmentSyncInfoList, collegeId, out totalCount, out successCount, out failCount);
                return !isReturnSimpleLog ? Newtonsoft.Json.JavaScriptConvert.SerializeObject(spDataSyncLogList) : spDataSyncLogList.ToFormatString();
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JavaScriptConvert.SerializeObject(new List<SPDataSyncLog>() { new SPDataSyncLog(false, null, null, "执行出错,错误消息:" + ex.Message) });
            }
        }
        public IList<SPDataSyncLog> BatchSync(IEnumerable<SPEquipment> equipmentSyncInfoList,Guid colledgeId, out int totalCount, out int successCount, out int failCount)
        {
            string errorMessage = "";
            totalCount = 0;
            successCount = 0;
            failCount = 0;
            try
            {
                XTransaction tran = null;
                if (equipmentSyncInfoList == null || equipmentSyncInfoList.Count() == 0) throw new ArgumentException("设备信息为空");
                var equipmentIds = equipmentSyncInfoList.Select(p => p.EquipmentId);
                var collegeEquipmentList = GetEntitiesByExpression(string.Format("CollegeId=\"{0}\"", colledgeId));
                if (collegeEquipmentList != null && collegeEquipmentList.Count() > 0)
                {
                    foreach (var collegeEquipment in collegeEquipmentList)
                    {
                        var willDelEquipmentList = collegeEquipmentList.Where(p => !equipmentIds.Contains(collegeEquipment.EquipmentId));
                        if (willDelEquipmentList != null && willDelEquipmentList.Count() > 0)
                        {
                            foreach (var willDelEquipment in willDelEquipmentList) willDelEquipment.IsDelete = true;
                            Save(willDelEquipmentList, ref tran);
                        }
                    }
                }
                IList<SPDataSyncLog> dataSyncLogs =new List<SPDataSyncLog>();
                foreach (var equipmentSyncInfo in equipmentSyncInfoList)
                {
                    var result = Sync(equipmentSyncInfo, colledgeId, out errorMessage);
                    dataSyncLogs.Add(new SPDataSyncLog(result, equipmentSyncInfo.EquipmentId, equipmentSyncInfo.Name, errorMessage));
                    if (result) successCount++;
                    if (!result) failCount++;
                }
                totalCount = successCount + failCount;
                return dataSyncLogs;
            }
            catch (Exception ex)
            {
                return new List<SPDataSyncLog>() { new SPDataSyncLog(false, null, null, "执行出错,错误消息:" + ex.Message) };
            }
        }
        public bool Sync(SPEquipment equipmentSyncInfo,Guid colledgeId, out string errorMessage)
        {
            errorMessage = "";
            var tran = SessionManager.Instance.BeginTransaction();
            try
            {
                bool isNew = false;
                if (equipmentSyncInfo == null) throw new ArgumentException("设备信息为空");
                if (!equipmentSyncInfo.Validate(out errorMessage)) throw new Exception(errorMessage);
                IDictionary<Guid, Guid?> ddOrgRoomId = null;
                if (!string.IsNullOrWhiteSpace(equipmentSyncInfo.OrgName) && !string.IsNullOrWhiteSpace(equipmentSyncInfo.RoomName))
                {
                    ddOrgRoomId = __labOrganizationBLL.GenerateOrgAndRoomByName(colledgeId,equipmentSyncInfo.OrgName,"", ref tran);
                }
                var schoolEquipment = GetFirstOrDefaultEntityByExpression(string.Format("EquipmentId=\"{0}\"", equipmentSyncInfo.EquipmentId.Trim()));
                if (schoolEquipment == null) 
                {
                    isNew = true;
                    schoolEquipment = new SchoolEquipment() { Id = Guid.NewGuid() };
                }
                schoolEquipment.EquipmentId = equipmentSyncInfo.EquipmentId;
                schoolEquipment.Name = equipmentSyncInfo.Name;
                schoolEquipment.Model = equipmentSyncInfo.Model;
                schoolEquipment.Remark = equipmentSyncInfo.Remark;
                schoolEquipment.ImgURL = equipmentSyncInfo.ImgURL;
                schoolEquipment.ShowURL = equipmentSyncInfo.ShowURL;
                schoolEquipment.BookURL = equipmentSyncInfo.BookURL;
                schoolEquipment.IsDelete = equipmentSyncInfo.IsDelete;
                schoolEquipment.ContactPerson = equipmentSyncInfo.ContactPerson;
                schoolEquipment.ContalTelNo = equipmentSyncInfo.ContalTelNo;
                schoolEquipment.OrgName = equipmentSyncInfo.OrgName;
                schoolEquipment.RoomName = equipmentSyncInfo.RoomName;
                schoolEquipment.ScopeOfApplication = equipmentSyncInfo.ScopeOfApplication;
                if (ddOrgRoomId != null && ddOrgRoomId.Keys.Count > 0)
                {
                    schoolEquipment.OrgId = ddOrgRoomId.Keys.First();
                    schoolEquipment.RoomId = ddOrgRoomId.Values.First();
                }
                schoolEquipment.CollegeId = colledgeId;
                if (isNew) Add(new SchoolEquipment[] { schoolEquipment }, ref tran, true);
                else Save(new SchoolEquipment[] { schoolEquipment }, ref tran, true);
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex) 
            { 
                if(tran.HasTransaction) tran.RollbackTransaction();
                errorMessage = ex.Message;
                return false;
            }
            finally{ tran.Dispose();}
        }
    }
}
