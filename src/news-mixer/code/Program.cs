using NewsMixer.InputSources.DummySource;
using NewsMixer.Output.Console;
using NewsMixer.Transforms.DummySummary;

Console.WriteLine("This is the NewsMixer!");
Console.WriteLine("Error 451 Unavailable for Legal Resons");
Console.WriteLine("");

var source = new DummySourceInput();
var transform = new DummySummaryTransform();
var output = new ConsoleOutput();

var token = new CancellationToken();
var enumerable1 = source.Execute(token);
var enumerable2 = transform.Execute(enumerable1, token);
var result = output.Execute(enumerable2, token);

await result;
