using AutoMapper;
using Flurl.Http;
using Metamorphosis.App.Intrfaces;
using Metamorphosis.App.Models;
using System.Text.RegularExpressions;

namespace Metamorphosis.App.Services;

public class MyCdnServices : IMyCdnServices
{
    private readonly IMapper _mapper;

    public MyCdnServices(IMapper mapper)
    {
        _mapper = mapper;
    }

    
    public MyCdn ConvertArrayStringInMyCdnObject(string[] myCdnParts)
    {
        return _mapper.Map<MyCdn>(myCdnParts);
    }

    
    public string[] ConvertStringArrayInMyCdnFormat(string myCdnFormatText)
    {
        var firstPart = myCdnFormatText.Split("|").ToList();
        var secondPart = firstPart[3].Split("/").ToList();

        firstPart.RemoveAt(3);
        secondPart.RemoveAt(2);
        secondPart.Add("MINHA CDN");

        return firstPart.Union(secondPart).ToArray();
    }

    
    public async Task<string> GetLogFormatMyCDN(string source)
    {
        var response = string.Empty;

        try
        {
            response = await source.GetStringAsync();
        }
        catch (FlurlHttpException ex)
        {
            response = $"Error when making request: {ex.Message}";
        }

        return response;

    }

    public bool ValidateSourceInput(string input)
    {
        var urlPattern = @"^https:\/\/.*\.txt$";
        var pathPattern = @"^\.\/output\/.+\.txt$";

        string[] parts = input.Split(' ');

        if (parts.Length != 2)
        {
            return false;
        }

        if (!Regex.IsMatch(parts[0], urlPattern))
        {
            return false;
        }

        if (!Regex.IsMatch(parts[1], pathPattern))
        {
            return false;
        }

        return true;
    }
}

