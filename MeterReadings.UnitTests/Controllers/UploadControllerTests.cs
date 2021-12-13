using MeterReadings.Common.Data.Models;
using MeterReadings.Common.Services;
using MeterReadings.Service.Controllers;
using MeterReadings.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MeterReadings.Service.UnitTests.Controllers
{
    [TestFixture]
    public class UploadControllerTests
    {
        [Test]
        public void TestUpload()
        {
            Mock<IReadingsRepository> repository = new Mock<IReadingsRepository>(MockBehavior.Strict);
            repository.Setup(repo => repo.SaveReadingsAsync(It.IsAny<IEnumerable<IReading>>())).ReturnsAsync(new List<IReading>());

            UploadController controller = new UploadController(repository.Object);

            CsvFileDefinition fileDefinition = new CsvFileDefinition
            {
                DocucmentType = "Csv",
                Delimiter = ",",
                FileContainsHeaders = true,
                AccountIdColumnIndex = 0,
                DateRecordedColumnIndex = 1,
                ValueColumnIndex = 2,
            };

            Mock<IFormFile> file = new Mock<IFormFile>(MockBehavior.Strict);
            file.Setup(file => file.Length).Returns(1);

            string content = TestData.TestData.MeterReadings;
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);
            streamWriter.Write(content);
            streamWriter.Flush();
            memoryStream.Position = 0;

            file.Setup(file => file.OpenReadStream()).Returns(memoryStream);

            IActionResult result = controller.Upload(fileDefinition, file.Object).Result;

            Assert.That(result, Is.InstanceOf(typeof(AcceptedResult)));

            AcceptedResult acceptedResult = (AcceptedResult)result;

            Assert.That(acceptedResult.Value, Is.InstanceOf(typeof(UploadResponse)));

            UploadResponse uploadResponse = (UploadResponse)acceptedResult.Value;

            Assert.That(uploadResponse.InvalidReadings.Count, Is.EqualTo(7));
            Assert.That(uploadResponse.ValidReadings.Count, Is.EqualTo(27));
        }
    }
}
