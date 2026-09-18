using CargoTrack.DTO.DTOs.DeliveryDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Validators.Deliveries
{
    public class VerifyDeliveryValidator : AbstractValidator<VerifyDeliveryDto>
    {
        public VerifyDeliveryValidator()
        {
            RuleFor(x => x.CargoId).NotEmpty().WithMessage("Kargo Seçilmelidir.");
            RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("Personel seçilmelidir.");
            RuleFor(x => x.RecipientName).NotEmpty().WithMessage("Teslim Alan Kişi Boş Bırakılamaz");
            RuleFor(x => x.DeliveryCode).NotEmpty().WithMessage("Teslimat Kodu Boş Bırakılamaz")
                .Length(6).WithMessage("Teslimat Kodu 6 Haneli Olmaldır");
            RuleFor(x => x.Note)
                .NotEmpty().WithMessage("Not Kısmı Boş Bırakılamaz")
                .MinimumLength(5).WithMessage("Not Kısmı En Az 5 Karakterden Oluşmalıdır");

        }
    }
}
