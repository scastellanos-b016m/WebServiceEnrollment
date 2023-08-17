using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WebServiceEnrollment.Model
{
    [DataContract]
    public class EnrollmentRequest
    {
        [DataMember]
        public string NoExpediente { get; set; }
        [DataMember]
        public string Ciclo { get; set; } 
        [DataMember]
        public int MesInicioPago { get; set; }
        [DataMember]
        public string CarreraId { get; set; }
        [DataMember]
        public string InscripcionCargoId { get; set; }
        [DataMember]
        public string CarneCargoId { get; set; }
        [DataMember]
        public string CargoMensualId { get; set; }
        [DataMember]
        public string DiaPago { get; set; }

    }
}