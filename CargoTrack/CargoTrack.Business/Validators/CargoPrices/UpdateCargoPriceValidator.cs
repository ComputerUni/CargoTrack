using CargoTrack.DTO.DTOs.CargoPriceDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Validators.CargoPrices
{
    public class UpdateCargoPriceValidator : AbstractValidator<UpdateCargoPriceDto>
    {
        public UpdateCargoPriceValidator()
        {
            RuleFor(x => x.CargoType)
                .NotEmpty().WithMessage("Kargo Tipi Boş Bırakılamaz");

            RuleFor(x => x.MinWeight)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum Ağırlık 0'dan Küçük Olamaz.");

            RuleFor(x => x.MaxWeight)
                .GreaterThan(0).WithMessage("Maksimum Ağırlık 0'dan Büyük Olmalıdır.")
                .GreaterThan(x => x.MinWeight).WithMessage("Maksimum Ağırlık Minimum Ağırlıktan Büyük Olmalıdır");

            RuleFor(x => x.DesiCoefficient)
                .GreaterThan(0).WithMessage("Desi Katsayısı 0'dan Büyük Olmalıdır.");

            RuleFor(x => x.BasePrice)
                .GreaterThan(0).WithMessage("Temel Fiyat 0'dan Büyük Olmalıdır.");

            RuleFor(x => x.AdditionalServicePrice)
                .GreaterThanOrEqualTo(0).WithMessage("Ek Hizmet Ücreti Negatif Olamaz.");
        }
    }
}
