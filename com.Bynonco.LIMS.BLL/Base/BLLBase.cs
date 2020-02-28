using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.august.DataLink;
using com.Bynonco.LIMS.DAL;
using com.august.Core.XQuery.NHibernate;
using com.Bynonco.LIMS.IBLL;
using log4net;
using com.Bynonco.LIMS.Model;
using NHibernate;
using NHibernate.Criterion;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.Logic.Model;

namespace com.Bynonco.LIMS.BLL.Base
{
    /// 业务逻辑基类
    /// <summary>
    /// 业务逻辑基类
    /// </summary>
    /// <typeparam name="T">泛型类型，必须基础数据对象Guid与延迟加载接口</typeparam>
    public abstract class BLLBase<T> : IBLLBase<T> where T : global::com.august.DataLink.DataObject<Guid>,ILazyLoad
    {
        /// log4net日志接口
        /// <summary>
        /// log4net日志接口
        /// </summary>
        protected static readonly ILog log = LogManager.GetLogger(typeof(T));
        /// 数据库连接串名称
        /// <summary>
        /// 数据库连接串名称
        /// </summary>
        protected string connectionName = "";
        /// 存储过程执行对象
        /// <summary>
        /// 存储过程执行对象
        /// </summary>
        private ProcedureAdapter procedureAdapter;
        public BLLBase()
        {
            procedureAdapter = new ProcedureAdapter(Configuration.DefaultConnectionString.Name);
        }
        public BLLBase(string connectionName)
        {
            this.connectionName = connectionName;
            procedureAdapter = new ProcedureAdapter(connectionName);
        }

        /// 数据库连接串名称
        /// <summary>
        /// 数据库连接串名称
        /// </summary>
        public virtual string ConnectionName
        {
            set { this.connectionName = value; }
        }

        /// 获取存储过程执行对象
        /// <summary>
        /// 获取存储过程执行对象
        /// </summary>
        public ProcedureAdapter ProcedureAdapter
        {
            get { return procedureAdapter; }
        }
        /// 检查ID对应实体是否存在
        /// <summary>
        /// 检查ID对应实体是否存在
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="errorMessage">错误消息</param>
        /// <returns></returns>
        public virtual bool CheckId(Guid id, out string errorMessage)
        {
            errorMessage = "";
            var T = GetEntityById(id);
            if (T == null)
            {
                var name = typeof(T).ToString();
                if (name.IndexOf('.') != -1 && !name.EndsWith(".")) name = name.Substring(name.LastIndexOf('.') + 1);
                errorMessage = string.Format("出错,找不到编码为【{0}】的{1}信息", id, name);
            }
            return true;
        }

        /// 持久化一个类型为T的实体对象到数据库
        /// <summary>
        /// 持久化一个类型为T的实体对象到数据库
        /// </summary>
        /// <param name="entities">T类型实体对象实例</param>
        /// <param name="tran">事务</param>
        /// <param name="isSupress">是否挂起事务，如果挂起，则不提交事务；如果不挂起，则提交事务，默认不挂起</param>
        /// <returns>成功返回真否则为假</returns>
        public virtual bool Add(IEnumerable<T> entities, ref XTransaction tran, bool isSupress = false)
        {
            if (tran == null) tran = string.IsNullOrEmpty(connectionName) ? SessionManager.Instance.BeginTransaction() : SessionManager.Instance.BeginTransaction(connectionName);
            var adp = DataAdpterFactory.CreateAdp<T, Guid>(ref tran);
            try
            {
                foreach (var entity in entities)
                {
                    entity.New();
                    adp.AddModel(entity, true);
                }
                if (!isSupress) tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("{0}:{1}", "Add", ex.Message);
                if (!isSupress) tran.RollbackTransaction();
                throw new Exception(ex.Message);
            }
            finally { adp.Dispose(); if (!isSupress)tran.Dispose(); }

        }

