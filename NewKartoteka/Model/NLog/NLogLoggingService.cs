using Kartoteka.Domain;
using NLog;
using System;
using System.Collections.Generic;
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

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }
    }
}
