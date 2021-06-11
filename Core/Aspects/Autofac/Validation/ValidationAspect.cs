using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using Core.Utilities.Results;
using FluentValidation;
using System;
using System.Linq;

namespace Core.Aspects.Autofac.Validation
{
    public class ValidationAspect : MethodInterception
    {
        private readonly Type _validatorType;
        public ValidationAspect(Type validatorType)
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new ArgumentException("Wrong Validation Type");
            }
            _validatorType = validatorType;
        }
        protected override void OnBefore(IInvocation invocation)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];
            var entities = invocation.Arguments.Where(i => i.GetType() == entityType);
            foreach (var entity in entities)
            {
                var validationFailures = ValidationTool.Validate(validator, entity);

                if (validationFailures != null)
                {
                    isSuccess = false;
                    result = new ErrorResult(validationFailures.ToList()[0].ErrorMessage);
                    break;
                }
            }
        }
    }
}
