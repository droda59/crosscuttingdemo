using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace CrossCutting.Web
{
    public static class IocConfig
    {
        public static IServiceProvider RegisterComponents(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new AutofacModule());
            builder.RegisterModule(new Common.AutofacModule());

            builder.Populate(services);
            
            var container = builder.Build();

            return new AutofacServiceProvider(container);
        }
    }
}