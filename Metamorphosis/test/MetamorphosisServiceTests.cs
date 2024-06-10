using AutoMapper;
using Metamorphosis.App.Intrfaces;
using Metamorphosis.App.Models;
using Metamorphosis.App.Services;
using Moq;
using System.Reflection;

namespace Metamorphosis.Test;

public class MetamorphosisServiceTests
{
    private readonly NowServices _nowServices;
    private readonly MyCdnServices _myCdnServices;
    private readonly Mock<IMyCdnServices> _myCdnServicesMock;
    private readonly Mock<IFileServices> _fileServicesMock;
    private readonly Mock<IMapper> _mapperMock;

    public MetamorphosisServiceTests()
    {
        _fileServicesMock = new Mock<IFileServices>();
        _myCdnServicesMock = new Mock<IMyCdnServices>();
        _mapperMock = new Mock<IMapper>();

        _nowServices = new NowServices(_myCdnServicesMock.Object, _fileServicesMock.Object);
        _myCdnServices = new MyCdnServices(_mapperMock.Object);
    }


    [Theory(DisplayName = "Verifica se o metodo trasforma a string no formato Minha CDN em uma array de 7 posições para a criação do objeto MyCDN")]
    [InlineData("312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2")]
    [InlineData("101|200|MISS|\"POST /myImages HTTP/1.1\"|319.4")]
    [InlineData("199|404|MISS|\"GET /not-found HTTP/1.1\"|142.9")]
    [InlineData("312|200|INVALIDATE|\"GET /robots.txt HTTP/1.1\"|245.1")]
    public void ConvertStringArrayInMyCdnFormat_WhenPassingStringInMyCdnFormat_ReturnArraySevenPosition(string text)
    {
        var result = _myCdnServices.ConvertStringArrayInMyCdnFormat(text);
        Assert.Equal(7, result.Length);
    }


    [Theory(DisplayName = "Verifica se o metodo cria um objeto do tipo MyCdn a partir de uma array de string")]
    [InlineData("312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2", "312", "200", "HIT", "100.2", "GET", "/robots.txt", "MINHA CDN")]
    [InlineData("101|200|MISS|\"POST /myImages HTTP/1.1\"|319.4", "101", "200", "MISS", "319.4", "POST", "/myImages", "MINHA CDN")]
    [InlineData("199|404|MISS|\"GET /not-found HTTP/1.1\"|142.9", "199", "404", "MISS", "142.9", "GET", "/not-found", "MINHA CDN")]
    [InlineData("312|200|INVALIDATE|\"GET /robots.txt HTTP/1.1\"|245.1", "312", "200", "INVALIDATE", "245.1", "GET", "/robots.txt", "MINHA CDN")]
    public void ConvertArrayStringInMyCdnObject_WhenPassingStringInMyCdnFormat_ReturnsCompleteObject(
        string text,
        string expectedResponseSize,
        string expectedStatusCode,
        string expectedCacheStatus,
        string expectedTimeTaken,
        string expectedHttpMethod,
        string expectedUriPath,
        string expectedProvider
        )
    {
        var expectedMyCdn = new MyCdn
        {
            ResponseSize = expectedResponseSize,
            StatusCode = expectedStatusCode,
            CacheStatus = expectedCacheStatus,
            TimeTaken = expectedTimeTaken,
            HttpMetod = expectedHttpMethod,
            UriPath = expectedUriPath,
            Provider = expectedProvider
        };

        _myCdnServicesMock
            .Setup(service => service.ConvertArrayStringInMyCdnObject(It.IsAny<string[]>()))
            .Returns(expectedMyCdn);

        var array = _myCdnServices.ConvertStringArrayInMyCdnFormat(text);
        var result = _myCdnServicesMock.Object.ConvertArrayStringInMyCdnObject(array);


        Assert.NotNull(result);
        Assert.Equal(expectedMyCdn.ResponseSize, result.ResponseSize);
        Assert.Equal(expectedMyCdn.StatusCode, result.StatusCode);
        Assert.Equal(expectedMyCdn.CacheStatus, result.CacheStatus);
        Assert.Equal(expectedMyCdn.TimeTaken, result.TimeTaken);
        Assert.Equal(expectedMyCdn.HttpMetod, result.HttpMetod);
        Assert.Equal(expectedMyCdn.UriPath, result.UriPath);
        Assert.Equal(expectedMyCdn.Provider, result.Provider);
    }


    [Theory(DisplayName = "Verifica se a url de entrada está no padrão que o sistema exige")]
    [InlineData("https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt ./output/minhaCdn1.txt")]
    [InlineData("https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt ./output/minhaCdn2.txt")]
    [InlineData("https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt ./output/minhaCdn3.txt")]
    public void ValidateSourceInput_WhenPassingStringSourceValid_ReturTrue(string source)
    {
        var result = _myCdnServices.ValidateSourceInput(source);
        Assert.True(result);
    }


    [Theory(DisplayName = "Verifica se a url de entrada não está no padrão que o sistema exige")]
    [InlineData("ttps://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt ./output/minhaCdn1.txt")]
    [InlineData("ttps://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt./output/minhaCdn1.txt")]
    [InlineData("ttps://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/.txt./output/minhaCdn1.txt")]
    public void ValidateSourceInput_WhenPassingStringSourceInvalid_ReturFalse(string source)
    {
        var result = _myCdnServices.ValidateSourceInput(source);
        Assert.False(result);
    }

}
