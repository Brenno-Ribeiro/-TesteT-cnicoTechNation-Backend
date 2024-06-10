using Metamorphosis.App.Models;

namespace Metamorphosis.App.Intrfaces;

public interface ISourceServices
{
    Source SeparateSources(string[] urlSource);
}
