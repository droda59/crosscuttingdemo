using System.Linq;
using System.Net.Http;
using System.Windows.Input;
using Autofac;
using Autofac.Builder;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using CrossCutting.Common.Cache;
using CrossCutting.Common.Database;
using CrossCutting.Common.Logging;
using CrossCutting.Common.Models;
using Microsoft.Extensions.Logging;
using Module = Autofac.Module;

namespace CrossCutting.Common
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CacheManager>()
                .As<ICacheManager>()
                .SingleInstance();

            // *** Initial ugly repo ***
            builder.RegisterType<UglyUserRepository>()
                .As<IRepository<User>>()
                .SingleInstance();

            // *** Hand-made decorators ***
//            builder.Register(x => new UserRepository().AsCacheRepository().AsLoggingRepository(x))
//                .As<IRepository<User>>()
//                .SingleInstance();

            // *** Autofac decorators ***
//            builder.RegisterType<UserRepository>()
//                .Named<IRepository<User>>("repository")
//                .SingleInstance();
//            
//            builder.RegisterDecorator<IRepository<User>>(
//                (c, inner) => inner.AsCacheRepository().AsLoggingRepository(c),
//                fromKey: "repository");
            
            // *** Autofac generic decorators ***
//            builder.RegisterGeneric(typeof(BaseRepository<>))
//                .Named("repository", typeof(IRepository<>));
//
//            builder.RegisterGenericDecorator(
//                typeof(LoggingRepository<>),
//                typeof(IRepository<>),
//                fromKey: "repository");

            // *** Autofac interceptors ***
//            builder.RegisterAssemblyTypes(this.ThisAssembly)
//                .Where(x => x.Name.EndsWith("Repository"))
//                .AsImplementedInterfaces()
//                .SingleInstance()
//                .EnableInterfaceInterceptors(new ProxyGenerationOptions(new LoggingMethodGenerationHook()))
//                .InterceptedBy(typeof(LoggingAspect));
//            
//            builder.RegisterAssemblyTypes(this.ThisAssembly)
//                .Where(x => x.Name.EndsWith("Repository"))
//                .AsImplementedInterfaces()
//                .SingleInstance()
//                .EnableInterfaceInterceptors()
//                .InterceptedBy(typeof(LoggingAspect))
//                .InterceptedBy(typeof(CachingAspect));
//
//            builder.Register(c => new LoggingAspect(c.Resolve<ILoggerFactory>()));
//            builder.Register(c => new CachingAspect(c.Resolve<ICacheManager>()));
        }
    }

    public static class DecoratorExtensions
    {
        public static IRepository<T> AsCacheRepository<T>(this IRepository<T> repository) where T : Document
        {
            return new CacheRepository<T>(repository, new CacheManager());
        }
        
        public static IRepository<T> AsLoggingRepository<T>(this IRepository<T> repository, IComponentContext componentContext) where T : Document
        {
            return new LoggingRepository<T>(repository, componentContext.Resolve<ILoggerFactory>());
        }
    }
}