using Metamorphosis.App.Models;

namespace Metamorphosis.App.Intrfaces;

public interface INowServices
{
    string CreateLogInNowFormat(MyCdn myCdn);
    string CreateHeaderLog();
    string ConvertMyCdnToNow(string myCdnFormatText);
    void CreateFileFormatNow(string contents, string fileName);
}
