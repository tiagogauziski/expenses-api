namespace Expenses.API.Extensions.Authorization
{
    internal class AuthOptions
    {
        public string Domain { get; set; }

        public string Audience { get; set; }

        public string ClientId { get; set; }
    }
}
