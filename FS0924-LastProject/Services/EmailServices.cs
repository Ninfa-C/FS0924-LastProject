using FluentEmail.Core;
using LastProject.Models.Email;

namespace LastProject.Services
{
    public class EmailServices
    {
        private readonly IFluentEmail _email;
        public EmailServices(IFluentEmail fluentEmail)
        {
            _email = fluentEmail;
        }

        public async Task<bool> SendConfirm(string name, string email, string confirmationLink)
        {
            var model = new ConfirmationEmail
            {
                Name = name,
                ConfirmationLink = confirmationLink
            };

            var result = await _email
                .To(email)
                .Subject("Conferma registrazione")
                .UsingTemplateFromFile("Templates/EmailConfirmation.cshtml", model)
                .SendAsync();

            return result.Successful;
        }
    }
}
