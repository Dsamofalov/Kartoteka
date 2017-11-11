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

        //REVIEW: Вот тут - overhead. С одной стороны - всё по SOLID-у, разделение интерфейсов.
        //С другой - проще обойтись одним методом Log(method, message), где method - это enum.
        //Также в логгере было бы неплохо логировать exception.
        //Докапываться не буду, т.к. с академической т.з. всё верно. Но, в реальном проде так лучше не делать.
        public void LogTrace(string message)
        {
            var sf = new StackFrame(1);
            var method = sf.GetMethod();
            _logger.Trace($"{method.DeclaringType.FullName}.{method.Name}: {message}");
        }
        public void LogDebug(string message)
        {
            var sf = new StackFrame(1);
            var method = sf.GetMethod();
            _logger.Debug($"{method.DeclaringType.FullName}.{method.Name}: {message}");
        }
        public void LogInfo(string message)
        {
            var sf = new StackFrame(1);
            var method = sf.GetMethod();
            _logger.Info($"{method.DeclaringType.FullName}.{method.Name}: {message}");
        }
        public void LogWarn(string message)
        {
            var sf = new StackFrame(1);
            var method = sf.GetMethod();
            _logger.Warn($"{method.DeclaringType.FullName}.{method.Name}: {message}");
        }
        public void LogError(string message)
        {
            var sf = new StackFrame(1);
            var method = sf.GetMethod();
            _logger.Error($"{method.DeclaringType.FullName}.{method.Name}: {message}");
        }
        public void LogFatal(string message)
        {
            var sf = new StackFrame(1);
            var method = sf.GetMethod();
            _logger.Fatal($"{method.DeclaringType.FullName}.{method.Name}: {message}");
        }
    }
}
