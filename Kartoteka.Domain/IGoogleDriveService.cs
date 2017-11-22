using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka.Domain
{
    public interface IGoogleDriveService
    /// интерфейс, связывающий реализацию на уровне DAL (проект GoogleDrive, загрузка файла-отчёта с авторами или книгами в GoogleDrive) 
    /// и уровень бизнес-логики (Domain) через внедрение зависимости этого интерфейса от конкретной реализации
    {
        DriveService Authorization();
        Dictionary<string, string> GetFolders(DriveService _service, Dictionary<string, string> folders);
        File UploadFile(DriveService _service, ExportData _uploadFile);
    }
}
