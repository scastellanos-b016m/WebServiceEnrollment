// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

using WebServiceEnrollment.Model;
using WebServiceEnrollment.DB;
using Serilog;
using WebServiceEnrollment.Utils;

namespace WebServiceEnrollment.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private IConfiguration _Configuration;
        public EnrollmentService(IConfiguration Configuration)
        {
            this._Configuration = Configuration;
        }
        public AccountStatementResponse AccountStatementProcess(AccountStatementRequest request)
        {
            AccountStatementResponse response = Conexion.GetInstancia(this._Configuration).GetAccountStatement(request);
            return response;
        }

        public AspiranteResponse CandidateRecordProcess(AspiranteRequest request)
        {
            AppLog appLog = new AppLog();
            appLog.ResponseTime = Convert.ToInt16(DateTime.Now.ToString("fff"));
            AspiranteResponse response = Conexion.GetInstancia(this._Configuration).ExecuteQuery(request);
            if (response.StatusCode == 201)
            {
                Utilerias.ImprimirLog(appLog, response.StatusCode, response.Message, "Information");
            }
            else
            {
                Utilerias.ImprimirLog(appLog, response.StatusCode, response.Message, "Error");
            }

            return response;
        }

        public EnrollmentResponse EnrollmentProcess(EnrollmentRequest request)
        {
            AppLog appLog = new AppLog();
            // appLog.ResponseTime = Convert.ToInt16(DateTime.Now.ToString("fff"));
            appLog.ResponseTime = Convert.ToInt16(DateTime.Now.ToString(this._Configuration.GetValue<string>("Profiles:formatDate")));

            EnrollmentResponse response = Conexion.GetInstancia(this._Configuration).ExecuteQuery(request);
            if (response.StatusCode == 201)
            {
                Utilerias.ImprimirLog(appLog, response.StatusCode, response.Message, "Information");
            }
            else
            {
                Utilerias.ImprimirLog(appLog, response.StatusCode, response.Message, "Error");
            }
            return response;
        }

        public string Test(string message)
        {
            string servicio = "Servicio Enrollment Up";
            return servicio; 
        }
    }
}