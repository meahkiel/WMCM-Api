using Application.DTO;
using Application.SeedWorks;

using Core.Campaigns;


namespace Application.Campaigns.Commands
{
    public static class Create
    {
        public class Command : IRequest<Result<CampaignDTO>>
        {
            
            public string Title { get; set; } = "";
            public string Description { get; set; } = "";
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
        }

        public class CreateUpdateValidator : AbstractValidator<Command>
        {
            public CreateUpdateValidator()
            {
                RuleFor(x => x.Title).NotNull().NotEmpty();

                RuleFor(x => x.DateFrom)
                    .GreaterThanOrEqualTo(DateTime.Today)
                    .LessThanOrEqualTo(x => x.DateTo);

                RuleFor(x => x.DateTo).GreaterThanOrEqualTo(x => x.DateFrom);
            }
        }

        public class CommandHandler : IRequestHandler<Command, Result<CampaignDTO>>
        {
            private readonly UnitWrapper _context;
            private readonly IMapper _mapper;
            private readonly IValidator<Command> _validator;

            public CommandHandler(
                UnitWrapper context, 
                IMapper mapper,
                IValidator<Command> validator)
            {
                _context = context;
                _mapper = mapper;
                _validator = validator;
            }

            public async Task<Result<CampaignDTO>> Handle(Command request, CancellationToken cancellationToken)
            {

                try
                {
                    var validateResult = await _validator.ValidateAsync(request);
                    
                    if(!validateResult.IsValid)
                    {
                        return Result<CampaignDTO>.Failure(validateResult.Errors[0].ErrorMessage);
                    }

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
