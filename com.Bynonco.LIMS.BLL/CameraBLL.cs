using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL
{
    public class CameraBLL : BLLBase<Camera>, ICameraBLL
    {
        private ICameraRelationBLL __cameraRelationBLL;
        public CameraBLL()
        {
            __cameraRelationBLL = BLLFactory.CreateCameraRelationBLL();
        }
        public bool DeleteCamera(Guid id, out string errorMessage)
        {
            return DeleteCameraList(new List<Guid>() { id }, out errorMessage);
        }
        public bool DeleteCameraList(IList<Guid> ids, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                if (ids == null || ids.Count() == 0) throw new ArgumentException("出错,待删除的摄像头为空");
                tran = SessionManager.Instance.BeginTransaction();
                foreach (var id in ids)
                {
                    var camera = GetEntityById(id);
                    if (camera == null) throw new ArgumentException(string.Format("出错,找不出编码为【{0}】的摄像头信息", id));
                    var cameraRelationList = __cameraRelationBLL.GetEntitiesByExpression(string.Format("CameraId=\"{0}\"", camera.Id));
                    if (cameraRelationList != null && cameraRelationList.Count() > 0) 
                        __cameraRelationBLL.Delete(cameraRelationList.Select(m => m.id), ref tran, true);
                    Delete(new Guid[] { camera.Id }, ref tran, true);
                }
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
            return true;
        }
    }
}