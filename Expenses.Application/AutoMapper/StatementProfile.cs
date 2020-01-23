using AutoMapper;
using Expenses.Application.Statement.ViewModel;
using Expenses.Domain.Commands.Statement;
using Expenses.Domain.Queries.Statement;

namespace Expenses.Application.AutoMapper
{
    /// <summary>
    /// Statement AutoMapper Profile configuration.
    /// </summary>
    public class StatementProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatementProfile"/> class.
        /// </summary>
        public StatementProfile()
        {
            CreateMap<CreateStatementRequest, CreateStatementCommand>();
            CreateMap<UpdateStatementRequest, UpdateStatementCommand>();
            CreateMap<CreateStatementCommand, Expenses.Domain.Models.Statement>()
                .ForMember(m => m.Date, o => o.AddTransform(t => t.Date.Date));
            CreateMap<UpdateStatementCommand, Expenses.Domain.Models.Statement>()
                .ForMember(m => m.Date, o => o.AddTransform(t => t.Date.Date));
            CreateMap<GetStatementListRequest, GetStatementListQuery>();
            CreateMap<Expenses.Domain.Models.Statement, StatementResponse>();
        }
    }
}
