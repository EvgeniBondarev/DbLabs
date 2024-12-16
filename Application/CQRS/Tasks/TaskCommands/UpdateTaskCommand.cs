using MediatR;

namespace Application.CQRS.Tasks.TaskCommands
{
    public class UpdateTaskCommand : IRequest<int>
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public string Decision { get; set; }
    }
}
