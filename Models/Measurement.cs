using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace MessageService.Models
{
    public class Measurement : TableEntity
    {
        public string MeasurementId { get; set; }
        public string SensorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public double Value { get; set; }

        public bool IsValid => Value < 200 || Value > 800;
    }
}
