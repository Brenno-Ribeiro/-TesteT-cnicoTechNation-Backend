
using Metamorphosis.App.Intrfaces;
namespace Metamorphosis.App.Services;

internal class FileServices : IFileServices
{
    public FileServices()
    {
    }

    public void WriteAllText(string path, string contents)
    {
        File.WriteAllText(path, contents);
    }
}
