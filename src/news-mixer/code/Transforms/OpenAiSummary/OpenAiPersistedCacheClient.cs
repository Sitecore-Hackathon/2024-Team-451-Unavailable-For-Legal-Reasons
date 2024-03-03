using Azure;
using Azure.AI.OpenAI;
using Azure.Core.Pipeline;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text.Json;

namespace NewsMixer.Transforms.OpenAiSummary
{
    public class ChatCompletionsRequest
    {
        public string? DeploymentName { get; set; }
        public string? SystemMessage { get; set; }
        public string? UserMessage { get; set; }
    }

    public interface IOpenAiClient
    {
        Task<string> GetChatCompletionsAsync(ILogger logger, ChatCompletionsRequest request, CancellationToken cancellationToken);
    }

    public class OpenAiPersistedCacheClient(string apiKey, HttpClient httpClient) : IOpenAiClient
    {
        private readonly OpenAIClient _client = new(apiKey, new OpenAIClientOptions
        {
            Transport = new HttpClientTransport(httpClient)
        });

        private readonly DirectoryInfo _cacheDir = new DirectoryInfo(
            Environment.GetEnvironmentVariable("CACHE_DIR") ?? Environment.GetEnvironmentVariable("TEMP") ?? throw new ArgumentException("Either CACHE_DIR or TEMP must be specified"));

        public async Task<string> GetChatCompletionsAsync(ILogger logger, ChatCompletionsRequest request, CancellationToken cancellationToken)
        {
            var filePath = GetCacheFilePath(request);
            var result = await ReadResponseFromFile(filePath);
            if (result != null) { return result!; }

            var options = new ChatCompletionsOptions
            {
                DeploymentName = request.DeploymentName
            };
            if (!string.IsNullOrEmpty(request.SystemMessage)) { options.Messages.Add(new ChatRequestSystemMessage(request.SystemMessage)); }
            options.Messages.Add(new ChatRequestSystemMessage(request.UserMessage));

            var response = await _client.GetChatCompletionsAsync(options, cancellationToken);

            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("transformer openapi usage totalTokens={totalTokens}...", response.Value.Usage.TotalTokens);
            }

            result = await WriteResponseToFileAsync(filePath, request, response);
            return result;
        }

        private async Task<string> WriteResponseToFileAsync(string filePath, ChatCompletionsRequest request, Response<ChatCompletions> response)
        {
            var result = string.Join("\n", response.Value.Choices.Select(x => x.Message?.Content));

            var line1 = JsonSerializer.SerializeToElement(request, typeof(ChatCompletionsRequest), new JsonSerializerOptions
            {
                WriteIndented = false,
            });
            
            await File.WriteAllTextAsync(filePath, line1 + Environment.NewLine + result);
            return result;
        }

        private async Task<string?> ReadResponseFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            var content = await File.ReadAllTextAsync(filePath);
            var index = content.IndexOf(Environment.NewLine);
            content = content.Substring(index).Trim();
            return content;
        }

        private string GetCacheFilePath(ChatCompletionsRequest request)
        {
            if (!_cacheDir.Exists)
            {
                _cacheDir.Create();
            }

            using var md5 = MD5.Create();
            var behavior = GetHash(md5, request.SystemMessage ?? string.Empty);
            var msg = GetHash(md5, request.UserMessage ?? string.Empty);

            var fileName = $"{request.DeploymentName}-{behavior}-{msg}";
            var filePath = Path.Join(_cacheDir.FullName, fileName);
            return filePath;
        }

        private static string GetHash(MD5 alg, string input)
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = alg.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes);
        }
    }
}
