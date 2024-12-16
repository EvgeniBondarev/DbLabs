using Application.DTO;
using MediatR;

namespace Application.CQRS.Tasks.TaskQueries
{
    public class GetTaskByIdQuery : IRequest<TaskDto>
    {
        public int Id { get; set; }
    }
}
