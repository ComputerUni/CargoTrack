using CargoTrack.DTO.DTOs.BranchDtos;
using FluentValidation;

namespace CargoTrack.Business.Validators.Branches
{
    public class UpdateBranchValidator : AbstractValidator<UpdateBranchDto>
    {
        public UpdateBranchValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Şube Adı Boş Bırakılamaz")
                .MinimumLength(3).WithMessage("Şube Adı En Az 3 Karakterden Oluşmalıdır.");

            RuleFor(x => x.CityId).NotEmpty().WithMessage("CityId Boş Bırakılamaz.");
        }
    }
}
