using System.IO;
using System.Web;

namespace DesafioApi.ModelBind
{
    public class MemoryPostedFile : HttpPostedFileBase
    {
        private readonly byte[] fileBytes;

        public MemoryPostedFile(byte[] fileBytes, string fileName = null, string contentType = null)
        {
            this.fileBytes = fileBytes;
            this.FileName = fileName;
            this.InputStream = new MemoryStream(fileBytes);
            this.ContentType = contentType;
        }

        public override int ContentLength => fileBytes.Length;

        public override string ContentType { get; }

        public override string FileName { get; }

        public override Stream InputStream { get; }
    }
}