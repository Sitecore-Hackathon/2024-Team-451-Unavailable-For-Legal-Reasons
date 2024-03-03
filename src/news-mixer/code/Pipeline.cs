using Microsoft.Extensions.Logging;
using NewsMixer.InputSources;
using NewsMixer.Models;
using NewsMixer.Output;
using NewsMixer.Transforms;

namespace NewsMixer
{
    public class Pipeline(ILoggerFactory loggerFactory)
    {
        private readonly List<ISourceInput> _inputs = [];
        private readonly List<PipelineStream> _streams = [];
        private readonly ILoggerFactory _loggerFactory = loggerFactory;
        private readonly ILogger _logger = loggerFactory.CreateLogger<Pipeline>();

        public Pipeline AddInput(params ISourceInput[] inputs)
        {
            _inputs.AddRange(inputs);

            return this;
        }

        public Pipeline AddStream(Action<PipelineStream> cfg)
        {
            var stream = new PipelineStream(_loggerFactory);

            cfg(stream);

            _streams.Add(stream);

            return this;
        }

        public class PipelineStream(ILoggerFactory loggerFactory)
        {
            private readonly List<ITransform> _transforms = [];
            private readonly List<IOutput> _outputs = [];
            private readonly ILogger _logger = loggerFactory.CreateLogger<PipelineStream>();

            public PipelineStream AddTransform(params ITransform[] transforms)
            {
                _transforms.AddRange(transforms);

                return this;
            }

            public PipelineStream AddOutput(params IOutput[] outputs)
            {
                _outputs.AddRange(outputs);

                return this;
            }

            public async Task Execute(NewsItem input, CancellationToken token)
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("executing stream...");
                }

                var itm = input;

                foreach (var t in _transforms)
                {
                    if (_logger.IsEnabled(LogLevel.Debug))
                    {
                        _logger.LogDebug("executing transform...");
                    }

                    itm = await t.Execute(itm, _logger, token);

                    if (_logger.IsEnabled(LogLevel.Debug))
                    {
                        _logger.LogDebug("transform completed");
                    }
                }

                foreach (var output in _outputs)
                {
                    await output.Execute(itm, token);
                }

                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("stream completed.");
                }
            }
        }

        public async Task Execute(CancellationToken token)
        {

            foreach (var source in _inputs)
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("executing source...");
                }

                var enumerable = source.Execute(token);

                await foreach (var t in enumerable)
                {
                    foreach (var stream in _streams)
                    {
                        await stream.Execute(t, token);
                    }
                }
            }
        }
    }

    public interface IPipelineConfig
    {
        IPipelineConfig AddTransform(params ITransform[] transforms);
        IPipelineConfig AddOutput(params IOutput[] outputs);
    }
}
