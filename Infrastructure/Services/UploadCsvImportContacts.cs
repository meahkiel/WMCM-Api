using Application.DTO;
using Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Application.Contacts;
using MediatR;
using Application.Core;

namespace Infrastructure.Services
{
    public class UploadCsvImportContacts : IUploadCsvImportContacts
    {
        private readonly IMediator _mediator;

        public UploadCsvImportContacts(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<Result<Unit>> UploadCsvFile(UploadCsvFileRequest request,string path)
        {

            try
            {
                if (request.File.ContentType != "text/csv")
                {
                    throw new Exception("Media Not Supported");
                }

                await CopyFile(request, path);

                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {

                    using var reader = new StreamReader(stream, encoding: System.Text.Encoding.UTF8);

                    List<ContactFormDTO> contacts = ReadStream(await reader.ReadToEndAsync());

                    await _mediator.Send(new Import.Command { Entries = contacts });

                    return Result<Unit>.Success(Unit.Value);
                }
            }
            catch (Exception ex)
            {
                return Result<Unit>.Failure(ex.Message);
               
            }
          
        }

        private static async Task CopyFile(UploadCsvFileRequest request, string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }
        }

        private List<ContactFormDTO> ReadStream(string contents)
        {
            List<ContactFormDTO> contacts = new List<ContactFormDTO>();
            var splitRows = contents.Split($"\n");
            try
            {
                for (var i = 0; i < splitRows.Length; i++)
                {
                    if (i == 0 || string.IsNullOrEmpty(splitRows[i])) continue;
                    var contact = ReadContact(splitRows[i]);
                    if (contact != null)
                        contacts.Add(contact);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return contacts;

        }

        private static ContactFormDTO ReadContact(string content)
        {
            var splitColumns = content.Split(",".ToCharArray());
            var contact = new ContactFormDTO
            {
                Title = splitColumns[0],
                FirstName = splitColumns[1],
                MiddleName = splitColumns[2],
                LastName = splitColumns[3],
                Gender = splitColumns[4],
                MobileNo = splitColumns[5],
                PrimaryContact = splitColumns[6],
                EmailAddress = splitColumns[7],
                Location = splitColumns[8],
                GroupTag = splitColumns[9]
            };

            return contact;
        }

       
    }
}
