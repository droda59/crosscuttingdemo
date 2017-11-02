using System;

namespace CrossCutting.Common.Cache
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CachingEnabledAttribute : Attribute
    {
    }
}