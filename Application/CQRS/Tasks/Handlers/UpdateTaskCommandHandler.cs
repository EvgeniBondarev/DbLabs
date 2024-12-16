using Application.CQRS.Tasks.TaskCommands;
using MediatR;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Tasks.Handlers
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, int>
    {
        private readonly AppDbContext _dbContext;

        public UpdateTaskCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            // Находим задачу по идентификатору
            var task = await _dbContext.Tasks.FindAsync(new object[] { request.Id }, cancellationToken);

            // Если задача не найдена, возвращаем 0 или выбрасываем исключение
            if (task == null)
            {
                return 0; // Можно заменить на throw new Exception("Task not found.");
            }

            // Обновляем свойства задачи
            task.Number = request.Number;
            task.Description = request.Description;
            task.Decision = request.Decision;

            // Сохраняем изменения в базе данных
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Возвращаем идентификатор обновленной задачи
            return task.Id;
        }
    }
}
