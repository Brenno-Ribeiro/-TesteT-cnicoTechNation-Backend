namespace Metamorphosis.App.Models;

public class MyCdn
{
    public string? ResponseSize { get; set; }
    public string? StatusCode { get; set; }
    public string? CacheStatus { get; set; }
    public string? TimeTaken { get; set; }
    public string? HttpMetod { get; set; }
    public string? UriPath { get; set; }
    public string? Provider { get; set; }

    public MyCdn()
    {
            
    }
    public MyCdn(string? responseSize, string? statusCode, string? cacheStatus, string? timeTaken, string? httpMetod, string? uriPath, string? provider)
    {
        ResponseSize = responseSize;
        StatusCode = statusCode;
        CacheStatus = cacheStatus;
        TimeTaken = timeTaken;
        HttpMetod = httpMetod;
        UriPath = uriPath;
        Provider = provider;
    }
}

