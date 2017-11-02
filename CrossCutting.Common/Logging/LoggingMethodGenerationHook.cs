using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace CrossCutting.Common.Logging
{
    public class LoggingMethodGenerationHook : IProxyGenerationHook
    {
        public void MethodsInspected()
        {
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return Attribute.IsDefined(methodInfo, typeof(LoggingDescriptionAttribute));
        }
    }
}