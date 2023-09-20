using ApiMultiPartFormData.Exceptions;
using ApiMultiPartFormData.Models;
using ApiMultiPartFormData.Services.Interfaces;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Web;

namespace DesafioApi.ModelBind
{
    public class HttpPostedFileBaseModelBinderService : IModelBinderService
    {
        public Task<object> BuildModelAsync(Type propertyType, object value, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (value == null)
            {
                throw new UnhandledParameterException();
            }
            HttpFileBase httpFileBase = (value as HttpFileBase) ?? throw new UnhandledParameterException();

            byte[] bytes = new byte[httpFileBase.ContentLength];
            httpFileBase.InputStream.Read(bytes, 0, (int)httpFileBase.ContentLength);
            HttpPostedFileBase postedFileBaseFile = new MemoryPostedFile(bytes, httpFileBase.FileName, httpFileBase.ContentType);
            if (propertyType != typeof(HttpPostedFileBase))
            {
                throw new UnhandledParameterException();
            }
            return Task.FromResult((object)postedFileBaseFile);
        }
    }
}
