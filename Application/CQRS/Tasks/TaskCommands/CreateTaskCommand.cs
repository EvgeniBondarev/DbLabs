using MediatR;

namespace Application.CQRS.Tasks.TaskCommands
{
    public class CreateTaskCommand : IRequest<int>
    {
        public int Number { get; set; }
        public string Description { get; set; }
        public string Decision { get; set; }
    }
}
