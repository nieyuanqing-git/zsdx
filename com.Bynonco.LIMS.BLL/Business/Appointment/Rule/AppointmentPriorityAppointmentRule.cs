using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL.Business.AppointmentRule
{
    public class AppointmentPriorityAppointmentRule : AppointmentRuleBase
    {
        private IAppointmentPriorityBLL __appointmentPriorityBLL;
        private RuleBusinessTypeBLL _ruleBusinessTypeBLL;
        private IList<AppointmentPriority> __appointmentPriorityList;
        private IUserBLL __userBll;
        private User user;
        private Guid __equipmentId;
        private Guid? __userId;
        private Dictionary<string, bool> isUserInBusiness = new Dictionary<string, bool>();
        private Dictionary<string, List<AppointmentPriorityUser>> allAppointmentPriorityUserList = new Dictionary<string, List<AppointmentPriorityUser>>();
        public AppointmentPriorityAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, AppointmentRuleBase next, Guid equipmentId, Guid? userId, Equipment equipment, User user)
             : base(equipmentAppoitmentTimes, next, equipment, user)
        {
            this.__userId = userId;
            __equipmentId = equipmentId;
            __appointmentPriorityBLL = BLLFactory.CreateAppointmentPriorityBLL();
            _ruleBusinessTypeBLL = new RuleBusinessTypeBLL();
            __userBll = BLLFactory.CreateUserBLL();
            this.user = user;


            __appointmentPriorityList = __appointmentPriorityBLL.GetValidateAppointmentPriorityListByEquipmentId(__equipmentId);
            //判断当前用户是否存在规则里面
            if (isUserInBusiness != null && isUserInBusiness.Count <= 0 && __appointmentPriorityList != null && __appointmentPriorityList.Count > 0 && user != null)
            {
                foreach (var appointmentPriority in __appointmentPriorityList)
                {
                    //当前用户是否满足规则
                    if (!isUserInBusiness.ContainsKey(appointmentPriority.id.ToString()))
                    {
                        var isflag = appointmentPriority.Businesses.Any(p => _ruleBusinessTypeBLL.IsUserInBusiness(user, p.BusinessObject));
                        isUserInBusiness.Add(appointmentPriority.id.ToString(), isflag);
                    }
                    allAppointmentPriorityUserList.Add(appointmentPriority.id.ToString(), appointmentPriority.Businesses.Where(p => _ruleBusinessTypeBLL.IsUserInBusiness(user, p.BusinessObject)).ToList());
                }
            }
        }
        protected override void Prepare()
        {
            __appointmentPriorityList = __appointmentPriorityBLL.GetValidateAppointmentPriorityListByEquipmentId(__equipmentId);
        }

        //public override void Superimposing(Model.Business.EquipmentAppointmentTime appointmentTime)
        //{
        //    if (__appointmentPriorityList == null || __appointmentPriorityList.Count == 0 || appointmentTime.Status == EquipmentAppointmentTimeStatus.Invalid) return;
        //    foreach (var appointmentPriority in __appointmentPriorityList)
        //    {
        //        if (appointmentPriority.Businesses != null && appointmentPriority.Businesses.Count > 0 && appointmentPriority.Businesses.Any(p => p.UserId == __userId)) continue;
        //        if (!appointmentPriority.StartAt.HasValue) appointmentPriority.StartAt = DateTime.MinValue;
        //        if (!appointmentPriority.EndAt.HasValue) appointmentPriority.EndAt = DateTime.MaxValue;
        //        if ((appointmentTime.BeginTime - DateTime.Now).TotalDays >= appointmentPriority.PriorityDays)
        //        {
        //            var appointmentPriorityUserList = appointmentPriority.Businesses == null || !__userId.HasValue ? null : appointmentPriority.Businesses.Where(p => p.UserId == __userId.Value);
        //            if ((appointmentTime.BeginTime - appointmentPriority.StartAt.Value).TotalMilliseconds >= 0 &&
        //                (appointmentPriorityUserList == null || appointmentPriorityUserList.Count() == 0) && appointmentPriority.Businesses != null)
        //            {
        //                if (__userId.HasValue)
        //                {
        //                    var appointmentPriorityListFind = __appointmentPriorityList.Where(p => (!p.EndAt.HasValue || p.EndAt.Value >= appointmentTime.BeginTime) && p.Id != appointmentPriority.Id);
        //                    if (appointmentPriorityListFind != null && appointmentPriorityListFind.Count() > 0)
        //                    {
        //                        foreach (var appointmentPriorityFind in appointmentPriorityListFind)
        //                        {
        //                            if (appointmentPriorityFind.Businesses != null && appointmentPriorityFind.Businesses.Count > 0 && appointmentPriorityFind.Businesses.Any(p => p.UserId == __userId))
        //                            {
        //                                if (WeekDayUtility.ConvertToWordDays(appointmentPriorityFind.WorkDays).Any(p => p == appointmentTime.BeginTime.DayOfWeek))
        //                                {
        //                                    var priorityimeScopers = TimeUtility.GetTimeScopersByTimeFormatStr(appointmentPriorityFind.PriorityHours, _equipment.AppointmentTimeStep.Value, appointmentTime.BeginTime.Date);
        //                                    if (priorityimeScopers.Any(p => DateTimeUtility.IsContain(appointmentTime.BeginTime, appointmentTime.EndTime, p.BeginTime, p.EndTime)))
        //                                    {
        //                                        return;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                if (!string.IsNullOrWhiteSpace(appointmentPriority.WorkDays))
        //                {
        //                    if (!WeekDayUtility.ConvertToWordDays(appointmentPriority.WorkDays).Any(p => p == appointmentTime.BeginTime.DayOfWeek)) continue;
        //                }
        //                if (!string.IsNullOrWhiteSpace(appointmentPriority.PriorityHours))
        //                {
        //                    var priorityimeScopers = TimeUtility.GetTimeScopersByTimeFormatStr(appointmentPriority.PriorityHours, _equipment.AppointmentTimeStep.Value, appointmentTime.BeginTime.Date);
        //                    if (priorityimeScopers.Any(p => DateTimeUtility.IsContain(appointmentTime.BeginTime, appointmentTime.EndTime, p.BeginTime, p.EndTime)))
        //                    {
        //                        appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
        //                        appointmentTime.Remark = appointmentPriority.Remark;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        public override void Superimposing(Model.Business.EquipmentAppointmentTime appointmentTime)
        {
            if (__appointmentPriorityList == null || __appointmentPriorityList.Count == 0 || appointmentTime.Status == EquipmentAppointmentTimeStatus.Invalid) return;

            foreach (var appointmentPriority in __appointmentPriorityList)
            {
                //修改 
                var mapKey = appointmentPriority.id.ToString();
                IList<AppointmentPriorityUser> Businesses = appointmentPriority.Businesses;

                // if ( Businesses.Count > 0 && Businesses.Any(p => _ruleBusinessTypeBLL.IsUserInBusiness(user, p.BusinessObject))) continue;
                if (Businesses.Count > 0 && isUserInBusiness.ContainsKey(appointmentPriority.id.ToString()) && (bool)isUserInBusiness[mapKey]) continue;
                if (!appointmentPriority.StartAt.HasValue) appointmentPriority.StartAt = DateTime.MinValue;
                if (!appointmentPriority.EndAt.HasValue) appointmentPriority.EndAt = DateTime.MaxValue;
                if ((!appointmentPriority.CancelByDay && (appointmentTime.BeginTime - DateTime.Now).TotalDays >= appointmentPriority.PriorityDays) || (appointmentPriority.CancelByDay && (appointmentTime.BeginTime.Date - DateTime.Now.Date).TotalDays >= appointmentPriority.PriorityDays))
                {
                    //var appointmentPriorityUserList = appointmentPriority.Businesses == null || !__userId.HasValue ? null : appointmentPriority.Businesses.Where(p => _ruleBusinessTypeBLL.IsUserInBusiness(user, p.BusinessObject));
                    var appointmentPriorityUserList = Businesses == null || !__userId.HasValue ? null : !allAppointmentPriorityUserList.ContainsKey(mapKey) ? null : allAppointmentPriorityUserList[mapKey];



                    if ((appointmentTime.BeginTime - appointmentPriority.StartAt.Value).TotalMilliseconds >= 0 && (appointmentPriorityUserList == null || appointmentPriorityUserList.Count() == 0) && Businesses != null)
                    {
                        if (__userId.HasValue)
                        {
                            var appointmentPriorityListFind = __appointmentPriorityList.Where(p => (!p.EndAt.HasValue || p.EndAt.Value >= appointmentTime.BeginTime) && p.Id != appointmentPriority.Id);

                            var appointmentPriorityFinds = appointmentPriorityListFind as IList<AppointmentPriority> ?? appointmentPriorityListFind.ToList();

                            if (appointmentPriorityFinds.Any())
                            {
                                foreach (var appointmentPriorityFind in appointmentPriorityFinds)
                                {

                                    //if (appointmentPriorityFind.Businesses != null && appointmentPriorityFind.Businesses.Count > 0 && appointmentPriorityFind.Businesses.Any(p => _ruleBusinessTypeBLL.IsUserInBusiness(user, p.BusinessObject)))
                                    if (appointmentPriorityFind.Businesses != null && appointmentPriorityFind.Businesses.Count > 0 && isUserInBusiness.ContainsKey(appointmentPriorityFind.id.ToString()) && (bool)isUserInBusiness[appointmentPriorityFind.id.ToString()])
                                    {
                                        if (WeekDayUtility.ConvertToWordDays(appointmentPriorityFind.WorkDays).Any(p => p == appointmentTime.BeginTime.DayOfWeek))
                                        {
                                            var priorityimeScopers = TimeUtility.GetTimeScopersByTimeFormatStr(appointmentPriorityFind.PriorityHours, _equipment.AppointmentTimeStep.Value, appointmentTime.BeginTime.Date);
                                            if (priorityimeScopers.Any(p => DateTimeUtility.IsContain(appointmentTime.BeginTime, appointmentTime.EndTime, p.BeginTime, p.EndTime)))
                                            {
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(appointmentPriority.WorkDays))
                        {
                            if (!WeekDayUtility.ConvertToWordDays(appointmentPriority.WorkDays).Any(p => p == appointmentTime.BeginTime.DayOfWeek)) continue;
                        }
                        if (!string.IsNullOrWhiteSpace(appointmentPriority.PriorityHours))
                        {
                            var priorityimeScopers = TimeUtility.GetTimeScopersByTimeFormatStr(appointmentPriority.PriorityHours, _equipment.AppointmentTimeStep.Value, appointmentTime.BeginTime.Date);
                            if (priorityimeScopers.Any(p => DateTimeUtility.IsContain(appointmentTime.BeginTime, appointmentTime.EndTime, p.BeginTime, p.EndTime)))
                            {
                                appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                                appointmentTime.Remark = appointmentPriority.Remark;
                            }
                        }
                    }
                }
            }
        }
    }
}
