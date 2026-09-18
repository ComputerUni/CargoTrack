using CargoTrack.DTO.DTOs.CityDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Validators.Cities
{
    public class CreateCityValidator : AbstractValidator<CreateCityDto>
    {
        public CreateCityValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Şehir Adı Boş Bırakılamaz")
                .MinimumLength(3).WithMessage("Şehir Adı En Az 3 Karakterden Oluşmalıdır");
        }
    }
}
