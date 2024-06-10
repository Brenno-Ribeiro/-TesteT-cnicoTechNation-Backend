using Metamorphosis.App.Models;

namespace Metamorphosis.App.Intrfaces;

public interface IMyCdnServices
{
    MyCdn ConvertArrayStringInMyCdnObject(string[] myCdnParts);
    string[] ConvertStringArrayInMyCdnFormat(string myCdnFormatText);
    bool ValidateSourceInput(string input);
    Task<string> GetLogFormatMyCDN(string sourceUrl);
}