        ///更新类型为T的实体对象到数据库
        /// <summary>
        ///更新类型为T的实体对象到数据库
        /// </summary>
        /// <param name="entities">T类型实体对象实例</param>
        /// <param name="tran">事务</param>
        /// <param name="isSupress">是否挂起事务，如果挂起，则不提交事务；如果不挂起，则提交事务，默认不挂起</param>
        /// <returns>成功返回真否则为假</returns>
        public virtual bool Save(IEnumerable<T> entities, ref XTransaction tran, bool isSupress = false)
        {
            if (tran == null) tran = string.IsNullOrEmpty(connectionName) ? SessionManager.Instance.BeginTransaction() : SessionManager.Instance.BeginTransaction(connectionName);
            var adp = DataAdpterFactory.CreateAdp<T, Guid>(ref tran);
            try
            {
                foreach (var entity in entities)
                {
                    entity.AccessChange();
                    var id = entity.Id;
                    entity.Id = default(Guid);
                    entity.Id = id;
                    adp.SaveModel(entity, true);
                }
                if (!isSupress) tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("{0}:{1}", "Save", ex.Message);
                if (!isSupress) tran.RollbackTransaction();
                throw new Exception(ex.Message);
            }
            finally { adp.Dispose(); if (!isSupress)tran.Dispose(); }

        }

