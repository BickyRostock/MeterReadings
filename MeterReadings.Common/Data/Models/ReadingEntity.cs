using System;
using System.ComponentModel.DataAnnotations;

namespace MeterReadings.Common.Data.Models
{
    public class ReadingEntity : IReading
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateRecorded { get; set; }

        [Required]
        public int Value { get; set; }

        [Required]
        public int AccountId { get; set; }

        public ReadingEntity() { }

        public ReadingEntity(IReading reading)
        {
            Id = reading.Id;
            DateRecorded = reading.DateRecorded;
            Value = reading.Value;
            AccountId = reading.AccountId;
        }
    }
}
