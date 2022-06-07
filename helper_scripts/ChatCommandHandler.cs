using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sandbox.ModAPI;

namespace dev.jamac.AtmosphericDragSupersonic
{
    /// <summary>
    /// A helper class for creating chat commands.
    /// </summary>
    public class ChatCommandHandler
    {
        private static readonly Regex commandNameRegex = new Regex(@"^(?:[a-zA-Z]|[a-zA-Z][a-zA-Z0-9_\-])*[a-zA-Z]$");
        private static readonly Regex splitRegex = new Regex(@"\s+");
        private Dictionary<string, Action<string[]>> commands;

        /// <summary>
        /// The prefix character(s) used for chat commands.
        /// </summary>
        public string CommandPrefix { get; set; } = "/";

        public ChatCommandHandler()
        {
            commands = new Dictionary<string, Action<string[]>>();
            MyAPIGateway.Utilities.MessageEntered += OnMessageEntered;
        }

        private void OnMessageEntered(string messageText, ref bool sendToOthers)
        {
            // check for command prefix
            if(!messageText.StartsWith(CommandPrefix)) { return; }
            messageText = messageText.Substring(CommandPrefix.Length);

            // check for valid command name
            if(!commandNameRegex.IsMatch(messageText)) { return; }
            string[] args = splitRegex.Split(messageText.Trim());
            if(commands.ContainsKey(args[0]))
            {
                sendToOthers = false;
                commands[args[0]](args);
            }
        }

        /// <summary>
        /// Registers a new chat command.
        /// Command names can only contain alphanumeric, underscore, or dash characters.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <param name="action">
        /// The action to perform when the command is invoked. 
        /// All provided arguments, including the command name, will be passed to the action as a string array.
        /// </param>
        public void RegisterCommand(string name, Action<string[]> action)
        {
            if(!commandNameRegex.IsMatch(name))
            {
                throw new ArgumentException($"'{name}' is not a valid command name. Command names can only contain alphanumeric, underscore, or dash characters.");
            }

            name = name.ToLower();

            if(commands.ContainsKey(name))
            {
                throw new ArgumentException($"A command with the name '{name}' is already registered.");
            }

            commands.Add(name, action);
        }

        /// <summary>
        /// Unregisters a chat command.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        public void UnregisterCommand(string name)
        {
            if(!commands.ContainsKey(name))
            {
                throw new ArgumentException($"No command with the name '{name}' is registered.");
            }

            commands.Remove(name);
        }

        /// <summary>
        /// Checks if a command is registered.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        public bool IsCommandRegistered(string name)
        {
            return commands.ContainsKey(name);
        }

        /// <summary>
        /// Unregisters the handler from the MessageEntered event.
        /// Make sure to call this in UnloadData!
        /// </summary>
        public void Stop()
        {
            MyAPIGateway.Utilities.MessageEntered -= OnMessageEntered;
            commands.Clear();
        }
    }
}
