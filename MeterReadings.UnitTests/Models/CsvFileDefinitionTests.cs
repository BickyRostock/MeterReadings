using MeterReadings.Service.Models;
using NUnit.Framework;

namespace MeterReadings.Service.UnitTests.Models
{
    [TestFixture]
    public class CsvFileDefinitionTests
    {
        [Test]
        public void TestValidateReturnsTrue()
        {
            CsvFileDefinition fileDefinition = new CsvFileDefinition
            {
                DocucmentType = "CsV",
                FileContainsHeaders = true,
                Delimiter = ",",
                AccountIdColumnIndex = 0,
                DateRecordedColumnIndex = 1,
                ValueColumnIndex = 2,
            };

            bool valid = fileDefinition.Validate();

            Assert.That(valid, Is.True);
        }

        [Test]
        public void TestValidateReturnsFalseWithInvalidDocumentType()
        {
            CsvFileDefinition fileDefinition = new CsvFileDefinition
            {
                DocucmentType = "pdf",
                FileContainsHeaders = true,
                Delimiter = ",",
                AccountIdColumnIndex = 0,
                DateRecordedColumnIndex = 1,
                ValueColumnIndex = 2,
            };

            bool valid = fileDefinition.Validate();

            Assert.That(valid, Is.False);
        }

        [Test]
        public void TestValidateReturnsFalseWithNoDelimiter()
        {
            CsvFileDefinition fileDefinition = new CsvFileDefinition
            {
                DocucmentType = "csv",
                FileContainsHeaders = true,
                AccountIdColumnIndex = 0,
                DateRecordedColumnIndex = 1,
                ValueColumnIndex = 2,
            };

            bool valid = fileDefinition.Validate();

            Assert.That(valid, Is.False);
        }

        [Test]
        public void TestValidateReturnsFalseWithInvalidColumnIndexes()
        {
            CsvFileDefinition fileDefinition = new CsvFileDefinition
            {
                DocucmentType = "csv",
                FileContainsHeaders = true,
                AccountIdColumnIndex = 0,
                DateRecordedColumnIndex = 10,
                ValueColumnIndex = 2,
            };

            bool valid = fileDefinition.Validate();

            Assert.That(valid, Is.False);
        }
    }
}
