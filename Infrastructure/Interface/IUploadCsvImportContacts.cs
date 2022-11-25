using Infrastructure.Core;
using Application.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Application.SeedWorks;

namespace Infrastructure.Interface
{
    public interface IUploadCsvImportContacts
    {
        Task<Result<Unit>> UploadCsvFile(UploadCsvFileRequest request, string path);
    }
}
