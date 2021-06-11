using Castle.DynamicProxy;
using Core.Utilities.Results;
using System;

namespace Core.Utilities.Interceptors
{
    public abstract class MethodInterception : MethodInterceptionBaseAttribute
    {
        public bool isSuccess;
        public IResult result;
        protected virtual void OnBefore(IInvocation invocation) { }
        protected virtual void OnAfter(IInvocation invocation) { }
        protected virtual void OnException(IInvocation invocation, Exception e) { }
        protected virtual void OnSuccess(IInvocation invocation) { }
        public override void Intercept(IInvocation invocation)
        {
            isSuccess = true;
            OnBefore(invocation);
            if (isSuccess)
            {
                try
                {
                    invocation.Proceed();
                }
                catch (Exception e)
                {
                    isSuccess = false;
                    OnException(invocation, e);
                    throw;
                }
                finally
                {
                    if (isSuccess)
                    {
                        OnSuccess(invocation);
                    }
                }
                OnAfter(invocation);
            } else
            {
                invocation.ReturnValue = result;
            }            
        }
    }
}
