using MeterReadings.Common.Data.Models;
using System;

namespace MeterReadings.Service.Extensions
{
    public static class ReadingExtensions
    {
        public static IReading ParseCsvString(this IReading reading, string csvData, int accountColIndex, int dateRecordedColIndex, int valueColIndex, string delimiter)
        {

            string[] csvParts = csvData.Split(new[] { delimiter }, StringSplitOptions.TrimEntries);

            int accountIdParsed;
            bool accountIdWasParsed = int.TryParse(csvParts[accountColIndex], out accountIdParsed);
            reading.AccountId = accountIdWasParsed ? accountIdParsed : 0;

            DateTime dateRecordedParsed;
            bool dateRecordedWasParsed = DateTime.TryParse(csvParts[dateRecordedColIndex], out dateRecordedParsed);

            if(dateRecordedWasParsed)
            {
                reading.DateRecorded = dateRecordedParsed;
            }

            int valueParsed;
            bool valueWasParsed = int.TryParse(csvParts[valueColIndex], out valueParsed);
            reading.Value = valueWasParsed ? valueParsed : 0;

            return reading;
        }

        public static bool Compare(this IReading reading, IReading toCompare)
        {
            return DateTime.Compare(reading.DateRecorded.Date, toCompare.DateRecorded.Date) == 0
                && reading.Value == toCompare.Value;
        }
    }
}
