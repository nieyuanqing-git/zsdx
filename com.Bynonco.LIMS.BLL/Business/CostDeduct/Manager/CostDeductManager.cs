using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.august.DataLink;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL
{
    /// <summary>
    /// 成本扣费管理者
    /// </summary>
    public class CostDeductManager:ICostDeductManager
    {
        /// <summary>设备预约预扣费</summary>
        /// <param name="appointment">设备预约</param>
        /// <param name="viewOpenFundDetails"></param>
        /// <param name="userAccount">用户账号</param>
        /// <param name="tran">事务</param>
        /// <param name="operatorId">操作员ID</param>
        /// <param name="operatorIP">操作员客户端IP</param>
        /// <param name="isDeductSubject">是否扣费课题组</param>
        /// <param name="costDeductEquipmentOpenFundd">成本扣费设备开放函数</param>
        /// <param name="isSaveEquipmentOpenFun">是否保存设备开放函数</param>
        /// <returns></returns>
        public CostDeduct AppointmentPredictDeduct(Appointment appointment, IList<ViewOpenFundDetail> viewOpenFundDetails, UserAccount userAccount, ref XTransaction tran, Guid? operatorId, string operatorIP, bool isDeductSubject = true, IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFundd = null, bool isSaveEquipmentOpenFun = true)
        {
            var results = new AppointmentPredictCostDeduct(new object[] { appointment, userAccount, isDeductSubject, costDeductEquipmentOpenFundd, isSaveEquipmentOpenFun, viewOpenFundDetails },operatorId,operatorIP).Deduct(ref tran);
            return results == null || results.Length == 0 ? null : results[0] as CostDeduct;
        }
        /// <summary> 取消预约预扣费 </summary>
        /// <param name="appointment"></param>
        /// <param name="tran"></param>
        /// <param name="operatorId"></param>
        /// <param name="operatorIP"></param>
        /// <param name="isDeductSubject"></param>
        /// <param name="isSaveEquipmentOpenFun"></param>
        /// <returns></returns>
        public UserAccount CancelAppointmentPredictDeduct(Model.Appointment appointment, ref XTransaction tran, Guid? operatorId, string operatorIP, bool isDeductSubject = true, bool isSaveEquipmentOpenFun = true)
        {
            var results = new AppointmentPredictCostDeduct(new object[] { appointment, null, isDeductSubject, null, isSaveEquipmentOpenFun }, operatorId, operatorIP).CancelDeduct(ref tran);
            return results == null || results.Length == 0 ? null : results[0] as UserAccount;
        }

        public CostDeduct AppointmentAuditDeduct(Appointment appointment, UserAccount userAccount, ref XTransaction tran, Guid? operatorId, string operatorIP)
        {
            var results = new AppointmentAuditCostDeduct(new object[] { appointment, userAccount }, operatorId, operatorIP).Deduct(ref tran);
            return results == null || results.Length == 0 ? null : results[0] as CostDeduct;
        }

        public UserAccount CancelAppointmentAuditDeduct(Appointment appointment, ref XTransaction tran, Guid? operatorId, string operatorIP)
        {
            var results = new AppointmentAuditCostDeduct(new object[] { appointment, null }, operatorId, operatorIP).CancelDeduct(ref tran);
            return results == null || results.Length == 0 ? null : results[0] as UserAccount;
        }

        /// <summary> 预约扣费 </summary>
        /// <param name="appointment"></param>
        /// <param name="startAt"></param>
        /// <param name="endAt"></param>
        /// <param name="usedConfirmFeedBack"></param>
        /// <param name="tran"></param>
        /// <param name="operatorId"></param>
        /// <param name="operatorIP"></param>
        /// <returns></returns>
        public UsedConfirm AppointmenDeduct(Model.Appointment appointment, DateTime startAt, DateTime endAt, UsedConfirmFeedBack usedConfirmFeedBack, ref XTransaction tran, Guid? operatorId, string operatorIP)
        {
            var results = new AppointmentCostDeduct(new object[] { appointment, startAt, endAt, usedConfirmFeedBack }, operatorId, operatorIP).Deduct(ref tran);
            return results == null || results.Length == 0 ? null : results[0] as UsedConfirm;
        }
        /// <summary> 新使用记录 </summary>
        /// <param name="recordId"></param>
        /// <param name="userLabel"></param>
        /// <param name="equipmentLabel"></param>
        /// <param name="startAt"></param>
        /// <param name="endAt"></param>
        /// <param name="isDeduct"></param>
        /// <param name="subjectInfo"></param>
        /// <param name="remarkInfo"></param>
        /// <param name="usedConfirmFeedBack"></param>
        /// <param name="errorMsg"></param>
        /// <param name="tran"></param>
        /// <param name="operatorId"></param>
        /// <param name="operatorIP"></param>
        /// <returns></returns>
        public UsedConfirm NewUsedRecordDeduct(string recordId, string userLabel, string equipmentLabel, DateTime startAt, DateTime endAt, bool isDeduct, string subjectInfo, string remarkInfo, UsedConfirmFeedBack usedConfirmFeedBack, out string errorMsg, ref XTransaction tran, Guid? operatorId, string operatorIP)
        {
            errorMsg = "";
            try
            {
                var results = new NewUsedRecordCostDeduct(new object[] { recordId, userLabel, equipmentLabel, startAt, endAt, isDeduct, subjectInfo, remarkInfo, usedConfirmFeedBack }, operatorId, operatorIP).Deduct(ref tran);
                return results[0] as UsedConfirm;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return null;
            }
        }

        public UsedConfirm UsedConfirmCostDeduct(UsedConfirm usedConfirm, Guid operatorId, bool isAllowAccountMinuse, bool isSuperAmin, ref XTransaction tran, string operatorIP)
        {
            var results = new UsedConfirmCostDeduct(new object[] { usedConfirm, operatorId, isAllowAccountMinuse, isSuperAmin }, operatorId, operatorIP).Deduct(ref tran);
            return results == null || results.Length == 0 ? null : results[0] as UsedConfirm;
        }

        public UsedConfirm CancelUsedConfirmCostDeduct(UsedConfirm usedConfirm, ref XTransaction tran, Guid? operatorId, string operatorIP)
        {
            var results = new UsedConfirmCostDeduct(new object[] { usedConfirm }, operatorId, operatorIP).CancelDeduct(ref tran);
            return results == null || results.Length == 0 ? null : results[0] as UsedConfirm;
        }

        public ManualCostDeduct ManualCostDeduct(ManualCostDeduct manualCostDeduct, Guid operatorId, bool isAllowAccountMinuse, bool isSuperAmin, ref XTransaction tran, string operatorIP)
        {
            var results = new ManualedCostDeduct(new object[] { manualCostDeduct, operatorId, isAllowAccountMinuse, isSuperAmin }, operatorId, operatorIP).Deduct(ref tran);
            return results == null || results.Length == 0 ? null : results[0] as ManualCostDeduct;
        }

        public void CancleManualCostDeduct(ManualCostDeduct manualCostDeduct, ref XTransaction tran, Guid? operatorId, string operatorIP)
        {
            new ManualedCostDeduct(new object[] { manualCostDeduct }, operatorId, operatorIP).CancelDeduct(ref tran);
        }

        public AnimalCostDeduct AnimalCostDeduct(Animal animal, ref XTransaction tran,DateTime endCostDeductDate, Guid? operatorId, string operatorIP)
        {
            var results = new AnimalDayCostDeduct(new object[] { animal, operatorId }, endCostDeductDate,operatorId, operatorIP).Deduct(ref tran);
            return results == null || results.Length == 0 ? null : results[0] as AnimalCostDeduct;
        }

        public void CancleAnimalCostDeduct(Animal animal, AnimalCostDeduct animalCostDeduct, ref XTransaction tran, Guid? operatorId, string operatorIP)
        {
            new AnimalDayCostDeduct(new object[] { animal, operatorId, animalCostDeduct }, DateTime.Now.Date.AddSeconds(-1), operatorId, operatorIP).CancelDeduct(ref tran);
        }

        public MaterialOutput MaterialCostDeduct(MaterialOutput materialOutput, Guid operatorId, bool isAllowAccountMinuse, bool isSuperAmin, ref XTransaction tran,string operatorIP)
        {
            var results = new MaterialCostDeduct(new object[] { materialOutput, operatorId, isAllowAccountMinuse, isSuperAmin }, operatorId, operatorIP).Deduct(ref tran);
            return results == null || results.Length == 0 ? null : results[0] as MaterialOutput;
        }
        
        public void CancelMaterialCostDeduct(MaterialOutput materialOutput, ref XTransaction tran, Guid? operatorId, string operatorIP)
        {
            new MaterialCostDeduct(new object[] { materialOutput }, operatorId, operatorIP).CancelDeduct(ref tran);
        }

        public WaterControlCostDeduct WaterControlCostDeduct(WaterControlCostDeduct waterControlCostDeduct, Guid operatorId, bool isAllowAccountMinuse, bool isSuperAmin, ref XTransaction tran, string operatorIP)
        {
            var results = new WaterControledCostDeduct(new object[] { waterControlCostDeduct, operatorId, isAllowAccountMinuse, isSuperAmin }, operatorId, operatorIP).Deduct(ref tran);
            return results == null || results.Length == 0 ? null : results[0] as WaterControlCostDeduct;
        }

        public void CancleWaterControlCostDeduct(WaterControlCostDeduct waterControlCostDeduct, ref XTransaction tran, Guid? operatorId, string operatorIP)
        {
            new WaterControledCostDeduct(new object[] { waterControlCostDeduct }, operatorId, operatorIP).CancelDeduct(ref tran);
        }

        public NMPUsedConfirm NMPUsedConfirmCostDeduct(NMPUsedConfirm nmpUsedConfirm, Guid operatorId, bool isAllowAccountMinuse, bool isSuperAmin, ref XTransaction tran, string operatorIP)
        {
            var results = new NMPUsedConfirmCostDeduct(new object[] { nmpUsedConfirm, operatorId, isAllowAccountMinuse, isSuperAmin }, operatorId, operatorIP).Deduct(ref tran);
            return results == null || results.Length == 0 ? null : results[0] as NMPUsedConfirm;
        }

        public NMPUsedConfirm CancelNMPUsedConfirmCostDeduct(NMPUsedConfirm nmpUsedConfirm, ref XTransaction tran, Guid? operatorId, string operatorIP)
        {
            var results = new NMPUsedConfirmCostDeduct(new object[] { nmpUsedConfirm }, operatorId, operatorIP).CancelDeduct(ref tran);
            return results == null || results.Length == 0 ? null : results[0] as NMPUsedConfirm;
        }

        public NMPUsedConfirm NMPAppointmenDeduct(Model.NMPAppointment nmpAppointment, DateTime startAt, DateTime endAt, NMPUsedConfirmFeedBack nmpUsedConfirmFeedBack, ref XTransaction tran, Guid? operatorId, string operatorIP)
        {
            var results = new NMPAppointmentCostDeduct(new object[] { nmpAppointment, startAt, endAt, nmpUsedConfirmFeedBack }, operatorId, operatorIP).Deduct(ref tran);
            return results == null || results.Length == 0 ? null : results[0] as NMPUsedConfirm;
        }

        public NMPUsedConfirm NMPNewUsedRecordDeduct(string recordId, string userLabel, string equipmentLabel, DateTime startAt, DateTime endAt, bool isDeduct, string subjectInfo, string remarkInfo, NMPUsedConfirmFeedBack nmpUsedConfirmFeedBack, out string errorMsg, ref XTransaction tran, Guid? operatorId, string operatorIP)
        {
            errorMsg = "";
            try
            {
                var results = new NMPNewUsedRecordCostDeduct(new object[] { recordId, userLabel, equipmentLabel, startAt, endAt, isDeduct, subjectInfo, remarkInfo, nmpUsedConfirmFeedBack }, operatorId, operatorIP).Deduct(ref tran);
                return results[0] as NMPUsedConfirm;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return null;
            }
        }

    }
}
