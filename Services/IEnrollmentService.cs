// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
using System.ServiceModel;
using WebServiceEnrollment.Model;

namespace WebServiceEnrollment.Services
{
    [ServiceContract]
    public interface IEnrollmentService
    {
        [OperationContract]
        public string Test(string message);
        [OperationContract]
        EnrollmentResponse EnrollmentProcess(EnrollmentRequest request);
        [OperationContract]
        AccountStatementResponse AccountStatementProcess(AccountStatementRequest request);
        [OperationContract]
        AspiranteResponse CandidateRecordProcess(AspiranteRequest request);
    }
}