using Application.Interface;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class SentimentService : ISentimentService
{
    public SentimentService()
    {
        
    }
    public async Task<object> GetSentiment(string text,string fileName)
    {
        //Load sample data

        var sampleData = new BaseSentiment.ModelInput()
        {
            Description = text,
        };

        //Load model and predict output
        var result = BaseSentiment.Predict(sampleData);

        return await Task.FromResult(new
        {
            Score = result.Score,
            Label = result.PredictedLabel
        });

        throw new NotImplementedException();
    }
}
