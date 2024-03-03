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
        private readonly IOpenAiClient _client = new OpenAiPersistedCacheClient(config.ApiKey, httpClientFactory.CreateClient());

        public async Task<NewsItem> Execute(NewsItem itm, ILogger logger, CancellationToken cancellationToken)
        {
            var resultLanguage = config.Language ?? itm.ContentLanguage;

            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("executing transformer {transformer} for language={language}...", nameof(OpenAiSummaryTransform), resultLanguage);
            }

            var result = await _client.GetChatCompletionsAsync(logger, new ChatCompletionsRequest
            {
                DeploymentName = config.DeploymentName,
                SystemMessage = config.AiBehavior,
                UserMessage = config.UserPrompt + "Create the summary in {Language}.".Replace("{Language}", resultLanguage) + "\n\n" + itm.Content,
            }, cancellationToken);

            itm.Content = result;
            itm.ContentLanguage = resultLanguage;

            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("completed transformer {transformer}.", nameof(OpenAiSummaryTransform));
            }

            return itm;
        }
    }
}
