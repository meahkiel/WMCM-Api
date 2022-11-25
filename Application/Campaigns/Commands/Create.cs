using Application.DTO;
using Application.SeedWorks;
using AutoMapper;
using Core.Campaigns;
using FluentValidation;
using MediatR;
using Repositories.Unit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Campaigns.Commands
{
    public class Create
    {
        public class Command : IRequest<Result<CampaignDTO>>
        {
            
            public string Title { get; set; } = "";
            public string Description { get; set; } = "";
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title).NotNull().NotEmpty();

                RuleFor(x => x.DateFrom)
                    .GreaterThanOrEqualTo(DateTime.UtcNow)
                    .LessThanOrEqualTo(x => x.DateTo);
                
                RuleFor(x => x.DateTo).GreaterThanOrEqualTo(x => x.DateFrom);
            }
        }

        public class CommandHandler : IRequestHandler<Command, Result<CampaignDTO>>
        {
            private readonly UnitWrapper _context;
            private readonly IMapper _mapper;

            public CommandHandler(UnitWrapper context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<CampaignDTO>> Handle(Command request, CancellationToken cancellationToken)
            {

                try
                {
                    //check logical data
                    var campaign = new Campaign {
                        Id = Guid.NewGuid(),
                        Title = request.Title,
                        Description = request.Description,
                        DateFrom = request.DateFrom,
                        DateTo = request.DateTo
                    };

                    _context.CampaignRepo.Add(campaign);

                    var result = await _context.SaveChangesAsync();

                    if (!result)
                    {
                        throw new Exception("Failed to create campaign");
                    }

                    return Result<CampaignDTO>.Success(_mapper.Map<CampaignDTO>(campaign));
                }
                catch (Exception ex)
                {
                    return Result<CampaignDTO>.Failure(ex.Message);
                }
            }
        }
    }
}
