using AutoMapper;
using Metamorphosis.App.Intrfaces;
using Metamorphosis.App.Models;

namespace Metamorphosis.App.Services
{
    internal class SourceServices : ISourceServices
    {
        private readonly IMapper _mapper; 

        public SourceServices(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Source SeparateSources(string[] urlSource)
        {
            return _mapper.Map<Source>(urlSource);
        }
    }
}
