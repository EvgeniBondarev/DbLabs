using MediatR;

namespace Application.CQRS.Labs.LabCommands
{
    public class UpdateLabCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
