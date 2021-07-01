using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetOffice.ExcelApi;
using NetOffice.Exceptions;
using Environment = Chami.Db.Entities.Environment;

namespace ChamiUI.BusinessLayer.Exporters
{
    /// <summary>
    /// Exports a list of <see cref="Environment"/> entities to an Excel spreadsheet.
    /// Requires the NetOffice package.
    /// </summary>
    public class EnvironmentExcelExporter:IChamiExporter, IDisposable
    {
        /// <summary>
        /// Constructs a new <see cref="EnvironmentExcelExporter"/> and inizializes its list of environments to process.
        /// </summary>
        public EnvironmentExcelExporter()
        {
            _environments = new List<Environment>();
        }
        /// <summary>
        /// Constructs a new <see cref="EnvironmentExcelExporter"/> and initializes its list with the elements in the parameter.
        /// </summary>
        /// <param name="environments">The initial <see cref="Environment"/> entities to add to the list of entities to process.</param>
        public EnvironmentExcelExporter(ICollection<Environment> environments)
        {
            _environments = new List<Environment>(environments);
        }

        /// <summary>
        /// Adds a new <see cref="Environment"/> entity to the list of elements to process.
        /// </summary>
        /// <param name="environment">An <see cref="Environment"/> entity to export in the spreadsheet.</param>
        public void AddEnvironment(Environment environment)
        {
            _environments.Add(environment);
        }
        private readonly List<Environment> _environments;
        private Application _excelApplication;
        private Workbook _workbook;

        /// <summary>
        /// Exports a new Excel spreadsheet asynchronously
        /// </summary>
        /// <param name="filename"></param>
        public async Task ExportAsync(string filename)
        {
            _excelApplication = new Application();
            _workbook = _excelApplication.Workbooks.Add();
            _excelApplication.DisplayAlerts = false;

            Worksheet worksheet = (Worksheet) _workbook.Worksheets.FirstOrDefault();
            if (worksheet == null)
            {
                worksheet = (Worksheet) _workbook.Worksheets.Add();
            }
            int sheetNumber = 1;
            var nameDicts = new Dictionary<string, bool>();
            foreach (var environment in _environments)
            {
                
                if (sheetNumber > 1)
                {
                    worksheet = (Worksheet) _workbook.Worksheets.Add();
                }

                nameDicts.TryGetValue(environment.Name, out bool exists);
                var newName = environment.Name;

                if (exists)
                {
                    var guid = Guid.NewGuid().ToString().Split("-", StringSplitOptions.None)[0];
                    newName = $"{newName}_{guid}";
                    
                }
                else
                {
                    worksheet.Name = environment.Name;
                }
                worksheet.Name = newName;
                nameDicts[newName] = true;
                

                var cells = worksheet.Cells;
                PrintHeader(worksheet);

                int i = 2;
                foreach (var environmentVariable in environment.EnvironmentVariables)
                {
                    cells[i, 1].Value = environment.EnvironmentId;
                    cells[i, 2].Value = environment.Name;
                    cells[i, 3].Value = environment.AddedOn;
                    cells[i, 4].Value = environmentVariable.EnvironmentVariableId;
                    cells[i, 5].Value = environmentVariable.Name;
                    cells[i, 6].Value = environmentVariable.Value;
                    cells[i, 7].Value = environmentVariable.AddedOn;
                    i++;
                }

                sheetNumber++;
            }

            await Task.Run(() => _workbook.SaveAs(filename));
            _excelApplication.Quit();
        }
        public void Export(string filename)
        {
            _excelApplication = new Application();
            _workbook = _excelApplication.Workbooks.Add();
            _excelApplication.DisplayAlerts = false;

            Worksheet worksheet = (Worksheet) _workbook.Worksheets.FirstOrDefault();
            if (worksheet == null)
            {
                worksheet = (Worksheet) _workbook.Worksheets.Add();
            }
            int sheetNumber = 1;
            foreach (var environment in _environments)
            {
                
                if (sheetNumber > 1)
                {
                    worksheet = (Worksheet) _workbook.Worksheets.Add();
                }
                worksheet.Name = environment.Name;

                var cells = worksheet.Cells;
                PrintHeader(worksheet);

                int i = 2;
                foreach (var environmentVariable in environment.EnvironmentVariables)
                {
                    cells[i, 1].Value = environment.EnvironmentId;
                    cells[i, 2].Value = environment.Name;
                    cells[i, 3].Value = environment.AddedOn;
                    cells[i, 4].Value = environmentVariable.EnvironmentVariableId;
                    cells[i, 5].Value = environmentVariable.Name;
                    cells[i, 6].Value = environmentVariable.Value;
                    cells[i, 7].Value = environmentVariable.AddedOn;
                    i++;
                }

                sheetNumber++;
            }
            _workbook.SaveAs(filename);
            _excelApplication.Quit();
        }

        protected void PrintHeader(Worksheet worksheet)
        {
            var cells = worksheet.Cells;
            cells[1, 1].Value = "EnvironmentId";
            cells[1, 2].Value = "Name";
            cells[1, 3].Value = "AddedOn";
            cells[1, 4].Value = "EnvironmentVariableId";
            cells[1, 5].Value = "Name";
            cells[1, 6].Value = "Value";
            cells[1, 7].Value = "AddedOn";
        }

        /// <summary>
        /// Implements the <see cref="IDisposable"/> interface.
        /// </summary>
        /// <seealso cref="IDisposable"/>
        public void Dispose()
        {
            if (_workbook != null)
            {
                try
                {
                    _workbook.Close();
                }
                catch (MethodCOMException)
                {
                    // The workbook was already closed
                }
            }
            
            _excelApplication.Quit();
            _excelApplication.Dispose();
        }
    }
}