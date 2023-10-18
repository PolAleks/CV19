using System.Data;

namespace CV19.Models
{
    internal struct ConfirmedCount
    {
        // Дата 
        public DataSetDateTime Date { get; set; }

        // Кол-во заражений на указанную дату
        public int Count { get; set; }
    }

}
