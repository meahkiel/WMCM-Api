using Core.Channels;

namespace Application.Interface
{
    public interface IServiceFactory
    {
        IActivityServiceAccessor GetWebPost(ChannelSetting setting);
        IActivityServiceAccessor GetSMS(ChannelSetting setting);
        IActivityServiceAccessor GetSMTP(ChannelSetting setting);

        IActivityServiceAccessor GetSocial(ChannelSetting setting);
    



    }
}
