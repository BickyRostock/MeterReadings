using MeterReadings.Common.Data.Models;
using MeterReadings.Service.Extensions;
using MeterReadings.Service.Models;
using NUnit.Framework;
using System;

namespace MeterReadings.Service.UnitTests.Models
{
    [TestFixture]
    public class ReadingTests
    {
        [Test]
        public void TestDeserialiseThroughExtensionMethod()
        {
            string csvString = "2344,22/04/2019 09:24,01002";

            string[] csvParts = csvString.Split(",");

            string dateTimeStringPart = csvParts[1];

            DateTime dateRecorded = Convert.ToDateTime(dateTimeStringPart);

            IReading reading = new Reading();
            reading.ParseCsvString(csvString, 0, 1, 2, ",");

            Assert.That(reading.AccountId, Is.EqualTo(2344));
            Assert.That(DateTime.Compare(reading.DateRecorded, dateRecorded), Is.EqualTo(0));
            Assert.That(reading.Value, Is.EqualTo(1002));
        }

        [Test]
        public void TestThatReadingIsInvalidwithNoAccountId()
        {
            Reading reading = new Reading
            {
                DateRecorded = new DateTime(2021, 01, 01),
                Value = 123,
            };

            reading.Validate();

            Assert.That(reading.IsValid, Is.False);
        }

        [Test]
        public void TestThatReadingIsInvalidwithNoValue()
        {
            Reading reading = new Reading
            {
                DateRecorded = new DateTime(2021, 01, 01),
                AccountId = 123,
            };

            reading.Validate();

            Assert.That(reading.IsValid, Is.False);
        }

        [Test]
        public void TestThatReadingIsInvalidwithNoDateRecorded()
        {
            Reading reading = new Reading
            {
                AccountId = 123,
                Value = 123,
            };

            reading.Validate();

            Assert.That(reading.IsValid, Is.False);
        }

        [Test]
        public void TestThatReadingIsValid()
        {
            Reading reading = new Reading
            {
                AccountId = 1,
                Value = 123,
                DateRecorded = new DateTime(2021, 01, 01),
            };

            reading.Validate();

            Assert.That(reading.IsValid, Is.True);
        }
    }
}
