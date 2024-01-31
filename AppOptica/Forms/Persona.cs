using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppOptica.Forms
{
    [Table("Personas")]
    public class Persona
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string? PrimerNombre { get; set; }
        public string? SegundoNombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? Direccion { get; set; }
        public string? Cedula { get; set; }
    }
}
