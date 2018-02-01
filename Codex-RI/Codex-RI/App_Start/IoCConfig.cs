using Autofac;
using Autofac.Integration.Mvc;
using Neo4jClient;
using Services.Services.Persons;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Codex_RI.App_Start
{
    public class IoCConfig
    {
        public static void CreateAndRegisterContainer()
        {
            var builder = GetContainerBuilder();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }

        private static ContainerBuilder GetContainerBuilder()
        {
            var builder = new ContainerBuilder();

            RegisterMVCControllersWithBuilder(builder);
            RegisterServicesAssemblyWithBuilder(builder);

            return builder;
        }

        private static void RegisterServicesAssemblyWithBuilder(ContainerBuilder builder)
        {
            var servicesAssembly = typeof(PersonService).Assembly;
            var serviceTypes = GetConcreteTypesFromAssembly(servicesAssembly);

            //builder.RegisterType<GraphClient>()
            //    .UsingConstructor(typeof(Uri), typeof(string), typeof(string))
            //    .WithParameters(new List<Parameter>() { new PositionalParameter(0, new Uri(neo4jEndpoint)), new PositionalParameter(1, neo4jUser), new PositionalParameter(2, neo4jPassword) })
            //    .OnActivated(x =>
            //    {
            //        // Glimpse.Neo4jClient.Plugin.RegisterGraphClient(x.Instance);
            //        x.Instance.JsonContractResolver = new CamelCasePropertyNamesContractResolver();
            //        // x.Instance.JsonConverters.Add(new ComplexPropertyConverter());
            //        x.Instance.Connect();
            //    })
            //    .OnRelease(x => x.Dispose())
            //    //.Keyed<GraphClient>("Data")
            //    .InstancePerRequest();

            builder.Register(x =>
            {
                var client = new GraphClient(new Uri(ConfigurationManager.AppSettings["Neo4j.DataSource"]), ConfigurationManager.AppSettings["Neo4j.UserId"], ConfigurationManager.AppSettings["Neo4j.Password"]);
                //client.JsonContractResolver = new CamelCasePropertyNamesContractResolver();
                client.Connect();
                return client;
            })
            .OnRelease(x => x.Dispose())
            .As<GraphClient>()
            .InstancePerRequest();

            var vectorAssembly = typeof(Neo4jClientVector.IoCHandler).Assembly;
            var vectorTypes = GetConcreteTypesFromAssembly(vectorAssembly);

            builder.RegisterTypes(vectorTypes)
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterTypes(serviceTypes)
                   .AsImplementedInterfaces()
                   .InstancePerRequest();
        }



        private static void RegisterMVCControllersWithBuilder(ContainerBuilder builder)
        {
            var controllerAssembly = typeof(MvcApplication).Assembly;
            builder.RegisterControllers(controllerAssembly);
        }

        private static Type[] GetConcreteTypesFromAssembly(Assembly assembly)
        {
            var types = (from t in assembly.GetTypes()
                         where false == t.IsInterface
                         && false == t.IsAbstract
                         && t.GetInterface("I" + t.Name, false) != null
                         select t).ToArray();
            return types;
        }
    }
}