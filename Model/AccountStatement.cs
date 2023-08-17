using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceEnrollment.Model
{
    public class AccountStatement
    {
        public string Carne { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public decimal Mora { get; set; }
        public decimal Descuento { get; set; }
    }
}