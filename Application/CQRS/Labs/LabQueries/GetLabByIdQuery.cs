using Application.DTO;
using MediatR;


namespace Application.CQRS.Labs.LabQueries
{
    public class GetLabByIdQuery : IRequest<LabDto>
    {
        public int Id { get; set; }
    }
}
