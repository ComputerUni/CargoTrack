using CargoTrack.DTO.DTOs.DeliveryExceptionDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Validators.DeliveryExceptions
{
    public class CreateDeliveryExceptionsValidator : AbstractValidator<CreateDeliveryExceptionDto>
    {
        public CreateDeliveryExceptionsValidator()
        {
            RuleFor(x => x.CargoId).NotEmpty().WithMessage("Kargo Seçilmelidir.");
            RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("Personel Seçilmelidir.");
            RuleFor(x => x.ExceptionReason).NotEmpty().WithMessage("Hata Nedeni Seçilmelidir.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama Boş Bırakılamaz")
                .MinimumLength(10).WithMessage("Açıklama En Az 10 Karakter Olmalıdır");
        }
    }
}
