using Azure.AI.OpenAI;
using Azure.Core.Pipeline;
using Microsoft.Extensions.Logging;
using NewsMixer.Models;

namespace NewsMixer.Transforms.OpenAiSummary
{
    public class OpenAiSummaryConfiguration
    {
        public string ApiKey { get; set; } = null!;
        public string? Language { get; set; }
        public string DeploymentName { get; set; } = "gpt-3.5-turbo";
        public string UserPrompt = "Create a summary of the following text so that I can easily get an idea if the article is worth reading and what I would learn from the article.";
        public string AiBehavior = "You are a 5 year old kindergaden child";
    }

    public class OpenAiSummaryTransform(OpenAiSummaryConfiguration config, IHttpClientFactory httpClientFactory) : ITransform
    {
        private readonly OpenAIClient _client = new(config.ApiKey, new OpenAIClientOptions
        {
            Transport = new HttpClientTransport(httpClientFactory.CreateClient())
        });

        public async Task<NewsItem> Execute(NewsItem itm, ILogger logger, CancellationToken cancellationToken)
        {
            var resultLanguage = config.Language ?? itm.ContentLanguage;

            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("executing transformer {transformer} for language={language}...", nameof(OpenAiSummaryTransform), resultLanguage);
            }

            var result = await _client.GetChatCompletionsAsync(new ChatCompletionsOptions
            {
                DeploymentName = config.DeploymentName,
                Messages =
                {
                    new ChatRequestSystemMessage(config.AiBehavior),
                    new ChatRequestUserMessage(config.UserPrompt + "Create the summary in {Language}.".Replace("{Language}", resultLanguage) + "\n\n" + itm.Content),
                }
            }, cancellationToken);

            itm.Content = string.Join("\n", result.Value.Choices.Select(x => x.Message?.Content));
            itm.ContentLanguage = resultLanguage;

            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("completed transformer {transformer}.", nameof(OpenAiSummaryTransform));
            }

            return itm;
        }
    }
}
