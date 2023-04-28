using Application.SeedWorks;
using Core.Campaigns;

namespace Application.Campaigns.Commands
{
    public static class Edit
    {
        public record Command(Guid Id, DateTime DateFrom, DateTime DateTo) : IRequest<Result<Unit>>
        {
            public string Title { get; set; } = "";
            public string Description { get; set; } = "";
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

        public class CommandHandler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly UnitWrapper _context;
            private readonly IMapper _mapper;
            private readonly IValidator<Command> _validator;

            public CommandHandler(UnitWrapper context, IMapper mapper, IValidator<Command> validator)
            {
                _context = context;
                _mapper = mapper;
                _validator = validator;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var validateResult = await _validator.ValidateAsync(request);
                    
                    if (!validateResult.IsValid)
                    {
                        return Result<Unit>.Failure(validateResult.Errors[0].ErrorMessage);
                    }

                    var campaign = _mapper.Map<Campaign>(request);
                    _context.CampaignRepo.Update(campaign);
                    await _context.SaveChangesAsync();

                    return Result<Unit>.Success(Unit.Value);
                }
                catch (Exception e)
                {

                    return Result<Unit>.Failure(e.Message);
                }

            }
        }

    }
}
