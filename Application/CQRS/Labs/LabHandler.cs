using Application.CQRS.Labs.LabCommands;
using Application.CQRS.Labs.LabQueries;
using Application.DTO;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.CQRS.Labs
{
    public class LabHandler :
        IRequestHandler<CreateLabCommand, int>,
        IRequestHandler<UpdateLabCommand, Unit>,
        IRequestHandler<DeleteLabCommand, Unit>,
        IRequestHandler<GetLabByIdQuery, LabDto>,
        IRequestHandler<GetAllLabsQuery, IEnumerable<LabDto>>
    {
        private readonly AppDbContext _context;

        public LabHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateLabCommand request, CancellationToken cancellationToken)
        {
            var lab = new Lab { Name = request.Name };
            _context.Labs.Add(lab);
            await _context.SaveChangesAsync(cancellationToken);
            return lab.Id;
        }

        public async Task<Unit> Handle(UpdateLabCommand request, CancellationToken cancellationToken)
        {
            var lab = await _context.Labs.FindAsync(request.Id);
            if (lab != null)
            {
                lab.Name = request.Name;
                _context.Labs.Update(lab);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteLabCommand request, CancellationToken cancellationToken)
        {
            var lab = await _context.Labs.FindAsync(request.Id);
            if (lab != null)
            {
                _context.Labs.Remove(lab);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }

        public async Task<LabDto> Handle(GetLabByIdQuery request, CancellationToken cancellationToken)
        {
            var lab = await _context.Labs.FindAsync(request.Id);
            return lab != null ? new LabDto { Id = lab.Id, Name = lab.Name } : null;
        }

        public async Task<IEnumerable<LabDto>> Handle(GetAllLabsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Labs
                .Select(l => new LabDto { Id = l.Id, Name = l.Name })
                .ToListAsync(cancellationToken);
        }
    }
}
