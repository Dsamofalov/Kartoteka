using Kartoteka.Domain;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewKartoteka.Model.NLogRealization
{
    public class NLogLoggingService : ILoggerService
    {
        private Logger _logger;

        public NLogLoggingService()
        {
             _logger = LogManager.GetLogger("NLog");
        }

        public void LogTrace(string message)
        {
            _logger.Trace(message);
        }
        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }
        public void LogInfo(string message)
        {
            var sf = new StackFrame(1);
            var method = sf.GetMethod();

            //здесь лучше наверное задавать парамет callsite, тогда можно редактировать шаблон сообщений в конфиге nlog-а
            _logger.Info($"{method.DeclaringType.FullName}.{method.Name}: {message}");
        }
        public void LogWarn(string message)
        {
            _logger.Warn(message);
        }
        public void LogError(string message)
        {
            _logger.Error(message);
        }
        public void LogFatal(string message)
        {
            _logger.Fatal(message);
        }
    }
}
