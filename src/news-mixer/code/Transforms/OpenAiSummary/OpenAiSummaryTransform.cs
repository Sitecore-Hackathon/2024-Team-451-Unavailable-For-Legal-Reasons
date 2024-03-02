using Azure.AI.OpenAI;
using NewsMixer.Models;

namespace NewsMixer.Transforms.OpenAiSummary
{
    public class OpenAiSummaryConfiguration
    {
        public string ApiKey { get; set; } = "";
        public string? Language { get; set; }
        public string DeploymentName { get; set; } = "gpt-3.5-turbo";
        public string UserPrompt = "Create a summary of the following text so that I can easily get an idea if the article is worth reading and what I would learn from the article.";
        public string AiBehavior = "You are a 5 year old kindergaden child";
    }

    public class OpenAiSummaryTransform(OpenAiSummaryConfiguration config) : ITransform
    {
        private readonly OpenAIClient _client = new(config.ApiKey);

        public async Task<NewsItem> Execute(NewsItem itm, CancellationToken cancellationToken)
        {
            var result = await _client.GetChatCompletionsAsync(new ChatCompletionsOptions
            {
                DeploymentName = config.DeploymentName,
                Messages =
                {
                    new ChatRequestSystemMessage(config.AiBehavior),
                    new ChatRequestUserMessage(config.UserPrompt + "Create the summary in {Language}.".Replace("{Language}", config.Language ?? itm.ContentLanguage) + "\n\n" + itm.Content)
                }

            }, cancellationToken);

            itm.Content = string.Join("\n", result.Value.Choices.Select(x => x.Message?.Content));
            itm.ContentLanguage = config.Language ?? itm.ContentLanguage;
            return itm;
        }
    }
}
