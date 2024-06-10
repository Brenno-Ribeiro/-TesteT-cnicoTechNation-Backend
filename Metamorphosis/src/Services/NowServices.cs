using Metamorphosis.App.Intrfaces;
using Metamorphosis.App.Models;
using System.Globalization;
using System.Text;

namespace Metamorphosis.App.Services;

public class NowServices : INowServices
{
    private readonly IMyCdnServices _myCdnServices;
    private readonly IFileServices _fileServices;

    public NowServices(IMyCdnServices myCdnServices, IFileServices fileServices)
    {
        _myCdnServices = myCdnServices;
        _fileServices = fileServices;
    }

    public string CreateLogInNowFormat(MyCdn myCdn)
    {
        var sb = new StringBuilder();

        sb.Append($"\"{myCdn.Provider}\" ");
        sb.Append($"{myCdn.HttpMetod?.Replace("\"", "")} ");
        sb.Append($"{myCdn.StatusCode} ");
        sb.Append($"/{myCdn.UriPath?.Replace(" HTTP", "")} ");
        sb.Append($"{Math.Round(double.Parse($"{myCdn.TimeTaken} ", CultureInfo.InvariantCulture)).ToString()} ");
        sb.Append($"{myCdn.ResponseSize} ");
        sb.Append(myCdn.CacheStatus == "INVALIDATE" ? "REFRESH_HIT" : myCdn.CacheStatus);

        return sb.ToString();
    }

    public string CreateHeaderLog()
    {
        var sb = new StringBuilder();
        sb.AppendLine("#Version: 1.0");
        sb.AppendLine($"#Date: {DateTime.Now}");
        sb.AppendLine("#Fields: provider http-method status-code uri-path time-taken\r\n response-size cache-status");
        return sb.ToString();
    }


    public string ConvertMyCdnToNow(string myCdnFormatText)
    {
        var parts = _myCdnServices.ConvertStringArrayInMyCdnFormat(myCdnFormatText);
        var myCdn = _myCdnServices.ConvertArrayStringInMyCdnObject(parts);
        var nowFormatLog = CreateLogInNowFormat(myCdn);
        return nowFormatLog;
    }

    public void CreateFileFormatNow(string contents, string fileName)
    {
        var lines = fileName.Split("/");
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var filelPath = Path.Combine(desktopPath, lines[2]);
        _fileServices.WriteAllText(filelPath, contents);
    }
}

