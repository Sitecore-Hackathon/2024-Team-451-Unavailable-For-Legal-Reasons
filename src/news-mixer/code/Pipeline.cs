using NewsMixer.InputSources;
using NewsMixer.Models;
using NewsMixer.Output;
using NewsMixer.Transforms;

namespace NewsMixer
{
    public class Pipeline
    {
        private readonly List<ISourceInput> _inputs = [];
        private readonly List<PipelineStream> _streams = [];

        public Pipeline AddInput(params ISourceInput[] inputs)
        {
            _inputs.AddRange(inputs);

            return this;
        }

        public Pipeline AddStream(Action<PipelineStream> cfg)
        {
            var stream = new PipelineStream();

            cfg(stream);

            _streams.Add(stream);

            return this;
        }

        public class PipelineStream
        {
            private readonly List<ITransform> _transforms = [];
            private readonly List<IOutput> _outputs = [];

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
                var itm = input;
                foreach (var t in _transforms)
                {
                    itm = await t.Execute(itm, token);
                }

                await Parallel.ForEachAsync(_outputs, async (o, ics) => await o.Execute(itm, ics));
            }
        }

        public async Task Execute(CancellationToken token) => await Parallel.ForEachAsync(_inputs, token, async (source, ics) =>
                                                                       {
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
