﻿using Gem.Network.Containers;
using Gem.Network.Messages;
using Gem.Network.Other;
using Gem.Network.Repositories;
using Gem.Network.Server;
using Seterlund.CodeGuard;
using System;

namespace Gem.Network.Managers
{
    public class InfoProvider
             : AbstractContainer<MessageArguments, byte>
    {
        public InfoProvider()
            : base(new FlyweightRepository<MessageArguments, byte>())
        { }

        public IDisposable Add(MessageArguments clientInfo)
        {
            Guard.That(dataRepository).IsTrue(x => x.TotalElements < (int)byte.MaxValue,
            "You have reached the maximum capacity. Consider deregistering");

            //clientInfo.ID = GetUniqueByte();

            return dataRepository.Add(clientInfo.ID, clientInfo);
        }

        public void SubscribeEvent(byte id)
        {
            this[id].EventRaisingclass.SubscribeEvent(GemNetwork.Server);
        }

        public override MessageArguments this[byte id]
        {
            get
            {
                if (dataRepository.HasKey(id))
                {
                    return dataRepository.GetById(id);
                }
                else
                {
                    return null;
                }
            }
        }

        private byte GetUniqueByte()
        {
            byte uniqueByte = (byte)(GemNetwork.InitialId + dataRepository.TotalElements);
            do
            { } while (dataRepository.HasKey(++uniqueByte));

            return uniqueByte;
        }
    }
}
