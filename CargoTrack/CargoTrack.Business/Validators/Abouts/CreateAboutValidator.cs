using CargoTrack.DTO.DTOs.AboutDtos;
using CargoTrack.Entity.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Validators.Abouts
{
    public class CreateAboutValidator : AbstractValidator<CreateAboutDto>
    {
        public CreateAboutValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık boş bırakılamaz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama boş bırakılamaz");
            RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("Görsel Url boş bırakılamaz");
        }
    }
}
