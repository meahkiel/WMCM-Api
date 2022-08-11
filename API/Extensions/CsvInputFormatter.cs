using Application.Contacts;
using Application.DTO;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace API.Extensions
{
    public class CsvInputFormatter : TextInputFormatter
    {
        public CsvInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/octet-stream"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);

        }


        protected override bool CanReadType(System.Type type)
        {
            return type == typeof(List<ContactFormDTO>);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            
            var httpContext = context.HttpContext;
            var serviceProvider = httpContext.RequestServices;
            var logger = serviceProvider.GetRequiredService<ILogger<CsvInputFormatter>>();
            
            try
            {

                using var reader = new StreamReader(httpContext.Request.Body,encoding);
                List<ContactFormDTO> contacts = ReadStream(await reader.ReadToEndAsync(),context,logger);
                
                return await InputFormatterResult.SuccessAsync(contacts);

            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                throw new Exception(ex.Message,ex);
            }
        }


        private static List<ContactFormDTO> ReadStream(string contents, InputFormatterContext context,ILogger logger)
        {
            List<ContactFormDTO> contacts = new List<ContactFormDTO>();
            var splitRows = contents.Split($"\n");
            try
            {
                for (var i = 0; i < splitRows.Length; i++)
                {
                    if (i == 0 || string.IsNullOrEmpty(splitRows[i])) continue;
                    var contact = ReadContact(splitRows[i]);
                    if(contact != null)
                        contacts.Add(contact);
                }
                
            }
            catch (Exception ex)
            {
                context.ModelState.TryAddModelError(context.ModelName, ex.Message);
                logger.LogError("Read failed: nameLine = {contacts}", contacts);
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
                EmailAddress = splitColumns[6],
            };

            return contact;
        }
      
    }
}
