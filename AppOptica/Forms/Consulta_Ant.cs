using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppOptica.Forms
{
    public class Consulta_Ant
    {
        [PrimaryKey, AutoIncrement]
        public int IdConAnt { get; set; }
        // Otras propiedades...

        [ForeignKey("Consulta")]
        public int IdCon { get; set; }
        [PrimaryKey]

        public DateTime FechaC { get; set; }

        [ForeignKey("Cliente")]
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
