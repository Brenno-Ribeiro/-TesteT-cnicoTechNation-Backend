using Metamorphosis.App;
using Metamorphosis.App.Helpers;
using Metamorphosis.App.Intrfaces;
using Metamorphosis.App.Services;
using Microsoft.Extensions.DependencyInjection;


var serviceProvider = new ServiceCollection()
    .AddAutoMapper(typeof(AutoMapperHelper))
    .AddTransient<IMyCdnServices, MyCdnServices>()
    .AddTransient<INowServices, NowServices>()
    .AddTransient<ISourceServices, SourceServices>()
    .AddTransient<IFileServices, FileServices>()
    .BuildServiceProvider();


var myCdnServices = serviceProvider.GetRequiredService<IMyCdnServices>();
var nowServices = serviceProvider.GetRequiredService<INowServices>();
var sourceServices = serviceProvider.GetRequiredService<ISourceServices>();


await new Startup(myCdnServices, nowServices, sourceServices).Start();