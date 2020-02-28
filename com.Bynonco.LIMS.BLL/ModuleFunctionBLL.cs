using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.IBLL;
using com.august.DataLink;

namespace com.Bynonco.LIMS.BLL
{
    public class ModuleFunctionBLL : BLLBase<ModuleFunction>, IModuleFunctionBLL
    {
        public bool ChangeParentModuleFunction(Guid id, Guid? parentId, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var moduleFunction = GetEntityById(id);
                if (moduleFunction == null) throw new ArgumentException(string.Format("出错,找不到编码为【{0}】的模块信息", id));
                if (parentId.HasValue)
                {
                    var parentItem = GetEntityById(parentId.Value);
                    if (parentItem == null) throw new ArgumentException(string.Format("出错,找不到编码为【{0}】的父级模块信息", id));
                    if (parentItem.XPath.StartsWith(moduleFunction.XPath))
                        throw new ArgumentException(string.Format("出错,无法放置在子模块"));
                }
                IList<ModuleFunction> moduleFunctions = null;
                var impactModuleFunctions = GetEntitiesByExpression(string.Format("XPath→{0}*IsDelete=false", moduleFunction.XPath), null, "", true);
                moduleFunctions = GetEntitiesByExpression(string.Format("IsDelete=false*{0}", impactModuleFunctions.Select(p => p.Id).ToFormatStr("Id", LogicRelation.AndNot)), null, "", true);
                moduleFunctions = moduleFunctions.Where(p => !p.XPath.StartsWith(moduleFunction.XPath) && double.Parse(p.XPath) > double.Parse(moduleFunction.XPath) && p.XPath.Length >= moduleFunction.XPath.Length).ToList();
                var entities = XPathUtility.AdjustXPath(moduleFunction.XPath, moduleFunctions);
                var originalXPath = moduleFunction.XPath;
                var newXPath = "";
                var newParentItem = entities.Where(p => parentId.HasValue && p.Id == parentId.Value).FirstOrDefault();
                if (newParentItem != null)
                {
                    var currentChildItemList = entities.Where(p => p.ParentId == newParentItem.Id);
                    var currentChildCount = 0;
                    if (currentChildItemList != null && currentChildItemList.Count() > 0)
                    {
                        currentChildCount = currentChildItemList.Count();
                    }
                    newXPath = newParentItem.XPath + currentChildCount.ToString().PadLeft(3, '0');
                }
                else
                {
                    newXPath = XPathUtility.GenerateXPath(null, parentId,
                       (entityId) => { return GetEntitiesByExpression(string.Format("Id=\"{0}\"*IsDelete=false*{1}", entityId.ToString(), impactModuleFunctions.Select(p => p.Id).ToFormatStr("Id", LogicRelation.AndNot))).First(); },
                       (parentEntityId) => { return GetEntitiesByExpression(string.Format("ParentId=\"{0}\"*IsDelete=false*{1}", parentEntityId.ToString(), impactModuleFunctions.Select(p => p.Id).ToFormatStr("Id", LogicRelation.AndNot))); },
                       () => { return GetEntitiesByExpression(string.Format("ParentId=null*IsDelete=false*{0}", impactModuleFunctions.Select(p => p.Id).ToFormatStr("Id", LogicRelation.AndNot))); });
                }
                foreach (var item in impactModuleFunctions)
                {
                    if (item.Id == moduleFunction.Id) item.ParentId = parentId;
                    item.XPath = newXPath + (item.XPath.Length > originalXPath.Length ? item.XPath.Substring(originalXPath.Length) : "");
                    item.XPathLength = item.XPath.Length;
                    entities.Add(item);
                }
                Save(entities.ToArray(), ref tran, true);
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
        }
    }
}