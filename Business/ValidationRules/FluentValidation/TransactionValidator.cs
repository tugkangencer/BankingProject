using Business.Constants;
using Entities.Concrete;
using FluentValidation;
using System;

namespace Business.ValidationRules.FluentValidation
{
    public class TransactionValidator : AbstractValidator<Transaction>
    {
        public TransactionValidator()
        {
            RuleFor(a => a.SenderAccountNumber).GreaterThan(0).WithMessage(Messages.InvalidAccount);
            RuleFor(a => a.ReceiverAccountNumber).GreaterThan(0).WithMessage(Messages.InvalidAccount);
            RuleFor(t => t.Amount).GreaterThan(0).WithMessage(Messages.InvalidAmount).Must(HaveProperPrecision).WithMessage(Messages.BadPrecision);
        }

        private bool HaveProperPrecision(decimal amount)
        {
            return BitConverter.GetBytes(decimal.GetBits(amount)[3])[2] <= 2;
        }
    }
}
