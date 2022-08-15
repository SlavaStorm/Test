using System.ComponentModel;

namespace TabelDoc.Models
{
    public class Tabel
    {
        [DisplayName("№")]
        public int Number { get; set; }
        [DisplayName("ФИО")]
        public string FullName { get; set; }
        [DisplayName("Невыполненых РКК")]
        public int UnfulfilledDoc { get; set; }
        [DisplayName("Невыполненых обращений")]
        public int Unfulfilledletter { get; set; }
        [DisplayName("Общее количество")]
        public int TotalNumber { get; set; }

        public  Tabel()
        {
            TotalNumber = 1;
        }
    }
}
