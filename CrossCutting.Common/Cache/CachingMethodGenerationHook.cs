using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace CrossCutting.Common.Cache
{
    public class CachingMethodGenerationHook : IProxyGenerationHook
    {
        public void MethodsInspected()
        {
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return methodInfo.Name.StartsWith("Get")
                || methodInfo.Name.StartsWith("Update")
                || methodInfo.Name.StartsWith("Delete")
                || methodInfo.Name.StartsWith("Create");
            
            return Attribute.IsDefined(methodInfo, typeof(CachingEnabledAttribute));
        }
    }
}