using System;

namespace MeterReadings.Common.Data.Models
{
    public interface IReading
    {
        DateTime DateRecorded { get; set; }
        int Id { get; set; }
        int Value { get; set; }
        int AccountId { get; set; }
    }
}