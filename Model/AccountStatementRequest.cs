using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebServiceEnrollment.Model
{
    [DataContract]
    public class AccountStatementRequest
    {
        [DataMember]
        public string Carne { get; set; }   
    }
}