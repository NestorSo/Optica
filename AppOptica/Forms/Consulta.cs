
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System;

namespace AppOptica.Forms
{
    public class Consulta
    {
        public int IdCon { get; set; }

        public DateTime FechaC { get; set; }

     
        public int Cliente_ID { get; set; }

        [NotNull]
        public string Motivo { get; set; }

        [NotNull]
        public string Antecedentes { get; set; }

        public float OD { get; set; }
        public float OI { get; set; }
        [NotNull]
        public string TipoL { get; set; }
        public float ADD_ { get; set; }
        public float DIP { get; set; }
        public float Altura { get; set; }
    }

}
