using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka.Domain
{
    public interface ILoggerService
    // интерфейс, связывающий реализацию и использование через внедрение зависимости этого интерфейса от конкретной реализации.
    {
        void LogTrace(string message);
         void LogDebug(string message);
        void LogInfo(string message);
        void LogWarn(string message);
        void LogError(string message);
        void LogFatal(string message);
    }
}
