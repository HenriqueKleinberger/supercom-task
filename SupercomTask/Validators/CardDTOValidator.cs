using FluentValidation;
using SupercomTask.Constants;
using SupercomTask.DTO;

namespace SupercomTask.Validators
{
    public class CardDTOValidator : AbstractValidator<CardDTO>
    {
        public CardDTOValidator()
        {
            RuleFor(x => x.Title).Must(x => x == null || x.Length >= 2);
            RuleFor(x => x.Description).Must(x => x == null || x.Length >= 2);
            RuleFor(x => x.Deadline).Must(BeAValidDate).WithMessage(ErrorMessages.DEADLINE_VALIDATION);
            RuleFor(x => x.Status).NotNull();
        }

        private bool BeAValidDate(DateTime date)
        {
            return date.Date >= DateTime.Now.Date;
        }
    }
}
