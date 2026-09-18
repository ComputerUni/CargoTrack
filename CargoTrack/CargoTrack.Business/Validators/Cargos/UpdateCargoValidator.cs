using CargoTrack.DTO.DTOs.CargosDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Validators.Cargos
{
    public class UpdateCargoValidator : AbstractValidator<UpdateCargoDto>
    {
        public UpdateCargoValidator()
        {
            RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage("Ağırlık 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(1000).WithMessage("Ağırlık 1000 kilogramı geçemez.");

            RuleFor(x => x.Length)
                .GreaterThan(0).WithMessage("Uzunluk 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(300).WithMessage("Uzunluk 300 santimetreyi geçemez");

            RuleFor(x => x.Width)
               .GreaterThan(0).WithMessage("Genişlik 0'dan büyük olmalıdır.")
               .LessThanOrEqualTo(300).WithMessage("Genişlik 300 santimetreyi geçemez");

            RuleFor(x => x.Height)
               .GreaterThan(0).WithMessage("Yükseklik 0'dan büyük olmalıdır.")
               .LessThanOrEqualTo(300).WithMessage("Yükseklik 300 santimetreyi geçemez");

            RuleFor(x => x.ReceiverId).NotEmpty().WithMessage("Alıcı seçilmelidir.");
            RuleFor(x => x.OriginBranchId).NotEmpty().WithMessage("Gönderici şube seçilmelidir.");
            RuleFor(x => x.DestinationBranchId).NotEmpty().WithMessage("Varış şubesi seçilmelidir.");
        }
    }
}
