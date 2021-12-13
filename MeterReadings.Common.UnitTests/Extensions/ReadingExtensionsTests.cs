using MeterReadings.Common.Data.Models;
using MeterReadings.Service.Extensions;
using NUnit.Framework;
using System;

namespace MeterReadings.Common.UnitTests.Extensions
{
    [TestFixture]
    public class ReadingExtensionsTests
    {
        [Test]
        public void TestParseCsvString()
        {
            IReading reading = new ReadingEntity();

            string csvData = "2344,22/04/2019 09:24,01002";
            int accountColIndex = 0;
            int dateRecordedColIndex = 1;
            int valueColIndex = 2;
            string delimiter = ",";

            DateTime dateRecorded = Convert.ToDateTime(csvData.Split(delimiter)[dateRecordedColIndex]);

            reading.ParseCsvString(csvData, accountColIndex, dateRecordedColIndex, valueColIndex, delimiter);

            Assert.That(reading.AccountId, Is.EqualTo(2344));
            Assert.That(DateTime.Compare(reading.DateRecorded, dateRecorded), Is.EqualTo(0));
            Assert.That(reading.AccountId, Is.EqualTo(2344));
        }
    }
}
