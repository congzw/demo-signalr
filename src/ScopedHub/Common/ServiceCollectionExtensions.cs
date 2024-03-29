﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable CheckNamespace

namespace Common
{
    public static class ServiceCollectionExtensions
    {
        //services.AddAllImpl(typeof(ITest<Whatever>), ServiceLifetime.Transient); => OK
        //services.AddAllImpl(typeof(ITest<>), ServiceLifetime.Transient); => K.O. todo

        public static IServiceCollection AddAllImpl<InterfaceType>(this IServiceCollection services,  ServiceLifetime lifetime)
        {
            var parentType = typeof(InterfaceType);
            return AddAllImpl(services, parentType, lifetime);
        }
        public static IServiceCollection AddAllImpl(this IServiceCollection services, Type parentType, ServiceLifetime lifetime)
        {
            var implTypes = GetImplementingTypes(parentType).ToList();
            foreach (var implType in implTypes)
            {
                services.Add(new ServiceDescriptor(parentType, implType, lifetime));
            }
            return services;
        }
        
        private static IEnumerable<Type> _allTypes = null;
        private static IEnumerable<Type> GetAllTypes()
        {
            return _allTypes ?? (_allTypes = AppDomain
                       .CurrentDomain
                       .GetAssemblies()
                       .SelectMany(assembly => assembly.GetTypes()).ToList());
        }

        private static IEnumerable<Type> GetImplementingTypes(Type desiredType)
        {
            return GetAllTypes().Where(type => 
                DoesTypeSupportInterface(type, desiredType)
                && type.IsInstantiable());
        }

        private static bool DoesTypeSupportInterface(Type testType, Type desiredType)
        {
            if (desiredType.IsAssignableFrom(testType))
                return true;
            if (testType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == desiredType))
                return true;
            return false;
        }
        private static bool IsInstantiable(this Type type)
        {
            return !(type.IsAbstract || type.IsGenericTypeDefinition || type.IsInterface);
        }
    }
}