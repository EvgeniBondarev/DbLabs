using Application.CQRS.Tasks.TaskQueries;
using Application.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Tasks.Handlers
{
    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
    {
        private readonly AppDbContext _dbContext;

        public GetTaskByIdQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            // Находим задачу по идентификатору
            var task = await _dbContext.Tasks
                .Where(t => t.Id == request.Id)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Number = t.Number,
                    Description = t.Description,
                    Decision = t.Decision
                })
                .FirstOrDefaultAsync(cancellationToken);

            // Если задача не найдена, возвращаем null или выбрасываем исключение
            if (task == null)
            {
                return null; // Можно заменить на throw new Exception("Task not found.");
            }

            return task;
        }
    }
}
