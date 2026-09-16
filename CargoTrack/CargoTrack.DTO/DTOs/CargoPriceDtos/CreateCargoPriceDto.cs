using CargoTrack.Entity.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.CargoPriceDtos
{
    public class CreateCargoPriceDto
    {
        public double MinWeight { get; set; }
        public double MaxWeight { get; set; }
        public double DesiCoefficient { get; set; }
        public CargoType CargoType { get; set; }
        public decimal AdditionalServicePrice { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsIntercity { get; set; }
    }
}
