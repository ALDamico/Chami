using System;
using System.Collections.Generic;
using System.Linq;
using NetOffice.ExcelApi;
using NetOffice;
using Environment = ChamiUI.DataLayer.Entities.Environment;

namespace ChamiUI.BusinessLayer.Exporters
{
    public class EnvironmentExcelExporter:IChamiExporter, IDisposable
    {
        public EnvironmentExcelExporter()
        {
            _environments = new List<Environment>();
        }

        public EnvironmentExcelExporter(ICollection<Environment> environments)
        {
            _environments = new List<Environment>(environments);
        }

        public void AddEnvironment(Environment environment)
        {
            _environments.Add(environment);
        }
        private readonly List<Environment> _environments;
        private Application _excelApplication;
        private Workbook _workbook;
        public void Export(string filename)
        {
            _excelApplication = new Application();
            _workbook = _excelApplication.Workbooks.Add();
            _excelApplication.DisplayAlerts = false;

            Worksheet worksheet = (Worksheet) _workbook.Worksheets.FirstOrDefault();
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

        public void Dispose()
        {
            if (_workbook != null)
            {
                _workbook.Close();
            }
            
            _excelApplication.Quit();
            _excelApplication.Dispose();
        }
    }
}