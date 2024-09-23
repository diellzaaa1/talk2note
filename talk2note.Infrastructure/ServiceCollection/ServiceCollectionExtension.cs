//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Linq;
//using System.Reflection;
//using talk2note.Application.Interfaces;
//using talk2note.Infrastructure.Persistence;

//namespace talk2note.Infrastructure.ServiceCollection
//{
//    public static class ServiceCollectionExtensions
//    {
//        public static IServiceCollection AddAutomatedServices(this IServiceCollection services, params Assembly[] assemblies)
//        {
//            var registeredTypes = new HashSet<Type>();
//            var registeredInterfaces = new HashSet<Type>();

//            foreach (var assembly in assemblies)
//            {
//                var types = assembly.GetTypes();

//                foreach (var type in types)
//                {
//                    var interfaces = type.GetInterfaces();
//                    foreach (var iface in interfaces)
//                    {
//                        if (iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IGenericRepository<>))
//                        {
//                            // Register the generic repository
//                            services.AddScoped(iface, type);
//                            Console.WriteLine($"Registering {type.Name} with interface: {iface.Name}");
//                        }
//                        else if (!iface.IsGenericType)
//                        {
//                            // Register non-generic interfaces
//                            services.AddScoped(iface, type);
//                            Console.WriteLine($"Registering {type.Name} with interface: {iface.Name}");
//                        }
//                    }
//                }
//            }

//            return services;
//        }



//    }

//}
