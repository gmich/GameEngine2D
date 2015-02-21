﻿using System;
using Lidgren.Network;
using Gem.Network.Messages;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Gem.Network
{

    public class Client : IClient
    {

        #region Construct / Dispose

        //This is for mocking purposes only
        public Client() { }


        public Client(IPEndPoint serverIP,string serverName,ConnectionDetails connectionDetails = null)
        {
            this.serverIP = serverIP;
            //this.disconnectMessage = disconnectMessage;
           // this.deliveryMethod = deliveryMethod;
           // this.sequenceChannel = sequenceChannel;
            var config = new NetPeerConfiguration(serverName)
            {
                //Port = serverIP.Port
            };
            config.EnableMessageType(NetIncomingMessageType.WarningMessage);
            config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            config.EnableMessageType(NetIncomingMessageType.Error);
            config.EnableMessageType(NetIncomingMessageType.DebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            client = new NetClient(config);
            client.Start();
        }

        private void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    Disconnect();
                }
                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion


        #region Declarations

        private NetClient client;

        /// <summary>
        /// The server's ip
        /// </summary>
        private readonly IPEndPoint serverIP;
    
        private readonly int sequenceChannel;

        private readonly NetDeliveryMethod deliveryMethod;

        private bool isDisposed;

        private readonly string disconnectMessage;

        #endregion
        
        #region IClient Implementation

        /// <summary>
        /// Connect to the server
        /// </summary>
        public void Connect(string serverName, int port)
        {        
            client.Connect(serverIP);
        }

        public NetOutgoingMessage CreateMessage()
        {
            return client.CreateMessage();
        }
             
        public void Disconnect()
        {
            client.Shutdown(disconnectMessage);
        }

        public NetIncomingMessage ReadMessage()
        {
            return client.ReadMessage();
        }

        public void Recycle(NetIncomingMessage im)
        {
            client.Recycle(im);
        }

        //virtual is set for mocking purposes
        public virtual void SendMessage(NetOutgoingMessage msg)
        {
            client.SendMessage(msg, NetDeliveryMethod.ReliableUnordered);
        }
        
        #endregion
        
        public virtual void SendMessage<T>(T message,byte id)
        {
            var msg = client.CreateMessage();
            msg.Write(id);
            MessageSerializer.Encode(message, ref msg);
            client.SendMessage(msg, NetDeliveryMethod.ReliableUnordered);
        }

        public void SendMessage<T>(T message)
        {
            throw new NotImplementedException();
        }
    }
}