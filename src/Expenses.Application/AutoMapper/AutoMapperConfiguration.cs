using AutoMapper;

namespace Expenses.Application.AutoMapper
{
    /// <summary>
    /// AutoMapper configuration for the application.
    /// </summary>
    public class AutoMapperConfiguration
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new InvoiceProfile());
                cfg.AddProfile(new StatementProfile());
            });
        }
    }
}
