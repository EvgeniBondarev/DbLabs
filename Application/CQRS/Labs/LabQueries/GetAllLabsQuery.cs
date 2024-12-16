using Application.DTO;
using MediatR;

namespace Application.CQRS.Labs.LabQueries
{
    public class GetAllLabsQuery : IRequest<IEnumerable<LabDto>>
    {
    }
}
