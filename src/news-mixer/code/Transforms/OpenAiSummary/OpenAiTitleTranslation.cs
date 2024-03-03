using Microsoft.Extensions.Logging;
using NewsMixer.Models;

namespace NewsMixer.Transforms.OpenAiSummary
{
    public class OpenAiTitleConfiguration
    {
        public string ApiKey { get; set; } = "";
        public string? Language { get; set; }
        public string DeploymentName { get; set; } = "gpt-3.5-turbo";
    }

    public class OpenAiTitleTranslation(OpenAiTitleConfiguration config, IHttpClientFactory httpClientFactory) : ITransform
    {
        private readonly IOpenAiClient _client = new OpenAiPersistedCacheClient(config.ApiKey, httpClientFactory.CreateClient());

        public async Task<NewsItem> Execute(NewsItem itm, ILogger logger, CancellationToken cancellationToken)
        {
            var resultLanguage = config.Language ?? itm.ContentLanguage;

            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("executing transformer {transformer} for language={language}...", nameof(OpenAiTitleTranslation), resultLanguage);
            }

            if (resultLanguage == itm.ContentLanguage)
            {
                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug("completed transformer {transformer}, nothing to do.", nameof(OpenAiTitleTranslation));
                }

                return itm;
            }

            var result = await _client.GetChatCompletionsAsync(logger, new ChatCompletionsRequest
            {
                DeploymentName = config.DeploymentName,
                UserMessage = "Please give me the following title in {Language}.".Replace("{Language}", resultLanguage) + "\n\n" + itm.Title,
            }, cancellationToken);

            itm.Title = result;
            itm.ContentLanguage = resultLanguage;

            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("completed transformer {transformer}.", nameof(OpenAiTitleTranslation));
            }

            return itm;
        }
    }
}
