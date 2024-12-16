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
    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskDto>>
    {
        private readonly AppDbContext _dbContext;

        public GetAllTasksQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            // Извлекаем все задачи из базы данных и преобразуем их в DTO
            var tasks = await _dbContext.Tasks
                .Select(task => new TaskDto
                {
                    Id = task.Id,
                    Number = task.Number,
                    Description = task.Description,
                    Decision = task.Decision
                })
                .ToListAsync(cancellationToken);

            return tasks;
        }
    }
}
