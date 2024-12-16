using Application.DTO;
using MediatR;

namespace Application.CQRS.Tasks.TaskQueries
{

    public class GetAllTasksQuery : IRequest<IEnumerable<TaskDto>>
    {
    }
}
