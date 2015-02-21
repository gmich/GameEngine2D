﻿using Gem.Network.Utilities.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Commands
{
    /// <summary>
    /// Command execution delegate
    /// </summary>
    /// <param name="host">Host who will execute the command</param>
    /// <param name="command">Command name</param>
    /// <param name="arguments">Command arguments</param>
    public delegate void CommandExecute(IServer server,string command, IList<string> arguments);

    public interface ICommandExecutioner
    {
        void ExecuteCommand(string command);
    }

    /// <summary>
    /// Ccontains information to run the command
    /// </summary>
    public class CommandInfo
    {
        public CommandInfo(
            string command, string description, CommandExecute callback)
        {
            this.command = command;
            this.description = description;
            this.callback = callback;
        }

        //Command name
        public string command;

        //Description of command
        public string description;

        //Delegate for executing the command
        public CommandExecute callback;
    }
}