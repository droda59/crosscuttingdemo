using Autofac;
using CrossCutting.Web.Factories;

namespace CrossCutting.Web
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserFactory>()
                .As<IUserFactory>()
                .SingleInstance()
//                .EnableInterfaceInterceptors()
//                .InterceptedBy(typeof(LoggerInterceptor))
                ;
        }
    }
}