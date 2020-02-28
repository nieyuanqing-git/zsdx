using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;

namespace com.Bynonco.LIMS.BLL
{
    public class PatentImportBLL : BLLBase<PatentImport>, IPatentImportBLL
    {
        public IList<PatentImport> ImportPatent(IList<PatentImport> patentImportList, bool isDeal = true)
        {
            if (patentImportList == null || patentImportList.Count == 0)
                return null;
            var patentBLL = BLLFactory.CreatePatentBLL();
            var patentUserBLL = BLLFactory.CreatePatentUserBLL();
            var patentEquipmentBLL = BLLFactory.CreatePatentEquipmentBLL();
            var dictCode = BLLFactory.CreateDictCodeBLL();
            var equipmentBLL = BLLFactory.CreateEquipmentBLL();
            var userBLL = BLLFactory.CreateUserBLL();
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            foreach (var item in patentImportList)
            {

                try
                {
                    item.IsHandled = false;
                    Patent patent = new Patent();
                    
                    PatentEquipment patentEquipment = new PatentEquipment();

                    patent.Id = Guid.NewGuid();
                    if (item.EquipmentLable != "")
                    {
                        var equipment = equipmentBLL.GetFirstOrDefaultEntityByExpression(string.Format("Label=\"{0}\"", item.EquipmentLable));
                        if (equipment == null) continue;
                        else
                        {
                            patentEquipment.Id = Guid.NewGuid();
                            patentEquipment.EquipmentId = equipment.id;
                            patentEquipment.PatentId = patent.Id;
                            patentEquipment.EquipmentOrder = 1;
                        }
                    }
                    else continue;
                    IList<PatentUser> patentUserList = new List<PatentUser>();
                    if (item.PatentUser == null) continue;
                    else
                    {
                        var memberNameList = item.PatentUser.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < memberNameList.Count(); i++)
                        {
                            PatentUser patentUser = new PatentUser();
                            string itemName = memberNameList[i].Trim();
                            string itemId = "";
                            var userinfo = userBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserName=\"{0}\"", itemName));
                            if (userinfo == null) itemId = "";
                            else itemId = userinfo.id.ToString();
                            patentUser.Id = Guid.NewGuid();
                            patentUser.PatentId = patent.id;
                            if (itemId != "") patentUser.UserId = new Guid(itemId);
                            patentUser.UserName = itemName;
                            patentUser.UserOrder = i + 1;
                            patentUserList.Add(patentUser);
                        }
                    }
                    patent.PatentUserList = patentUserList;
                    patent.PatentName = item.PatentName;
                    patent.PatentNum = item.PatentNum;
                    patent.PatentType = new Guid(item.PatentType);
                    DateTime dtDate;
                    if (DateTime.TryParse(item.PatentTime, out dtDate))
                        patent.PatentTime = dtDate;
                    else
                        continue;
                    patent.PatentBelongTo = int.Parse(item.PatentBelongTo);
                    patent.UpdateTime = DateTime.Now;

                    patentBLL.Add(new Patent[] { patent }, ref tran, true);
                    patentEquipmentBLL.Add(new PatentEquipment[] { patentEquipment }, ref tran, true);
                    patentUserBLL.Add(patent.PatentUserList, ref tran, true);
                    tran.CommitTransaction();
                    item.IsHandled = true;
                }
                catch (Exception ex)
                {
                    patentImportList.Remove(item);
                    if (tran != null && tran.HasTransaction) tran.RollbackTransaction();
                }
                finally
                {
                    if (tran != null) tran.Dispose();
                }
            }
            return patentImportList;            
        }
       
    }
}