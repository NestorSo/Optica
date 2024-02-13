
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System;
using System.Collections.ObjectModel;
using SQLiteNetExtensions.Attributes;

namespace AppOptica.Forms
{
    public class Consulta
    {
        public int IdCon { get; set; }
        public DateTime FechaC { get; set; }
        public int Cliente_ID { get; set; }
        public string Motivo { get; set; }
        public string Antecedentes { get; set; }
        public float AddOD { get; set; }
        public float AddOI { get; set; }
        public float DipOD { get; set; }
        public float DipOI { get; set; }
        public float AlturaOD { get; set; }
        public float AlturaOI { get; set; }
        public string TipoL { get; set; }
    }


}
