
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System;
using System.Collections.ObjectModel;

namespace AppOptica.Forms
{
    public class Consulta
    {
        public int IdCon { get; set; }
        public DateTime FechaC { get; set; }
        public int Cliente_ID { get; set; }
        public string Motivo { get; set; }
        public string Antecedentes { get; set; }
        public float OD { get; set; }
        public float OI { get; set; }
        public string TipoL { get; set; }
        public float ADD_ { get; set; }
        public float DIP { get; set; }
        public float Altura { get; set; }
    }

}
