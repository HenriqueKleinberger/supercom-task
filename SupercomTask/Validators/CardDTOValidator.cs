using FluentValidation;
using SupercomTask.Constants;
using SupercomTask.DTO;

namespace SupercomTask.Validators
{
    public class CardDTOValidator : AbstractValidator<CardDTO>
    {
        public CardDTOValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Title).NotNull();
            RuleFor(x => x.DeadLine).Must(BeAValidDate).WithMessage(ErrorMessages.DEADLINE_VALIDATION);
            RuleFor(x => x.Status).NotNull();
        }

        private bool BeAValidDate(DateTime date)
        {
            return date.Date >= DateTime.Now.Date;
        }
    }
}
