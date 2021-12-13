using MeterReadings.Common.Data.Models;
using System;

namespace MeterReadings.Service.Models
{
    public class Reading : IReading
    {
        public DateTime DateRecorded { get; set; }
        public int Id { get; set; }
        public int Value { get; set; }
        public int AccountId { get; set; }
        public bool IsValid { get; private set; }

        public Reading() { }

        public Reading(IReading reading)
        {
            DateRecorded = reading.DateRecorded;
            Id = reading.Id;
            Value = reading.Value;
            AccountId = reading.AccountId;
            IsValid = Validate();
        }

        public bool Validate()
        {
            IsValid = ValidateValueProperty(Value) && ValidateAccountIdProperty(AccountId) && ValidateDateRecordedProperty(DateRecorded);
            return IsValid;
        }

        private static bool ValidateValueProperty(int value)
        {
            return value != 0 && value <= 99999;
        }

        private static bool ValidateAccountIdProperty(int accountId)
        {
            return accountId != 0;
        }

        private static bool ValidateDateRecordedProperty(DateTime dateRecorded)
        {
            return DateTime.Compare(dateRecorded, DateTime.MinValue) > 0;
        }
    }
}
