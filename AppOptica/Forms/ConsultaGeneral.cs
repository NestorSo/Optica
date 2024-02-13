using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppOptica.Forms
{
    public class ConsultaGeneral
    {
        public string PNC { get; set; }
        public string SNC { get; set; }
        public string PAC { get; set; }
        public string SAC { get; set; }
        public DateTime FechaC { get; set; }
        public float AddOD_Old { get; set; }
        public float AddOI_Old { get; set; }
        public float AddOD { get; set; }
        public float AddOI { get; set; }
        public float DipOD { get; set; }
        public float DipOI { get; set; }
        public float AlturaOD { get; set; }
        public float AlturaOI { get; set; }
        public string TipoL { get; set; }

    }

}
