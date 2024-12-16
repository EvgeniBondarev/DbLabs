using MediatR;


namespace Application.CQRS.Labs.LabCommands
{
    public class CreateLabCommand : IRequest<int>
    {
        public string Name { get; set; }
    }
}
