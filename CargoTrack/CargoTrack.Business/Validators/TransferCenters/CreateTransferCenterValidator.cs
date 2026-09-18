using CargoTrack.DTO.DTOs.TransferCenterDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Validators.TransferCenters
{
    public class CreateTransferCenterValidator : AbstractValidator<CreateTransferCenterDto>
    {
        public CreateTransferCenterValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Transfer Merkezi Adı Boş Bırakılamaz.")
                .MinimumLength(2).WithMessage("Transfer Merkezi Adı En Az 2 Karakter Olmalıdır.");

            RuleFor(x => x.CityId).NotEmpty().WithMessage("Şehir Boş Bırakılamaz.");
        }
    }
}
