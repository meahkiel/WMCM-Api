using Application.DTO;
using Application.SeedWorks;
using Core.Channels;

namespace Application.Channels
{
    public class Create
    {
        public record Command(ChannelSettingDTO Value) : IRequest<Result<Unit>>;

        public class CommandHandler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly UnitWrapper _context;
            

            public CommandHandler(UnitWrapper context)
            {
                _context = context;
                
            }
            
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                   
                    if(string.IsNullOrEmpty(request.Value.Id))
                    {
                        var channel = new ChannelSetting
                        {
                            Id = Guid.NewGuid(),
                            ApiKey = request.Value.ApiKey,
                            ApiSecretKey = request.Value.ApiSecretKey,
                            BaseUrl = request.Value.BaseUrl,
                            Description = request.Value.Description,
                            Email = request.Value.Email,
                            Header = request.Value.Header,
                            Host = request.Value.Host,
                            IsDisabled = request.Value.IsDisabled,
                            Password = request.Value.Password,
                            PhoneNo = request.Value.PhoneNo,
                            Port = request.Value.Port,
                            Title = request.Value.Title,
                            Type = request.Value.Type,
                            UserName = request.Value.UserName
                        };
                        _context.Channels.Add(channel);

                    }
                    else
                    {
                        var exisitingChannel = await _context.Channels.FindByIdAsync(request.Value.Id); 
                        if(exisitingChannel == null)
                        {
                            throw new Exception("Channel not found");
                        }

                        exisitingChannel.ApiKey = request.Value.ApiKey;
                        exisitingChannel.ApiSecretKey = request.Value.ApiSecretKey;
                        exisitingChannel.BaseUrl = request.Value.BaseUrl;
                        exisitingChannel.Description = request.Value.Description;
                        exisitingChannel.Email = request.Value.Email;
                        exisitingChannel.Header = request.Value.Header;
                        exisitingChannel.Host = request.Value.Host;
                        exisitingChannel.IsDisabled = request.Value.IsDisabled;
                        exisitingChannel.Password = request.Value.Password;
                        exisitingChannel.PhoneNo = request.Value.PhoneNo;
                        exisitingChannel.Port = request.Value.Port;
                        exisitingChannel.Title = request.Value.Title;
                        exisitingChannel.Type = request.Value.Type;
                        exisitingChannel.UserName = request.Value.UserName;

                    }

                    await _context.SaveChangesAsync();

                    return Result<Unit>.Success(Unit.Value);

                }
                catch(Exception ex)
                {
                    return Result<Unit>.Failure(ex.Message);
                }
            }
        }

    }
}
