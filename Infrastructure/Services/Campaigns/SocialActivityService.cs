using Application.Interface;
using Application.SeedWorks;
using Core.Campaigns;
using Core.Channels;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure.Services.Campaigns;

public class SocialActivityService : IActivityServiceAccessor
{
    private readonly HttpClient _httpClient;
    private readonly ChannelSetting _setting;

    public SocialActivityService(HttpClient httpClient,ChannelSetting setting)
    {
        _httpClient = httpClient;
        _setting = setting;
    }


    public async Task<ServiceMessageBroker> Execute(Activity activity)
    {

        _httpClient.BaseAddress = new Uri(_setting.BaseUrl);
        var url =  $"?access_token={_setting.ApiSecretKey}&message={activity.Body}";
        
        var result = await _httpClient.PostAsync(url, null);
        if (!result.IsSuccessStatusCode) {
            throw new Exception("Error encountered while sending message.");
        }
        
        return new ServiceMessageBroker(DateTime.Today, "Success", true);
    }


}