        ///从数据库删除一个类型为T的实体
        /// <summary>
        ///从数据库删除一个类型为T的实体
        /// </summary>
        /// <param name="ids">ID集合</param>
        /// <param name="tran">事务</param>
        /// <param name="isSupress">是否挂起事务，如果挂起，则不提交事务；如果不挂起，则提交事务，默认不挂起</param>
        /// <returns>成功返回真否则为假</returns>
        public virtual bool Delete(IEnumerable<Guid> ids, ref XTransaction tran, bool isSupress = false)
        {
            if (tran == null) tran = string.IsNullOrEmpty(connectionName) ? SessionManager.Instance.BeginTransaction() : SessionManager.Instance.BeginTransaction(connectionName);
            var adp = DataAdpterFactory.CreateAdp<T, Guid>(ref tran);
            try
            {
                foreach (var id in ids)
                    adp.DeleteModel(id, true);
                if (!isSupress) tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("{0}:{1}", "Delete", ex.Message);
                if (!isSupress) tran.RollbackTransaction();
                throw new Exception(ex.Message);
            }
            finally { adp.Dispose(); if (!isSupress)tran.Dispose(); }
        }
        /// 通过ID获取实体
        /// <summary>
        /// 通过ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetEntityById(Guid id)
        {
            T entity = default(T);
            try
            {
                return GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).First();
            }
            catch (Exception ex)
            {
                if(id != Guid.Empty)
                    log.ErrorFormat("{0}:{1}", "GetEntityById", ex.Message);
            }
            return entity;
        }
        /// 根据表达式从数据库获取第一个类型为T的实体集合
        /// <summary>
        /// 根据表达式从数据库获取第一个类型为T的实体集合
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <param name="mapping">映射</param>
        /// <param name="orderExpression">排序表达式</param>
        /// <param name="isSupressLazyLoad">是否挂起</param>
        /// <param name="isLoadReference">是否延迟引用</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <param name="outDistinctMapping">排除自定义字段</param>
        /// <returns></returns>
        public virtual T GetFirstOrDefaultEntityByExpression(string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            IList<T> entities = null;
            try
            {

                using (var adp = DataAdpterFactory.CreateAdp<T, Guid>(connectionName))
                using (var s = adp.GetBaseSession())
                {

                    if (string.IsNullOrEmpty(expression)) expression = "Id=-null";
                    ICriterion criterion = NHibernateQueryExpressionConverter<T>.ConvertToCriterion(expression, null);
                    NHibernate.ICriteria criteria = s.CreateCriteria<T>(typeof(T).Name);
                    criteria.Add(criterion);
                    var orders = OrderExpressionConverter.ConvertToOrderExppression(orderExpression, mapping);
                    if (orders != null && orders.Count > 0)
                    {
                        foreach (var order in orders)
                            criteria.AddOrder(order);
                    }
                    if (isDistinct)
                    {
                        var projections = Projections.ProjectionList();
                        projections.Add(Projections.Alias(Projections.Property("Id"), "Id"));
                        var items = s.SessionFactory.GetClassMetadata(typeof(T).FullName).PropertyNames;
                        foreach (var item in items)
                        {
                            var ptype = s.SessionFactory.GetClassMetadata(typeof(T).FullName).GetPropertyType(item);
                            //排除Image字段
                            if (ptype.GetType().Equals(typeof(NHibernate.Type.BinaryBlobType))) continue;
                            //排除自定义字段
                            if (outDistinctMapping != null && outDistinctMapping.Contains(item)) continue;

                            projections.Add(Projections.Alias(Projections.Property(item), item));
                        }
                        criteria.SetProjection(Projections.Distinct(projections));

                        criteria.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(T)));
                    }
                    entities = criteria.SetFirstResult(0).SetMaxResults(1).List<T>();
                    if (entities == null || entities.Count == 0) return null;
                    if (isLoadReference || isSupressLazyLoad)
                    {
                        foreach (var entity in entities)
                        {
                            if (isLoadReference)
                                entity.LoadReference();
                            if (isSupressLazyLoad)
                                entity.IsSupressLazyLoad = true;
                        }
                    }
                    return entities.First();
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("{0}:{1}", "GetEntitiesByExpression", ex.Message);
            }
            return entities == null ? null : entities.First();
        }
        /// 根据表达式从数据库获取制定范围类型为T的实体集合
        /// <summary>
        /// 根据表达式从数据库获取制定范围类型为T的实体集合
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <param name="beginIndex">当前页码</param>
        /// <param name="takeCount">分页大小</param>
        /// <param name="mapping">映射</param>
        /// <param name="orderExpression">排序表达式</param>
        /// <param name="isSupressLazyLoad">是否挂起</param>
        /// <param name="isLoadReference">是否延迟引用</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <param name="outDistinctMapping">排除自定义字段</param>
        /// <returns></returns>
        public virtual IList<T> GetScopeEntitiesByExpression(string expression, int beginIndex, int takeCount, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            IList<T> entities = null;
            try
            {

                using (var adp = DataAdpterFactory.CreateAdp<T, Guid>(connectionName))
                using (var s = adp.GetBaseSession())
                {

                    if (string.IsNullOrEmpty(expression)) expression = "Id=-null";
                    ICriterion criterion = NHibernateQueryExpressionConverter<T>.ConvertToCriterion(expression, null);
                    NHibernate.ICriteria criteria = s.CreateCriteria<T>(typeof(T).Name);
                    criteria.Add(criterion);
                    var orders = OrderExpressionConverter.ConvertToOrderExppression(orderExpression, mapping);
                    if (orders != null && orders.Count > 0)
                    {
                        foreach (var order in orders)
                            criteria.AddOrder(order);
                    }
                    if (isDistinct)
                    {
                        var projections = Projections.ProjectionList();
                        projections.Add(Projections.Alias(Projections.Property("Id"), "Id"));
                        var items  = s.SessionFactory.GetClassMetadata(typeof(T).FullName).PropertyNames;
                        foreach (var item in items)
                        {
                            var ptype = s.SessionFactory.GetClassMetadata(typeof(T).FullName).GetPropertyType(item);
                            //排除Image字段
                            if (ptype.GetType().Equals(typeof(NHibernate.Type.BinaryBlobType))) continue;
                            //排除自定义字段
                            if (outDistinctMapping != null && outDistinctMapping.Contains(item)) continue;

                            projections.Add(Projections.Alias(Projections.Property(item), item));
                        }
                        criteria.SetProjection(Projections.Distinct(projections));

                        criteria.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(T)));
                    }
                    entities = criteria.SetFirstResult(beginIndex - 1).SetMaxResults(takeCount).List<T>();
                    if (entities == null || entities.Count == 0) return null;
                    if (isLoadReference || isSupressLazyLoad)
                    {
                        foreach (var entity in entities)
                        {
                            if (isLoadReference)
                                entity.LoadReference();
                            if (isSupressLazyLoad)
                                entity.IsSupressLazyLoad = true;
                        }
                    }
                    return entities;
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("{0}:{1}", "GetEntitiesByExpression", ex.Message);
            }
            return entities;
        }
        /// 根据表达式从数据库获取类型为T的实体集合
        /// <summary>
        /// 根据表达式从数据库获取类型为T的实体集合
        /// </summary>
        /// <param name="expression">T类型实体对象实例</param>
        /// <param name="mapping">映射</param>
        /// <param name="orderExpression">排序表达式</param>
        /// <param name="isSupressLazyLoad">是否挂起</param>
        /// <param name="isLoadReference">是否延迟引用</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <param name="outDistinctMapping">排除自定义字段</param>
        /// <returns></returns>
        public virtual IList<T> GetEntitiesByExpression(string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            IList<T> entities = null;
            try
            {

                using (var adp = DataAdpterFactory.CreateAdp<T, Guid>(connectionName))
                using (var s = adp.GetBaseSession())
                {

                    if (string.IsNullOrEmpty(expression)) expression = "Id=-null";
                    ICriterion criterion = NHibernateQueryExpressionConverter<T>.ConvertToCriterion(expression, null);
                    NHibernate.ICriteria criteria = s.CreateCriteria<T>(typeof(T).Name);
                    criteria.Add(criterion);
                    var orders = OrderExpressionConverter.ConvertToOrderExppression(orderExpression, mapping);
                    if (orders != null && orders.Count > 0)
                    {
                        foreach (var order in orders)
                            criteria.AddOrder(order);
                    }
                    if (isDistinct)
                    {
                        var projections = Projections.ProjectionList();
                        projections.Add(Projections.Alias(Projections.Property("Id"), "Id"));
                        var items = s.SessionFactory.GetClassMetadata(typeof(T).FullName).PropertyNames;
                        foreach (var item in items)
                        {
                            var ptype = s.SessionFactory.GetClassMetadata(typeof(T).FullName).GetPropertyType(item);
                            //排除Image字段
                            if (ptype.GetType().Equals(typeof(NHibernate.Type.BinaryBlobType))) continue;
                            //排除自定义字段
                            if (outDistinctMapping != null && outDistinctMapping.Contains(item)) continue;

                            projections.Add(Projections.Alias(Projections.Property(item), item));
                        }
                        criteria.SetProjection(Projections.Distinct(projections));

                        criteria.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(T)));
                    }
                    entities = criteria.List<T>();
                    if (entities == null || entities.Count == 0) return null;
                    if (isLoadReference || isSupressLazyLoad)
                    {
                        foreach (var entity in entities)
                        {
                            if (isLoadReference)
                                entity.LoadReference();
                            if (isSupressLazyLoad)
                                entity.IsSupressLazyLoad = true;
                        }
                    }
                    return entities;
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("{0}:{1}", "GetEntitiesByExpression", ex.Message);
            }
            return entities;
        }
        /// 根据表达式从数据库获取类型为T的实体集合
        /// <summary>
        /// 根据表达式从数据库获取类型为T的实体集合
        /// </summary>
        /// <param name="dataGridSettings">EasyUI dategrid配置实例</param>
        /// <param name="mapping">映射</param>
        /// <param name="orderExpression">排序表达式</param>
        /// <param name="isSupressLazyLoad">是否挂起</param>
        /// <param name="isLoadReference">是否延迟引用</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <param name="outDistinctMapping">排除自定义字段</param>
        /// <returns>成功返回真否则为假</returns>
        public virtual IList<T> GetEntitiesByExpression(DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!string.IsNullOrWhiteSpace(dataGridSettings.SortColumn) && orderExpression.IndexOf(dataGridSettings.SortColumn) == -1)
            {
                var jqGridOrderExpression = string.Format("{0} {1}", dataGridSettings.SortColumn, dataGridSettings.SortOrder);
                if (string.IsNullOrWhiteSpace(orderExpression))
                    orderExpression = jqGridOrderExpression;
                else
                    orderExpression = string.Format("{0},{1}", orderExpression.Trim(), jqGridOrderExpression.Trim());
            }
            return GetEntitiesByExpression(dataGridSettings.QueryExpression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
        }
        /// 根据表达式从数据库获取类型为T的实体集合
        /// <summary>
        /// 根据表达式从数据库获取类型为T的实体集合
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <param name="pageindex">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="mapping">映射</param>
        /// <param name="orderExpression">排序表达式</param>
        /// <param name="isSupressLazyLoad">是否挂起</param>
        /// <param name="isLoadReference">是否延迟引用</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <param name="outDistinctMapping">排除自定义字段</param>
        /// <returns></returns>
        public virtual IList<T> GetEntitiesByExpression(string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            try
            {
                using (var adp = DataAdpterFactory.CreateAdp<T, Guid>(connectionName))
                using (var s = adp.GetBaseSession())
                {
                    if (string.IsNullOrEmpty(expression)) expression = "Id=-null";
                    ICriterion criterion = NHibernateQueryExpressionConverter<T>.ConvertToCriterion(expression, null);
                    NHibernate.ICriteria criteria = s.CreateCriteria<T>();
                    criteria.Add(criterion);
                    var orders = OrderExpressionConverter.ConvertToOrderExppression(orderExpression, mapping);
                    if (orders != null && orders.Count > 0)
                    {
                        foreach (var order in orders)
                            criteria.AddOrder(order);
                    }
                    if (isDistinct)
                    {
                        var projections = Projections.ProjectionList();
                        projections.Add(Projections.Alias(Projections.Property("Id"), "Id"));
                        var items = s.SessionFactory.GetClassMetadata(typeof(T).FullName).PropertyNames;
                        foreach (var item in items)
                        {
                            var ptype = s.SessionFactory.GetClassMetadata(typeof(T).FullName).GetPropertyType(item);
                            //排除Image字段
                            if (ptype.GetType().Equals(typeof(NHibernate.Type.BinaryBlobType))) continue;
                            //排除自定义字段
                            if (outDistinctMapping != null && outDistinctMapping.Contains(item)) continue;

                            projections.Add(Projections.Alias(Projections.Property(item), item));
                        }
                        criteria.SetProjection(Projections.Distinct(projections));

                        criteria.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(T)));
                    }
                    var entities = criteria.SetFirstResult((pageindex - 1) * pageSize).SetMaxResults(pageSize).List<T>();
                    if (entities == null || entities.Count == 0) return null;
                    if (isLoadReference || isSupressLazyLoad)
                    {
                        foreach (var entity in entities)
                        {
                            if (isLoadReference)
                                entity.LoadReference();
                            if (isSupressLazyLoad)
                                entity.IsSupressLazyLoad = true;
                        }
                    }
                    return entities;
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("{0}:{1}", "GetEntitiesByExpression", ex.Message);
                return null;
            }
        }
        /// 根据表达式从数据库获取类型为GridData的实体集合
        /// <summary>
        /// 根据表达式从数据库获取类型为GridData的实体集合
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <param name="pageindex">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="mapping">映射</param>
        /// <param name="orderExpression">排序表达式</param>
        /// <param name="isSupressLazyLoad">是否挂起</param>
        /// <param name="isLoadReference">是否延迟引用</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <param name="outDistinctMapping">排除自定义字段</param>
        /// <returns></returns>
        public virtual GridData<T> GetGridEntitiesByExpression(string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            try
            {
                GridData<T> gridData = new GridData<T>();
                using (var adp = DataAdpterFactory.CreateAdp<T, Guid>(connectionName))
                using (var s = adp.GetBaseSession())
                {
                    if (string.IsNullOrEmpty(expression)) expression = "Id=-null";
                    ICriterion criterion = NHibernateQueryExpressionConverter<T>.ConvertToCriterion(expression, null);
                    NHibernate.ICriteria criteria = s.CreateCriteria<T>();
                    criteria.Add(criterion);
                    if (isDistinct)
                    {
                        var projections = Projections.ProjectionList();
                        projections.Add(Projections.Alias(Projections.Property("Id"), "Id"));
                        var items = s.SessionFactory.GetClassMetadata(typeof(T).FullName).PropertyNames;
                        foreach (var item in items)
                        {
                            var ptype = s.SessionFactory.GetClassMetadata(typeof(T).FullName).GetPropertyType(item);
                            //排除Image字段
                            if (ptype.GetType().Equals(typeof(NHibernate.Type.BinaryBlobType))) continue;
                            //排除自定义字段
                            if (outDistinctMapping != null && outDistinctMapping.Contains(item)) continue;

                            projections.Add(Projections.Alias(Projections.Property(item), item));
                        }
                        criteria.SetProjection(Projections.Distinct(projections));
                        criteria.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(T)));
                    }
                    var orders = OrderExpressionConverter.ConvertToOrderExppression(orderExpression, mapping);
                    if (orders != null && orders.Count > 0)
                    {
                        foreach (var order in orders)
                            criteria.AddOrder(order);
                    }
                    var entities = criteria.SetFirstResult((pageindex - 1) * pageSize).SetMaxResults(pageSize).List<T>();
                    if (entities == null || entities.Count == 0) return null;
                    if (isLoadReference || isSupressLazyLoad)
                    {
                        foreach (var entity in entities)
                        {
                            if (isLoadReference)
                                entity.LoadReference();
                            if (isSupressLazyLoad)
                                entity.IsSupressLazyLoad = true;
                        }
                    }
                    gridData.Data = entities;
                    gridData.Count = CountModelListByExpression(expression, mapping, isDistinct,outDistinctMapping);
                    return gridData;
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("{0}:{1}", "GetEntitiesByExpression", ex.Message);
                return null;
            }
        }
        /// 根据表达式从数据库获取类型为GridData的实体集合
        /// <summary>
        /// 根据表达式从数据库获取类型为GridData的实体集合
        /// </summary>
        /// <param name="dataGridSettings">EasyUI dategrid配置实例</param>
        /// <param name="mapping">映射</param>
        /// <param name="orderExpression">排序表达式</param>
        /// <param name="isSupressLazyLoad">是否挂起</param>
        /// <param name="isLoadReference">是否延迟引用</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <param name="outDistinctMapping">排除自定义字段</param>
        /// <returns>成功返回真否则为假</returns>
        public virtual GridData<T> GetGridEntitiesByExpression(DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!string.IsNullOrWhiteSpace(dataGridSettings.SortColumn) && orderExpression.IndexOf(dataGridSettings.SortColumn) == -1)
            {
                var jqGridOrderExpression = string.Format("{0} {1}", dataGridSettings.SortColumn, dataGridSettings.SortOrder);
                if (string.IsNullOrWhiteSpace(orderExpression))
                    orderExpression = jqGridOrderExpression;
                else
                    orderExpression = string.Format("{0},{1}", orderExpression.Trim(), jqGridOrderExpression.Trim());
            }

            return GetGridEntitiesByExpression(dataGridSettings.QueryExpression, dataGridSettings.PageIndex, dataGridSettings.PageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
        }
        /// 根据表达式获取记录数
        /// <summary>
        /// 根据表达式获取记录数
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <param name="mapping">映射</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <param name="outDistinctMapping">排除自定义字段</param>
        /// <returns></returns>
        public virtual int CountModelListByExpression(string expression, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            try
            {
                if (mapping == null)
                {
                    mapping = new Dictionary<string, string>();
                    var entityName = typeof(T).Name;
                    if (entityName.StartsWith("View")) entityName = entityName.Substring(4);
                    mapping["Id"] = entityName + "Id";
                }
                using (var adp = DataAdpterFactory.CreateAdp<T, Guid>(connectionName))
                    return adp.CountModelListByExpression(expression, mapping, isDistinct);
            }
            catch (Exception ex)
            {
                log.ErrorFormat("{0}:{1}", "CountModelListByExpression", ex.Message);
                return 0;
            }
        }

        /// 根据sql语句从数据库获取对象实体集合 
        /// <summary>
        /// 根据sql语句从数据库获取对象实体集合 
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="isSupressLazyLoad">是否挂起</param>
        /// <param name="isLoadReference">是否延迟引用</param>
        /// <returns></returns>
        public IList<T> GetEntitiesBySql(string sql, bool isSupressLazyLoad = false, bool isLoadReference = false)
        {
            try
            {
                using (var adp = DataAdpterFactory.CreateAdp<T, Guid>(connectionName))
                {
                    var entities = adp.GetModelListBySql(sql);
                    if (entities == null || entities.Count == 0) return null;
                    if (isLoadReference || isSupressLazyLoad)
                    {
                        foreach (var entity in entities)
                        {
                            if (isLoadReference)
                                entity.LoadReference();
                            if (isSupressLazyLoad)
                                entity.IsSupressLazyLoad = true;
                        }
                    }
                    return entities;
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("{0}:{1}", "GetEntitiesBySql", ex.Message);
                return null;
            }
        }
        /// 获取多个结果
        /// <summary>
        /// 获取多个结果
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object GetMutiResult(string sql)
        {
            try
            {
                using (var adp = DataAdpterFactory.CreateAdp<T, Guid>(connectionName))
                {
                    return adp.GetMultiResult(sql);
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("{0}:{1}", "GetMutiResult", ex.Message);
                return null;
            }
        }
        /// 获取单个结果
        /// <summary>
        /// 获取单个结果
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object GetSingleResult(string sql)
        {
            try
            {
                using (var adp = DataAdpterFactory.CreateAdp<T, Guid>(connectionName))
                {
                    return adp.GetSingleResult(sql);
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("{0}:{1}", "GetSingleResult", ex.Message);
                return null;
            }
        }
        /// 转换表达式为sql语句
        /// <summary>
        /// 转换表达式为sql语句
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="mapping">映射</param>
        /// <returns></returns>
        public string ConverExpressionToSql(string expression, Dictionary<string, string> mapping)
        {
            using (var adp = DataAdpterFactory.CreateAdp<T, Guid>(connectionName))
            {
                return adp.ConverExpressionToSql(expression, mapping);
            }
        }
        /// 生成EasyUI datagrid配置
        /// <summary>
        /// 生成EasyUI datagrid配置
        /// </summary>
        /// <param name="queryExpression">查询表达式</param>
        /// <param name="orderExpression">排序表达式</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="mpping">映射</param>
        /// <returns></returns>
        public DataGridSettings GenerateDataGridSetting(string queryExpression, string orderExpression, int pageIndex, int pageSize, Dictionary<string, string> mpping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = queryExpression, PageIndex = pageIndex, PageSize = pageSize };
            string sortColumn = "", sortOrder = "";
            if (!string.IsNullOrWhiteSpace(orderExpression))
            {
                var order = OrderExpressionConverter.ConvertToOrderExppression(orderExpression, mpping).First();
                sortColumn = order.ToString().Split(' ')[0];
                sortOrder = order.ToString().Split(' ')[1];
                dataGridSettings.SortColumn = sortColumn;
                dataGridSettings.SortOrder = sortOrder;
            }
            return dataGridSettings;
        }
        /// 获取随机大小集合
        /// <summary>
        /// 获取随机大小集合
        /// </summary>
        /// <param name="inputList">输入集合</param>
        /// <param name="randomCount">随机数</param>
        /// <returns></returns>
        public IList<T> GetRandomList(List<T> inputList, int randomCount)
        {
            T[] copyArray = new T[inputList.Count];
            inputList.CopyTo(copyArray);

            List<T> copyList = new List<T>();
            copyList.AddRange(copyArray);

            List<T> outputList = new List<T>();
            Random rd = new Random(DateTime.Now.Millisecond);

            while (copyList.Count > 0 && randomCount > 0)
            {
                int rdIndex = rd.Next(0, copyList.Count - 1);
                T remove = copyList[rdIndex];
                copyList.Remove(remove);
                outputList.Add(remove);
                randomCount--;
            }
            return outputList;
        }
    }
}
