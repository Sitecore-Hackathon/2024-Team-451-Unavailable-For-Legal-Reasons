using Microsoft.Extensions.Logging;
using NewsMixer.InputSources;
using NewsMixer.Models;
using NewsMixer.Output;
using NewsMixer.Transforms;

namespace NewsMixer
{
    public class Pipeline
    {
        public Pipeline(ILogger logger)
        {
            _logger = logger;
        }

        private readonly List<ISourceInput> _inputs = [];
        private readonly List<PipelineStream> _streams = [];
        private readonly ILogger _logger;

        public Pipeline AddInput(params ISourceInput[] inputs)
        {
            _inputs.AddRange(inputs);

            return this;
        }

        public Pipeline AddStream(Action<PipelineStream> cfg)
        {
            var stream = new PipelineStream(_logger);

            cfg(stream);

            _streams.Add(stream);

            return this;
        }

        public class PipelineStream
        {
            public PipelineStream(ILogger logger)
            {
                _logger = logger;
            }

            private readonly List<ITransform> _transforms = [];
            private readonly List<IOutput> _outputs = [];
            private readonly ILogger _logger;

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
                    itm = await t.Execute(itm, _logger, token);
                }

                await Parallel.ForEachAsync(_outputs, async (o, ics) => await o.Execute(itm, ics));
            }
        }

        public async Task Execute(CancellationToken token) => await Parallel.ForEachAsync(_inputs, token, async (source, ics) =>
                                                                       {
                                                                           if (_logger.IsEnabled(LogLevel.Debug))
                                                                           {
                                                                               _logger.LogDebug("executing source...");
                                                                           }

                                                                           var enumerable = source.Execute(ics);

                                                                           await foreach (var t in enumerable)
                                                                           {
                                                                               await Parallel.ForEachAsync(_streams, async (stream, ics2) =>
                                                                               {
                                                                                   await stream.Execute(t, ics2);
                                                                               });
                                                                           }
                                                                       });
    }

    public interface IPipelineConfig
    {
        IPipelineConfig AddTransform(params ITransform[] transforms);
        IPipelineConfig AddOutput(params IOutput[] outputs);
    }
}
