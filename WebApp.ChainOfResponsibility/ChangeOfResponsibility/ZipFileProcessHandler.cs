using System.IO.Compression;

namespace WebApp.ChainOfResponsibility.ChangeOfResponsibility
{
    public class ZipFileProcessHandler<T> : ProcessHandler
    {

        public override object handle(object request)
        {
            var excelMemoryStream = request as MemoryStream;

            excelMemoryStream.Position = 0;


            using (var zipStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    var zipArchiveEntry = zipArchive.CreateEntry($"{typeof(T).Name}.xlsx"); 

                    using (var entryStream = zipArchiveEntry.Open())
                    {
                        excelMemoryStream.CopyTo(entryStream);
                    }
                }

                return base.handle(zipStream);

            }
        }
    }
}
