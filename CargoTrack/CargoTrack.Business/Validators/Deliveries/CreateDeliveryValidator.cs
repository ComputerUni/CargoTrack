using CargoTrack.DTO.DTOs.DeliveryDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Validators.Deliveries
{
    public class CreateDeliveryValidator : AbstractValidator<CreateDeliveryDto>
    {
        public CreateDeliveryValidator()
        {
            RuleFor(x => x.CargoId).NotEmpty().WithMessage("Kargo Seçilmelidir.");
            RuleFor(x => x.RecipientName).NotEmpty().WithMessage("Teslim Alan Kişi Boş Bırakılamaz.");
            RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("Personel Seçilmelidir.");
            RuleFor(x => x.Note)
                .NotEmpty().WithMessage("Not Kısmı Boş Bırakılamaz")
                .MinimumLength(5).WithMessage("Not Kısmı En Az 5 Karakterden Oluşmalıdır");
                
        }
    }
}
