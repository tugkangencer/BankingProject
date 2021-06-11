using Business.Constants;
using Entities.Concrete;
using FluentValidation;
using System;
using System.Linq;

namespace Business.ValidationRules.FluentValidation
{
    public class AccountValidator : AbstractValidator<Account>
    {
        public AccountValidator()
        {
            RuleFor(a => a.AccountNumber).GreaterThan(0).WithMessage(Messages.InvalidAccount);
            RuleFor(a => a.CurrencyCode).Must(ContainCorrectCurrency).WithMessage(Messages.InvalidCurrency);
            RuleFor(a => a.Balance).GreaterThanOrEqualTo(0).WithMessage(Messages.InvalidBalance).Must(HaveProperPrecision).WithMessage(Messages.BadPrecision);
        }

        private bool ContainCorrectCurrency(string currencyCode)
        {
            if(currencyCode != null)
            {
                currencyCode = currencyCode.ToUpper();
            }
            string[] CurrencyCodes = { "TRY", "USD", "EUR" };
            return CurrencyCodes.Any(c => c == currencyCode);
        }

        private bool HaveProperPrecision(decimal balance)
        {
            return BitConverter.GetBytes(decimal.GetBits(balance)[3])[2] <= 2;
        }
    }
}
