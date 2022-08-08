using ProEvents.Domain.Model;

namespace ProEvents.Service.DTOs
{
    public class BatchDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Amount { get; set; }
        public int EventId { get; set; }
        public EventDTO Event { get; set; }
    }
}