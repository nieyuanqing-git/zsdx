using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleProxyFactory
    {
        public static SampleProxy CreateSampleProxy(SampleRole sampleRole,Guid userId)
        {
            switch (sampleRole)
            {
                case SampleRole.Applicant:
                    return new ApplicantSampleProxy(userId);
                case SampleRole.SampleTransactor:
                    return new SampleTransactorProxy(userId);
                case SampleRole.SampleOfficer:
                    return new SampleOfficerProxy(userId);
                case SampleRole.Tester:
                    return new TesterSampleProxy(userId);
                case SampleRole.Uploader:
                    return new UploadSampleProxy(userId);
                case SampleRole.Releaser:
                    return new ReleaserSampleProxy(userId);
                case SampleRole.DoubtAuditor:
                    return new DoubtAuditSampleProxy(userId); 
                case SampleRole.Seracher:
                    return new SampleSearchSampleProxy(userId);
                case SampleRole.Charger:
                    return new ChargerSampleProxy(userId);
                default:
                    throw new Exception("出错,参数错误");
            }
        }
        public static SampleProxy CreateSampleProxy(string sampleRoleStr, Guid userId)
        {
            switch (sampleRoleStr.Trim())
            {
                case "Applicant":
                    return new ApplicantSampleProxy(userId);
                case "SampleTransactor":
                    return new SampleTransactorProxy(userId);
                case "SampleOfficer":
                    return new SampleOfficerProxy(userId);
                case "Tester":
                    return new TesterSampleProxy(userId);
                case "Uploader":
                    return new UploadSampleProxy(userId);
                case "Releaser":
                    return new ReleaserSampleProxy(userId);
                case "DoubtAuditor":
                    return new DoubtAuditSampleProxy(userId);
                case "Seracher":
                    return new SampleSearchSampleProxy(userId);
                case "Charger":
                    return new ChargerSampleProxy(userId);
                default:
                    throw new Exception("出错,参数错误");
            }
        }
    }
}
