using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebServiceEnrollment.Model
{
    [DataContract]
    public class AspiranteResponse
    {
        [DataMember]
        public int StatusCode { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string NoExpediente { get; set; }
    }
}