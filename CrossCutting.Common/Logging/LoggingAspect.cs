﻿using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;

namespace CrossCutting.Common.Logging
{
    public class LoggingAspect : IInterceptor
    {
        private readonly ILogger _logger;

        public LoggingAspect(ILoggerFactory loggerFactory)
        {
            this._logger = loggerFactory.CreateLogger("Logging");
        }

        public void Intercept(IInvocation invocation)
        {
            var description = invocation.Method.GetCustomAttribute<LoggingDescriptionAttribute>()?.Description ?? invocation.Method.Name;
            var stringParameters = invocation.Arguments.ToArray();
            if (stringParameters.Any())
            {
                description += " with parameters ";
                
                for (var i = 0; i < stringParameters.Length; i++)
                {
                    description += GetParameterDescription(stringParameters[i], invocation.Method.GetParameters()[i]);
                    if (i < stringParameters.Length - 1)
                    {
                        description += ", ";
                    }
                }
            }
            
            this._logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()}: {description}");

            try
            {
                invocation.Proceed();                
            }
            catch (Exception e)
            {
                this._logger.LogError(e, $"{DateTime.UtcNow.ToLongTimeString()}: !!! Logged Exception: {e.Message}");
                throw;
            }

            var typeName = invocation.Method.ReturnType.Name;
            var returnValue = invocation.ReturnValue;

            if (returnValue is IEnumerable returnValues)
            {
                var objectValueCount = returnValues.Cast<object>().Count();
                typeName = $"{objectValueCount} {returnValue.GetType().GetGenericArguments()[0]?.Name}[]";
                returnValue = string.Empty;
            }
            else
            {
                returnValue = $"<{returnValue}>";
            }

            this._logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()}: Returned from {invocation.TargetType.Name} with {typeName} {returnValue}");
        }

        private static string GetParameterDescription(object stringParameter, ParameterInfo parameterInfo)
        {
            var parameterValue = stringParameter?.ToString();

            if (!(stringParameter is string) && stringParameter is IEnumerable parameterValues)
            {
                parameterValue = string.Join(", ", parameterValues);
            }

            return $"<{parameterInfo.Name}: {parameterValue ?? string.Empty}>";
        }
    }
}