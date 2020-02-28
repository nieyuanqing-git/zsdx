using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class NMPSubjectLimitValidate: NMPAppointmentValidateBase
    {
        private INMPSubjectAppointmentTimeLimitBLL __nmpSubjectAppointmentTimeLimitBLL;
        public NMPSubjectLimitValidate(Guid nmpEquipmentId, Guid nmpId, Guid userId, IEnumerable<NMPAppointment> nmpAppointments)
            : base(nmpEquipmentId, nmpId, userId, nmpAppointments)
        {
            __nmpSubjectAppointmentTimeLimitBLL = BLLFactory.CreateNMPSubjectAppointmentTimeLimitBLL();
        }
        public override bool Validates(out string errorMessage)
        {
            errorMessage = "";
            if (_nmpAppointments != null && _nmpAppointments.Count() > 0)
                return Validates(_nmpAppointments.First(), _nmpAppointments, out errorMessage);
           
            return true;
        }
        private bool Validates(NMPAppointment nmpAppointment, IEnumerable<NMPAppointment> newNMPAppointments, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var lstNMPAppointments = new List<NMPAppointment>();
                if (newNMPAppointments != null && newNMPAppointments.Count() > 0)
                {
                    foreach (var nmpAppointmentTemp in newNMPAppointments)
                    {
                        //取消预约的记录,这里应用了一个小技巧,如果是新增预约的情况,第一个参数为新预约记录的第一条记录,第二个参数对应应该有两条与第一个参数一样的预约记录,因为第一个参数在算未计费的预约总金额时候会减去该记录的金额，所以第二参数需要两条与第一个参数一样的预约记录这样才能保证余额的准确
                        int count = nmpAppointmentTemp.Id == nmpAppointment.Id && newNMPAppointments.Count(p => p.Id == nmpAppointmentTemp.Id) == 1 ? 2 : 1;
                        for (int i = 0; i < count; i++)
                        {
                            lstNMPAppointments.Add(nmpAppointmentTemp);
                        }
                    }
                }
                var subjectAppointmentTimeLimit = __nmpSubjectAppointmentTimeLimitBLL.GetFirstOrDefaultEntityByExpression(string.Format("NMPId=\"{0}\"*SubjectId=\"{1}\"", _nmpId, nmpAppointment.SubjectId.Value));
                if (subjectAppointmentTimeLimit != null)
                {
                    var minDate = nmpAppointment.BeginTime.Value.Date.AddDays(-subjectAppointmentTimeLimit.Period + 1);
                    var endDate = nmpAppointment.BeginTime.Value.Date;
                    for (DateTime dateTime = minDate; dateTime <= endDate; dateTime = dateTime.AddDays(1))
                    {
                        var nmpAppointmentList = _nmpAppointmentBLL.GetSubjectPeriodNMPAppointmentList(nmpAppointment.NMPEquipmentId.Value, nmpAppointment.SubjectId.Value, dateTime, dateTime.AddDays(subjectAppointmentTimeLimit.Period));
                        if (nmpAppointmentList != null && nmpAppointmentList.Count > 0) lstNMPAppointments.AddRange(nmpAppointmentList);
                        var currentHours = lstNMPAppointments.Sum(p => (p.EndTime.Value - p.BeginTime.Value).TotalHours) - (nmpAppointment.EndTime.Value - nmpAppointment.BeginTime.Value).TotalHours;
                        if (subjectAppointmentTimeLimit.Duration >= currentHours) continue;
                        errorMessage = "超出工位课题组预约限制【" + subjectAppointmentTimeLimit.Desription + "】";
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}
