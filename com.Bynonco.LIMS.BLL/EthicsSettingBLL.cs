using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;
using com.august.DataLink;

namespace com.Bynonco.LIMS.BLL
{
    public class EthicsSettingBLL : IEthicsSettingBLL
    {
        private IDictCodeTypeBLL __dictCodeTypeBLL;
        private IDictCodeBLL __dictCodeBLL;
        private ITrainningTypeBLL __trainningTypeBLL;
        public EthicsSettingBLL()
        {
            __dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
            __trainningTypeBLL = BLLFactory.CreateTrainningTypeBLL();
        }
        public Model.Business.EthicsSetting GetEthicsSetting()
        {
            bool isNeedTrainningBeforeApply = false;
            IList<TrainningType> needTrainningTypes = null;
            Dictionary<string, string> showLinksWhenApplyPass = null;
            var isNeedTrainningBeforeApplyDictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("IsStop=false*IsDelete=false*Code=IsNeedTrainningBeforeEthicsApply");
            if (isNeedTrainningBeforeApplyDictCodeType != null && !string.IsNullOrWhiteSpace(isNeedTrainningBeforeApplyDictCodeType.Value))
            {
                isNeedTrainningBeforeApply = isNeedTrainningBeforeApplyDictCodeType.Value.Trim() == "1";
            }
            var needTrainningTypeDictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("IsStop=false*IsDelete=false*Code=NeedTrainningTypesBeforeEthicsApply");
            if (needTrainningTypeDictCodeType != null && needTrainningTypeDictCodeType.DictCodes != null && needTrainningTypeDictCodeType.DictCodes.Count > 0)
            {
                var trainningTypeIds = needTrainningTypeDictCodeType.DictCodes.Where(p => !string.IsNullOrWhiteSpace(p.Value) && p.Value.Trim().IsGuid()).Select(p => new Guid(p.Value.Trim()));
                if (trainningTypeIds != null && trainningTypeIds.Count() > 0)
                {
                    needTrainningTypes = __trainningTypeBLL.GetEntitiesByExpression(trainningTypeIds.ToFormatStr() + "*IsDelete=false*IsStop=false");
                }
            }
            var showLinksWhenApplyPassDictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("IsStop=false*IsDelete=false*Code=ShowLinksWhenEthicsApplyPass");
            if (showLinksWhenApplyPassDictCodeType != null && showLinksWhenApplyPassDictCodeType.DictCodes != null && showLinksWhenApplyPassDictCodeType.DictCodes.Count > 0)
            {
                showLinksWhenApplyPass = new Dictionary<string, string>();
                foreach (var showLinkDictCode in showLinksWhenApplyPassDictCodeType.DictCodes)
                    showLinksWhenApplyPass[showLinkDictCode.Name] = showLinkDictCode.Value;

            }
            EthicsSetting ethicsSetting = new EthicsSetting(isNeedTrainningBeforeApply, needTrainningTypes, showLinksWhenApplyPass);
            return ethicsSetting;
        }

