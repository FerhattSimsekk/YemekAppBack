using MediatR;

namespace Presentation.Controllers
{
    internal class DeleteEmployeeCommand : IRequest<object>
    {
        private long id;

        public DeleteEmployeeCommand(long id)
        {
            this.id = id;
        }
    }
}