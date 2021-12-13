using MeterReadings.Common.Data;
using MeterReadings.Common.Data.Models;
using MeterReadings.Common.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeterReadings.IntegrationTests
{
    [TestFixture]
    public class ReadingRepositoryTests
    {
        private IReadingsRepository m_readingsRepository;

        private CommonDBContext m_context;

        [OneTimeSetUp]
        public void OnetimeSetUp()
        {
            DbContextOptionsBuilder<CommonDBContext> optionsBuilder = new DbContextOptionsBuilder<CommonDBContext>();
            optionsBuilder.UseInMemoryDatabase("Tests");

            m_context = new CommonDBContext(optionsBuilder.Options);

            m_readingsRepository = new ReadingsRepository(m_context);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {

        }

        [TearDown]
        public void TearDownOnEach()
        {
            m_context.Readings.RemoveRange(m_context.Readings);
            m_context.SaveChanges();
        }

        [Test]
        public void TestSaveReading()
        {
            IReading reading = new ReadingEntity
            {
                AccountId = 1,
                DateRecorded = new DateTime(2021, 1, 1),
                Value = 123
            };

            IReading result = m_readingsRepository.SaveReadingAsync(reading).Result;

            Assert.That(result.Value, Is.EqualTo(123));
        }

        [Test]
        public void TestSaveReadings()
        {
            IReading reading1 = new ReadingEntity
            {
                AccountId = 1,
                DateRecorded = new DateTime(2021, 1, 1),
                Value = 123
            };

            IReading reading2 = new ReadingEntity
            {
                AccountId = 2,
                DateRecorded = new DateTime(2021, 1, 1),
                Value = 456
            };

            IEnumerable<IReading> readings = new List<IReading> { reading1, reading2 };

            IEnumerable<IReading> result = m_readingsRepository.SaveReadingsAsync(readings).Result;

            Assert.That(result.Count, Is.EqualTo(2));
        }


        [Test]
        public void TestGetReadingById()
        {
            IReading reading = new ReadingEntity
            {
                AccountId = 1,
                DateRecorded = new DateTime(2021, 1, 1),
                Value = 123
            };

            reading = m_readingsRepository.SaveReadingAsync(reading).Result;

            IReading returnedReading = m_readingsRepository.GetReadingByIdAsync(reading.Id).Result;

            Assert.That(reading.Id, Is.EqualTo(returnedReading.Id));
            Assert.That(reading.DateRecorded.ToShortDateString(), Is.EqualTo(returnedReading.DateRecorded.ToShortDateString()));
            Assert.That(reading.AccountId, Is.EqualTo(returnedReading.AccountId));
        }

        [Test]
        public void TestGetReadingsByAccountId()
        {
            IReading reading = new ReadingEntity
            {
                AccountId = 111,
                DateRecorded = new DateTime(2021, 1, 1),
                Value = 123
            };

            reading = m_readingsRepository.SaveReadingAsync(reading).Result;

            IEnumerable<IReading> readings = m_readingsRepository.GetReadingsByAccountIdAsync(reading.AccountId).Result;

            IReading accountReading = readings.FirstOrDefault();

            Assert.That(reading.Id, Is.EqualTo(accountReading.Id));
            Assert.That(reading.DateRecorded.ToShortDateString(), Is.EqualTo(accountReading.DateRecorded.ToShortDateString()));
            Assert.That(reading.AccountId, Is.EqualTo(accountReading.AccountId));
        }

        [Test]
        public void TestDeleteReadingById()
        {
            IReading reading = new ReadingEntity
            {
                AccountId = 1,
                DateRecorded = new DateTime(2021, 1, 1),
                Value = 123
            };

            reading = m_readingsRepository.SaveReadingAsync(reading).Result;

            IReading returnedReading = m_readingsRepository.GetReadingByIdAsync(reading.Id).Result;

            Assert.That(returnedReading, Is.Not.Null);

            int result = m_readingsRepository.DeleteReadingByIdAsync(returnedReading.Id).Result;

            Assert.That(result, Is.EqualTo(1));

            IReading deletedReading = m_readingsRepository.GetReadingByIdAsync(reading.Id).Result;

            Assert.That(deletedReading, Is.Null);
        }

        [Test]
        public void TestReadingGetsUpdatedIfDuplicateAttemptToSaveDuplicateForTheDay()
        {
            IReading firstReading = new ReadingEntity
            {
                AccountId = 1,
                DateRecorded = new DateTime(2021, 1, 1, 9, 30, 0),
                Value = 123
            };

            IReading secondReading = new ReadingEntity
            {
                AccountId = 1,
                DateRecorded = new DateTime(2021, 1, 1, 11, 30, 0),
                Value = 123
            };

            firstReading = m_readingsRepository.SaveReadingAsync(firstReading).Result;

            Assert.That(firstReading.Id, Is.Not.EqualTo(0));

            secondReading = m_readingsRepository.SaveReadingAsync(secondReading).Result;

            Assert.That(secondReading.Id, Is.EqualTo(firstReading.Id));

            IReading updatedReading = m_readingsRepository.GetReadingByIdAsync(firstReading.Id).Result;

            Assert.That(updatedReading.Value, Is.EqualTo(secondReading.Value));
        }

        [Test]
        public void TestSavingMultipleReadingsGetsUpdatedIfDuplicateAttemptToSaveDuplicateForTheDay()
        {
            IReading reading1 = new ReadingEntity
            {
                AccountId = 1,
                DateRecorded = new DateTime(2021, 1, 1),
                Value = 123
            };

            IReading reading2Culprit = new ReadingEntity
            {
                AccountId = 1,
                DateRecorded = new DateTime(2021, 1, 2, 9, 30, 0),
                Value = 456
            };

            IEnumerable<IReading> readingsFirstIteration = new List<IReading> { reading1, reading2Culprit };

            _ = m_readingsRepository.SaveReadingsAsync(readingsFirstIteration).Result;

            IReading reading3 = new ReadingEntity
            {
                AccountId = 1,
                DateRecorded = new DateTime(2021, 1, 3),
                Value = 678
            };

            IReading reading4Culprit = new ReadingEntity
            {
                AccountId = 1,
                DateRecorded = new DateTime(2021, 1, 2, 11, 30, 0),
                Value = 456
            };

            IEnumerable<IReading> readingsSecondIteration = new List<IReading> { reading3, reading4Culprit };

            _ = m_readingsRepository.SaveReadingsAsync(readingsSecondIteration).Result;

            IEnumerable<IReading> accountReadings = m_readingsRepository.GetReadingsByAccountIdAsync(1).Result;

            Assert.That(accountReadings.Count, Is.EqualTo(3));
        }
    }
}