        public bool SaveEthicsSetting(Model.Business.EthicsSetting ethicsSetting, out string errorMessage)
        {
            
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                tran = SessionManager.Instance.BeginTransaction();
                var ethicsSettingDictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=EthicsSetting*IsDelete=false");
                if (ethicsSettingDictCodeType == null)
                {
                    var animalManageDictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=AnimalManage*IsDelete=false");
                    if (animalManageDictCodeType == null)
                    {
                        animalManageDictCodeType = new DictCodeType()
                        {
                            Id = Guid.NewGuid(),
                            Code = "AnimalManage",
                            Name = "动物管理",
                            Value = "动物管理",
                            IsStop = false,
                            IsDelete = false,
                            ParentId = null
                        };
                        __dictCodeTypeBLL.Add(new DictCodeType[] { animalManageDictCodeType }, ref tran, true);
                    }
                    ethicsSettingDictCodeType = new DictCodeType()
                    {
                        Id = Guid.NewGuid(),
                        Code = "EthicsSetting",
                        Name = "伦理申请设置",
                        Value = "伦理申请设置",
                        IsStop = false,
                        IsDelete = false,
                        ParentId = animalManageDictCodeType.Id
                    };
                    __dictCodeTypeBLL.Add(new DictCodeType[] { ethicsSettingDictCodeType }, ref tran, true);
                }
                var isNeedTrainningBeforeApplyDictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("IsStop=false*IsDelete=false*Code=IsNeedTrainningBeforeEthicsApply");
                if (isNeedTrainningBeforeApplyDictCodeType != null)
                {
                    if (isNeedTrainningBeforeApplyDictCodeType.DictCodes != null && isNeedTrainningBeforeApplyDictCodeType.DictCodes.Count > 0)
                    {
                        __dictCodeBLL.Delete(isNeedTrainningBeforeApplyDictCodeType.DictCodes.Select(p => p.Id), ref tran, true);
                    }
                    __dictCodeTypeBLL.Delete(new Guid[] { isNeedTrainningBeforeApplyDictCodeType.Id }, ref tran, true);
                }
                var isNeedTrainningBeforeApplyNewDictCodeType = new DictCodeType()
                {
                    Id = Guid.NewGuid(),
                    Code = "IsNeedTrainningBeforeEthicsApply",
                    Name = "是否需要通过培训",
                    Value = ethicsSetting.IsNeedTrainningBeforeApply ? "1" : "0",
                    IsStop = false,
                    IsDelete = false,
                    ParentId = ethicsSettingDictCodeType.Id
                };
                __dictCodeTypeBLL.Add(new DictCodeType[] { isNeedTrainningBeforeApplyNewDictCodeType }, ref tran, true);
                var needTrainningTypeDictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("IsStop=false*IsDelete=false*Code=NeedTrainningTypesBeforeEthicsApply");
                if (needTrainningTypeDictCodeType != null)
                {
                    if (needTrainningTypeDictCodeType.DictCodes != null && needTrainningTypeDictCodeType.DictCodes.Count > 0)
                    {
                        __dictCodeBLL.Delete(needTrainningTypeDictCodeType.DictCodes.Select(p => p.Id), ref tran, true);
                    }
                    __dictCodeTypeBLL.Delete(new Guid[] { needTrainningTypeDictCodeType.Id }, ref tran, true);
                }
                var needTrainningTypeNewDictCodeType = new DictCodeType()
                {
                    Id = Guid.NewGuid(),
                    Code = "NeedTrainningTypesBeforeEthicsApply",
                    Name = "需要考试通过的类型",
                    Value = "需要考试通过的类型",
                    IsStop = false,
                    IsDelete = false,
                    ParentId = ethicsSettingDictCodeType.Id
                };
                __dictCodeTypeBLL.Add(new DictCodeType[] { needTrainningTypeNewDictCodeType }, ref tran, true);
                if (ethicsSetting.NeedPassTrainningTypes != null && ethicsSetting.NeedPassTrainningTypes.Count > 0)
                {
                    foreach (var needPassTrainningType in ethicsSetting.NeedPassTrainningTypes)
                    {
                        var needPassTrainningTypeDictCode = new DictCode()
                        {
                            Id=  Guid.NewGuid(),
                            IsDelete = false,
                            IsStop = false,
                            Name = needPassTrainningType.Name,
                            Code = needPassTrainningType.Name,
                            Value = needPassTrainningType.Id.ToString(),
                            DictCodeTypeId = needTrainningTypeNewDictCodeType.Id

                        };
                        __dictCodeBLL.Add(new DictCode[] { needPassTrainningTypeDictCode }, ref tran, true);
                    }
                }
                var showLinksWhenApplyPassDictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("IsStop=false*IsDelete=false*Code=ShowLinksWhenEthicsApplyPass");
                if (showLinksWhenApplyPassDictCodeType != null)
                {
                    if (showLinksWhenApplyPassDictCodeType.DictCodes != null && showLinksWhenApplyPassDictCodeType.DictCodes.Count > 0)
                    {
                        __dictCodeBLL.Delete(showLinksWhenApplyPassDictCodeType.DictCodes.Select(p => p.Id), ref tran, true);
                    }
                    __dictCodeTypeBLL.Delete(new Guid[] { showLinksWhenApplyPassDictCodeType.Id }, ref tran, true);

                }
                var showLinksWhenApplyPassNewDictCodeType = new DictCodeType()
                {
                    Id = Guid.NewGuid(),
                    Code = "ShowLinksWhenEthicsApplyPass",
                    Name = "审核通过显示的链接",
                    Value = "审核通过显示的链接",
                    IsStop = false,
                    IsDelete = false,
                    ParentId = ethicsSettingDictCodeType.Id
                };
                __dictCodeTypeBLL.Add(new DictCodeType[] { showLinksWhenApplyPassNewDictCodeType }, ref tran, true);
                if (ethicsSetting.ShowLinksWhenApplyPass != null && ethicsSetting.ShowLinksWhenApplyPass.Count > 0)
                {
                    foreach (var showLinksWhenApplyPass in ethicsSetting.ShowLinksWhenApplyPass)
                    {
                        var showLinksWhenApplyPassDictCode = new DictCode()
                        {
                            Id = Guid.NewGuid(),
                            IsDelete = false,
                            IsStop = false,
                            Name = showLinksWhenApplyPass.Key,
                            Code = showLinksWhenApplyPass.Key,
                            Value = showLinksWhenApplyPass.Value,
                            DictCodeTypeId = showLinksWhenApplyPassNewDictCodeType.Id

                        };
                        __dictCodeBLL.Add(new DictCode[] { showLinksWhenApplyPassDictCode }, ref tran, true);
                    }
                }
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (tran != null && tran.HasTransaction) tran.RollbackTransaction();
                return false;
            }
            finally { if (tran != null) tran.Dispose(); }
        }
    }
}
