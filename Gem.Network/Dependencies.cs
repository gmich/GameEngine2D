﻿using Autofac;
using Gem.Network.Builders;
using Gem.Network.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Configuration
{
    public class Dependencies
    {
        public static IContainer Container { get; private set; }

        static Dependencies()
        {
            var dependencies = new DependencyReader();
            dependencies.Read();

            var builder = new ContainerBuilder();

            RegisterFactories(builder, dependencies.Factory);

            RegisterBuilders(builder, dependencies.RuntimeBuilder);

            Dependencies.Container = builder.Build();
        }

        private static void RegisterFactories(ContainerBuilder builder,string factory)
        {
            switch (factory)
            {
                default:
                    builder.RegisterType<ClientEventFactory>().As<IEventFactory>().SingleInstance();

                    builder.Register(c => new MessageHandlerFactory(c.Resolve<IMessageHandlerBuilder>()))
                           .As<IMessageHandlerFactory>().SingleInstance();

                    builder.Register(c => new PocoTypeFactory(c.Resolve<IPocoBuilder>()))
                                    .As<IPocoFactory>().SingleInstance();
                    break;
            }
        }

        private static void RegisterBuilders(ContainerBuilder builder, string runtimeBuilder)
        {
            switch (runtimeBuilder)
            {
                default:
                    builder.RegisterType<ReflectionEmitBuilder>()
                           .As<IPocoBuilder>().SingleInstance();

                    builder.RegisterType<CsScriptBuilder>()
                           .As<IMessageHandlerBuilder>().SingleInstance();
                    break;
            }
        }
    }
}
