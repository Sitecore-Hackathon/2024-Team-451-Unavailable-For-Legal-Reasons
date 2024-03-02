using Azure.AI.OpenAI;
using NewsMixer.Models;

namespace NewsMixer.Transforms.OpenAiSummary
{
    public class OpenAiTitleConfiguration
    {
        public string ApiKey { get; set; } = "";
        public string? Language { get; set; }
        public string DeploymentName { get; set; } = "gpt-3.5-turbo";
    }

    public class OpenAiTitleTranslation(OpenAiTitleConfiguration config) : ITransform
    {
        private readonly OpenAIClient _client = new(config.ApiKey);

        public async Task<NewsItem> Execute(NewsItem itm, CancellationToken cancellationToken)
        {
            var resultLanguage = config.Language ?? itm.ContentLanguage;
            if (resultLanguage == itm.ContentLanguage)
            {
                return itm;
            }

            var result = await _client.GetChatCompletionsAsync(new ChatCompletionsOptions
            {
                DeploymentName = config.DeploymentName,
                Messages =
                {
                    new ChatRequestUserMessage("Please give me the following title in {Language}.".Replace("{Language}", resultLanguage) + "\n\n" + itm.Title),
                }

            }, cancellationToken);

            itm.Title = string.Join("\n", result.Value.Choices.Select(x => x.Message?.Content));
            itm.ContentLanguage = resultLanguage;
            return itm;
        }
    }
}
