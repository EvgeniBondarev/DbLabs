using MediatR;

namespace Application.CQRS.Labs.LabCommands
{
    public class DeleteLabCommand : IRequest
    {
        public int Id { get; set; }
    }
}
