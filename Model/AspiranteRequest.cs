using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebServiceEnrollment.Model
{
    [DataContract]
    public class AspiranteRequest
    {
        [DataMember]
        public string Apellido { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string Direccion { get; set; }
        [DataMember]
        public string Telefono { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string CarreraId { get; set; }
        [DataMember]
        public string ExamenId { get; set; }
        [DataMember]
        public string JornadaId { get; set; }
    }
}