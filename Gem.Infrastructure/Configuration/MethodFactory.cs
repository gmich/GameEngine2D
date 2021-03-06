﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace Gem.Infrastructure.Configuration
{

    /// <summary>
    /// Factory for locating methods.
    /// </summary>
    /// <typeparam name="TClassAttributeType">The type of the class marker attribute.</typeparam>
    /// <typeparam name="TMethodAttributeType">The type of the method marker attribute.</typeparam>
    internal class MethodFactory<TClassAttributeType, TMethodAttributeType> : INamedItemFactory<MethodInfo, MethodInfo>, IFactory
        where TClassAttributeType : Attribute
        where TMethodAttributeType : NameBaseAttribute
    {
        private readonly Dictionary<string, MethodInfo> nameToMethodInfo = new Dictionary<string, MethodInfo>();

        /// <summary>
        /// Gets a collection of all registered items in the factory.
        /// </summary>
        /// <returns>
        /// Sequence of key/value pairs where each key represents the name
        /// of the item and value is the <see cref="MethodInfo"/> of
        /// the item.
        /// </returns>
        public IDictionary<string, MethodInfo> AllRegisteredItems
        {
            get { return this.nameToMethodInfo; }
        }

        /// <summary>
        /// Scans the assembly for classes marked with <typeparamref name="TClassAttributeType"/>
        /// and methods marked with <typeparamref name="TMethodAttributeType"/> and adds them 
        /// to the factory.
        /// </summary>
        /// <param name="types">The types to scan.</param>
        /// <param name="prefix">The prefix to use for names.</param>
        public void ScanTypes(Type[] types, string prefix)
        {
            foreach (Type t in types)
            {
                try
                {
                    this.RegisterType(t, prefix);
                }
                catch (Exception ex)
                {
                    Gem.Infrastructure.Logging.Auditor.Logger.Error("Failed to scan types: {0}. {1}",types,ex);
                }
            }
        }

        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="itemNamePrefix">The item name prefix.</param>
        public void RegisterType(Type type, string itemNamePrefix)
        {
            if (type.IsDefined(typeof(TClassAttributeType), false))
            {
                foreach (MethodInfo mi in type.GetMethods())
                {
                    var methodAttributes = (TMethodAttributeType[])mi.GetCustomAttributes(typeof(TMethodAttributeType), false);
                    foreach (TMethodAttributeType attr in methodAttributes)
                    {
                        this.RegisterDefinition(itemNamePrefix + attr.Name, mi);
                    }
                }
            }
        }

        /// <summary>
        /// Clears contents of the factory.
        /// </summary>
        public void Clear()
        {
            this.nameToMethodInfo.Clear();
        }

        /// <summary>
        /// Registers the definition of a single method.
        /// </summary>
        /// <param name="name">The method name.</param>
        /// <param name="methodInfo">The method info.</param>
        public void RegisterDefinition(string name, MethodInfo methodInfo)
        {
            this.nameToMethodInfo[name] = methodInfo;
        }

        /// <summary>
        /// Tries to retrieve method by name.
        /// </summary>
        /// <param name="name">The method name.</param>
        /// <param name="result">The result.</param>
        /// <returns>A value of <c>true</c> if the method was found, <c>false</c> otherwise.</returns>
        public bool TryCreateInstance(string name, out MethodInfo result)
        {
            return this.nameToMethodInfo.TryGetValue(name, out result);
        }

        /// <summary>
        /// Retrieves method by name.
        /// </summary>
        /// <param name="name">Method name.</param>
        /// <returns>MethodInfo object.</returns>
        public MethodInfo CreateInstance(string name)
        {
            MethodInfo result;

            if (this.TryCreateInstance(name, out result))
            {
                return result;
            }
            //TODO: create an application specific exception
            throw new Exception("Unknown function: '" + name + "'");
        }

        /// <summary>
        /// Tries to get method definition.
        /// </summary>
        /// <param name="name">The method .</param>
        /// <param name="result">The result.</param>
        /// <returns>A value of <c>true</c> if the method was found, <c>false</c> otherwise.</returns>
        public bool TryGetDefinition(string name, out MethodInfo result)
        {
            return this.nameToMethodInfo.TryGetValue(name, out result);
        }
    }
}
