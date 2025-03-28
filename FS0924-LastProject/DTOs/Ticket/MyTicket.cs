namespace LastProject.DTOs.Ticket
{
    public class MyTicket
    {
        public Guid TicketId { get; set; }
        public string EventName { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string ArtistName { get; set; }
        public decimal Price { get; set; }
    }
}
