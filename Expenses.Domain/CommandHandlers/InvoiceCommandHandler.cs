using Expenses.Domain.Commands;
using Expenses.Domain.Core.Commands;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Domain.CommandHandlers
{
    public class InvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, bool>
    {
        public InvoiceCommandHandler()
        {

        }

        public Task<bool> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
