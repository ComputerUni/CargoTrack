using CargoTrack.DTO.DTOs.EmployeeDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Validators.Employees
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Çalışan Adı Boş Bırakılamaz.")
                .MinimumLength(2).WithMessage("Ad En Az 2 Karakter Olmalıdır.")
                .MaximumLength(50).WithMessage("Ad En Fazla 50 Karakter Olabilir.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Çalışan Soyadı Boş Bırakılamaz.")
                .MinimumLength(2).WithMessage("Soyad En Az 2 Karakter Olmalıdır.")
                .MaximumLength(50).WithMessage("Soyad En Fazla 50 Karakter Olabilir.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Çalışan Telefonu Boş Bırakılamaz.")
                .Length(11).WithMessage("Telefon Numarası 11 Haneden Oluşmalıdır.")
                .Matches(@"^[0-9]+$").WithMessage("Telefon Numarası Sadece Rakamlardan Oluşmalıdır.");

            RuleFor(x => x.BranchId).NotEmpty().WithMessage("Çalışan Şubesi Boş Bırakılamaz.");
        }
    }
}
