using ClosedXML.Excel;
using System.Data;

namespace WebApp.Command.Commands
{
    public class ExcelFile<T>
    {
        public readonly List<T> _list;

        public string FileName => $"{typeof(T).Name}.xlsx";

        public string FileType => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public ExcelFile(List<T> list)
        {
            _list = list;
        }

        public MemoryStream Create()
        {
            var wb = new XLWorkbook();

            var ds = new DataSet();

            ds.Tables.Add(GetTable());

            wb.Worksheets.Add(ds);

            var stream = new MemoryStream();

            wb.SaveAs(stream);

            return stream;
        }

        private DataTable GetTable()
        {
            var table = new DataTable();

            var type = typeof(T);

            type.GetProperties().ToList().ForEach(x =>
            {
                table.Columns.Add(x.Name, x.PropertyType);
            });

            _list.ForEach(x =>
            {
                var row = table.NewRow();

                type.GetProperties().ToList().ForEach(y =>
                {
                    row[y.Name] = y.GetValue(x, null);
                });

                table.Rows.Add(row);
            });

            return table;
        }
    }
}
