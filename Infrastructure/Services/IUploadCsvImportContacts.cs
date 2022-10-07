using Infrastructure.Core;
using Application.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Core;
using MediatR;

namespace Infrastructure.Services
{
    public interface IUploadCsvImportContacts
    {
        Task<Result<Unit>> UploadCsvFile(UploadCsvFileRequest request,string path);
    }
}
