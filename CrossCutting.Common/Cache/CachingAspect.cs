using System;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using CrossCutting.Common.Models;

namespace CrossCutting.Common.Cache
{
    public class CachingAspect : IInterceptor
    {
        private readonly ICacheManager _cacheManager;

        public CachingAspect(ICacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name == "Get")
            {
                var idParameter = (Guid) invocation.Arguments.FirstOrDefault(x => x is Guid);
                
                var cachedValue = this._cacheManager.Get(idParameter);
                if (cachedValue == null)
                {
                    invocation.Proceed();
                    cachedValue = invocation.ReturnValue;

                    this._cacheManager.Add(cachedValue as Document);
                }
                
                invocation.ReturnValue = cachedValue;
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}