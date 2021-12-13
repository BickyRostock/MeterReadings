using System.Linq;

namespace MeterReadings.Service.Models
{
    public class CsvFileDefinition
    {
        public string DocucmentType { get; set; }
        public bool FileContainsHeaders { get; set; }
        public int AccountIdColumnIndex { get; set; }
        public int DateRecordedColumnIndex { get; set; }
        public int ValueColumnIndex { get; set; }
        public string Delimiter { get; set; }

        public bool Validate()
        {
            return ValidateDocumentType() && ValidateDelimiter() && ValidateColumnIndex();
        }

        private bool ValidateDocumentType()
        {
            return !string.IsNullOrEmpty(DocucmentType) && DocucmentType.ToLower() == "csv";
        }

        private bool ValidateDelimiter()
        {
            return !string.IsNullOrEmpty(Delimiter);
        }

        private bool ValidateColumnIndex()
        {
            bool valid = true;

            int[] colIndexes = { AccountIdColumnIndex, DateRecordedColumnIndex, ValueColumnIndex };
            int[] ordered = colIndexes.OrderBy(item => item).ToArray();

            bool isColumnIndexesIncremental = false;

            for (int index = 0; index < ordered.Length - 1; index++)
            {
                isColumnIndexesIncremental = ordered[index + 1] - ordered[index] == 1;
                if (!isColumnIndexesIncremental)
                {
                    valid = false;
                    break;
                }
            }

            return valid;
        }
    }
}