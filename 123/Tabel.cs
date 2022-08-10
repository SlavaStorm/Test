namespace TabelDoc.Models
{
    public class Tabel
    {      
        public int Number { get; set; }
        public string FullName { get; set; }
        public int UnfulfilledDoc { get; set; }
        public int Unfulfilledletter { get; set; }
        public int TotalNumber { get; set; }

        public  Tabel()
        {
            TotalNumber = 1;
        }
    }
}
