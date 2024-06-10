using Metamorphosis.App.Intrfaces;
using System.Text;

namespace Metamorphosis.App
{
    internal class Startup
    {
        private readonly IMyCdnServices _myCdnServices;
        private readonly INowServices _nowServices;
        private readonly ISourceServices _sourceServices;

        public Startup(IMyCdnServices myCdnServices, INowServices nowServices, ISourceServices sourceServices)
        {
            _myCdnServices = myCdnServices;
            _nowServices = nowServices;
            _sourceServices = sourceServices; 
        }

        public async Task Start()
        {
            var repeatProcess = false;

            do
            {
                Console.Clear();
                Console.Write("Entre com a url de entrada e saida: ");
                var input = Console.ReadLine();

                if (ValidateSourceInput(input))
                {
                    await RunTheLogConversionProcess(input);
                    repeatProcess = ResetProcess();
                }
                else
                {
                    WarningIncorrectSource();
                    repeatProcess = ResetProcess();
                }   

            } while (repeatProcess == true);
        }

        public bool ResetProcess()
        {
            Console.WriteLine();
            Console.WriteLine("Deseja reiniciar o processo? [S] Sim [N] Não");
            var option = Console.ReadLine();
            return option.ToUpper() == "S" ? true : false; 
        }
        public void WarningIncorrectSource()
        {
            var sb = new StringBuilder();

            sb.AppendLine("========================");
            sb.AppendLine(" Atenção: URL incorreta ");
            sb.AppendLine("========================");
            sb.AppendLine("\n");
            sb.AppendLine("Pontos de consideração: ");
            sb.AppendLine("\n");
            sb.AppendLine("1 - É necessário ter um espaço ente a fonte de entrada e a fonte de saida");
            sb.AppendLine("2 - É necessário que as fontes tenham a extensão .txt");
            sb.AppendLine("3 - É necessário que a fonte de entrada começe com \"https\"");

            Console.WriteLine(sb.ToString());
        }

        public bool ValidateSourceInput(string input)
        {   
            return _myCdnServices.ValidateSourceInput(input);
        }

        public async Task RunTheLogConversionProcess(string input)
        {
            var source = _sourceServices.SeparateSources(input.Split(" "));
            var result = await _myCdnServices.GetLogFormatMyCDN(source.InputSource);
            var content = CreateLogContent(result);
            CreateFile(content, source.OutputSource);
            ShowLogResult(content);
        }
        public string CreateLogContent(string result)
        {
            var sb = new StringBuilder();
            var lines = result.Split("\r\n");

            var hearderLog = _nowServices.CreateHeaderLog();
            sb.AppendLine(hearderLog);


            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var formatLine = _nowServices.ConvertMyCdnToNow(line);
                    sb.AppendLine(formatLine);
                }
            }

            return sb.ToString();
        }
        public void CreateFile(string content, string targetPath)
        {
            _nowServices.CreateFileFormatNow(content, targetPath);
        }
        public void ShowLogResult(string content)
        {
            Console.WriteLine();
            Console.WriteLine(content);
            Console.WriteLine();
            Console.WriteLine("O seu arquivo de log no formato \"AGORA\" foi salvo na sua área de trabalho.");
        }
    }
}