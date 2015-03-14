﻿using Gem.Network.Builders;
using Gem.Network.Cache;
using Gem.Network.Handlers;
using Gem.Network.Repositories;
using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Factories
{
    public sealed class MessageHandlerFactory : IMessageHandlerFactory
    {

        #region Private Properties

        private readonly IMessageHandlerBuilder messageBuilder;
           
        #endregion

        #region Constructor

        public MessageHandlerFactory(IMessageHandlerBuilder messageBuilder)
        {
            this.messageBuilder = messageBuilder;
        }

        #endregion

        #region IMessageHandlerFactorys Implementation

        public Type Create(List<string> propertyTypeNames, string classname, string functionName)
        {
            Guard.That(propertyTypeNames.All(x => x != null), "The propertyTypeNames should not be null");
            Guard.That(classname).IsNotNull();
            Guard.That(functionName).IsNotNull();

            var newHandler = messageBuilder.Build(propertyTypeNames, classname, functionName);

            return newHandler;
        }
       
        #endregion

        #region Equality Comparer for Type[]

        internal class ArrayTypeEquality : EqualityComparer<List<string>>
        {
            public override int GetHashCode(List<string> strings)
            {
                int hash = 17;
                foreach (var str in strings)
                {
                    hash = hash * 31 + str.GetHashCode();
                }
                return hash;
            }

            public override bool Equals(List<string> strings1, List<string> strings2)
            {
                return strings1.SequenceEqual(strings2);
            }
        }

        #endregion
        
    }
}
