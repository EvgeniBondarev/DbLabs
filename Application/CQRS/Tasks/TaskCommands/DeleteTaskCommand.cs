using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Tasks.TaskCommands
{
    public class DeleteTaskCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}
