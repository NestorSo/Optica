using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppOptica.Forms
{
    [Table("Clientes")]

    public class Cliente
    {
        public int Cliente_ID { get; set; }

        public DateTime FechaR { get; set; }
        public string PNC { get; set; }
        public string SNC { get; set; }
        public string PAC { get; set; }
        public string SAC { get; set; }
        private string _telC;
        public string TelC
        {
            get => _telC;
            set
            {
                if (IsValidPhoneNumber(value))
                {
                    _telC = value;
                }
                else
                {
                    // Puedes manejar la invalidación de la entrada aquí
                    throw new ArgumentException("Número de teléfono no válido");
                }
            }
        }

        public string DirC { get; set; }
        public string Ocupacion { get; set; }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Patrón para validar un número de teléfono de 8 dígitos que comienza con 2, 5, 7 u 8
            string pattern = "^[2|5|7|8][0-9]{7}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }
    }
}
