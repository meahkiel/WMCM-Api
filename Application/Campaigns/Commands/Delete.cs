using Application.SeedWorks;
using MediatR;
using Repositories.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Campaigns.Commands
{
    public class Delete
    {

        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly UnitWrapper _unitWrapper;

            public CommandHandler(UnitWrapper unitWrapper)
            {
                _unitWrapper = unitWrapper;
            }


            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {

                try
                {
                    var existingCampaign = await _unitWrapper.CampaignRepo
                                                .GetSingleCampaign(Guid.Parse(request.Id));

                    if (existingCampaign == null)
                        throw new Exception("Campaign doesn't exist");

                    //check if there is any activities created
                    if (existingCampaign.Activities.Any())
                    {
                        throw new Exception("Unable to delete. activities has been created");
                    }

                    _unitWrapper.CampaignRepo.Remove(existingCampaign);
                    await _unitWrapper.SaveChangesAsync();

                    return Result<Unit>.Success(Unit.Value);
                }
                catch (Exception ex)
                {
                    return Result<Unit>.Failure(ex.Message);
                }
            }
        }
    }


}
