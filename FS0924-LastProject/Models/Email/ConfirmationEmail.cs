namespace LastProject.Models.Email
{
    public class ConfirmationEmail
    {
        public required string Name { get; set; }
        public required string ConfirmationLink { get; set; }
    }
}
