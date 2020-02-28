using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Base;
using com.august.DataLink;

namespace com.Bynonco.LIMS.BLL
{
    public class NMPEquipmentAuthorizationBLL : BLLBase<NMPEquipmentAuthorization>, INMPEquipmentAuthorizationBLL
    {
        public bool SaveNMPEquipmentAuthorization(IList<NMPEquipmentAuthorization> nmpEquipmentAuthorizationList, string ip, out string errorMessage)
        {
            errorMessage = "";
            if (nmpEquipmentAuthorizationList == null || nmpEquipmentAuthorizationList.Count() == 0)
            {
                errorMessage = "出错, 操作记录为空.";
                return false;
            }
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {

                foreach (var item in nmpEquipmentAuthorizationList)
                {
                    var originalEntity = GetEntityById(item.Id);
                    if (originalEntity == null)
                    {
                        Add(new NMPEquipmentAuthorization[] { item }, ref tran, true);
                        //GenerateEntityLog(OperateType.New, originalEntity, item, ip, ref tran, true);
                    }
                    else
                    {
                        Save(new NMPEquipmentAuthorization[] { item }, ref tran, true);
                        //GenerateEntityLog(OperateType.Modified, originalEntity, item, ip, ref tran, true);
                    }                    
                }
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString().StartsWith("出错,") ? ex.ToString() : string.Format("出错,{0}", ex.ToString());
                tran.RollbackTransaction();
                return false;
            }
            finally { tran.Dispose(); }
            return true;
        }
    }
}
