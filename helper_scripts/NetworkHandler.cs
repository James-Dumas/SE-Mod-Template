using System;
using System.Collections.Generic;
using Sandbox.ModAPI;

namespace dev.jamac.AtmosphericDragSupersonic
{
    /// <summary>
    /// A helper class for writing network code.
    /// </summary>
    public class NetworkHandler
    {
        private Dictionary<ushort, HashSet<Action<ushort, byte[], ulong, bool>>> registeredActions;

        /// <summary>
        /// The NetworkSessionType corresponding to the session this handler is attached to.
        /// </summary>
        public NetworkSessionType SessionType { get; private set; }

        public NetworkHandler()
        {
            registeredActions = new Dictionary<ushort, HashSet<Action<ushort, byte[], ulong, bool>>>();

            if(MyAPIGateway.Multiplayer.MultiplayerActive) 
            {
                if(MyAPIGateway.Utilities.IsDedicated)
                {
                    SessionType = NetworkSessionType.SERVER;
                }
                else if(MyAPIGateway.Multiplayer.IsServer)
                {
                    SessionType = NetworkSessionType.BOTH;
                }
                else
                {
                    SessionType = NetworkSessionType.CLIENT;
                }
            }
            else
            {
                SessionType = NetworkSessionType.BOTH;
            }
        }

        /// <summary>
        /// Registers a new action that will be invoked when a message with the given ID is received.
        /// The action will only be registered if the current NetworkSessionType matches the target NetworkSessionType.
        /// (A target NetworkSessionType of BOTH will match any session.)
        /// </summary>
        /// <param name="id">The ID to register the action for.</param>
        /// <param name="action">The action to invoke when a message with the given ID is received.</param>
        /// <param name="target">The session type that will invoke the action.</param>
        public void RegisterAction(ushort id, Action<ushort, byte[], ulong, bool> action, NetworkSessionType target)
        {
            if(SessionType.Matches(target))
            {
                MyAPIGateway.Multiplayer.RegisterSecureMessageHandler(id, action);

                if(!registeredActions.ContainsKey(id))
                {
                    registeredActions.Add(id, new HashSet<Action<ushort, byte[], ulong, bool>>());
                }

                registeredActions[id].Add(action);
            }
        }

        /// <summary>
        /// Unregisters a specific action associated with the given ID.
        /// </summary>
        /// <param name="id">The ID to unregister the action for.</param>
        /// <param name="action">The action to unregister.</param>
        public void UnregisterAction(ushort id, Action<ushort, byte[], ulong, bool> action)
        {
            if(registeredActions.ContainsKey(id))
            {
                if(registeredActions[id].Contains(action))
                {
                    MyAPIGateway.Multiplayer.UnregisterSecureMessageHandler(id, action);

                    registeredActions[id].Remove(action);

                    if(registeredActions[id].Count == 0)
                    {
                        registeredActions.Remove(id);
                    }
                }
            }
        }

        /// <summary>
        /// Unregisters all actions associated with the given ID.
        /// </summary>
        /// <param name="id">The ID to unregister the actions for.</param>
        public void UnregisterActions(ushort id)
        {
            if(registeredActions.ContainsKey(id))
            {
                foreach(var action in registeredActions[id])
                {
                    MyAPIGateway.Multiplayer.UnregisterSecureMessageHandler(id, action);
                }

                registeredActions.Remove(id);
            }
        }

        /// <summary>
        /// Unregisters all actions.
        /// </summary>
        public void UnregisterAll()
        {
            foreach(var action in registeredActions)
            {
                foreach(var a in action.Value)
                {
                    MyAPIGateway.Multiplayer.UnregisterSecureMessageHandler(action.Key, a);
                }
            }

            registeredActions.Clear();
        }

        /// <summary>
        /// Invoke the actions associated with the given ID on a specific session.
        /// Passes the message ID, the given data, and the ID of the current session to the actions.
        /// </summary>
        /// <param name="id">The ID of the message to invoke the actions for.</param>
        /// <param name="data">The data to pass to the actions.</param>
        /// <param name="recipient">The ID of the session to invoke the actions on. By default, the server's ID will be used.</param>
        public void InvokeActions(ushort id, byte[] data, ulong recipient = 0)
        {
            if(recipient == 0)
            {
                recipient = MyAPIGateway.Multiplayer.ServerId;
            }

            if(MyAPIGateway.Multiplayer.MyId == recipient)
            {
                if(registeredActions.ContainsKey(id))
                {
                    foreach(var action in registeredActions[id])
                    {
                        action(id, data, MyAPIGateway.Multiplayer.MyId, true);
                    }
                }
            }
            else
            {
                MyAPIGateway.Multiplayer.SendMessageTo(id, data, recipient, true);
            }
        }
    }

    /// <summary>
    /// Describes a type of network session.
    /// SERVER corresponds to a dedicated server, CLIENT corresponds to a non-hosting multiplayer client,
    /// and BOTH corresponds to singleplayer or the hosting client of a multiplayer session.
    /// </summary>
    public enum NetworkSessionType
    {
        SERVER,
        CLIENT,
        BOTH,
    }

    public static class NetworkSessionTypeExtensions
    {
        /// <summary>
        /// Returns true if the session is a server.
        /// </summary>
        public static bool IsServer(this NetworkSessionType sessionType)
        {
            return sessionType == NetworkSessionType.SERVER || sessionType == NetworkSessionType.BOTH;
        }

        /// <summary>
        /// Returns true if the session is a client.
        /// </summary>
        public static bool IsClient(this NetworkSessionType sessionType)
        {
            return sessionType == NetworkSessionType.CLIENT || sessionType == NetworkSessionType.BOTH;
        }

        /// <summary>
        /// Returns true if both sessions are servers or both are clients.
        /// </summary>
        public static bool Matches(this NetworkSessionType sessionType, NetworkSessionType other)
        {
            return sessionType == other || sessionType == NetworkSessionType.BOTH || other == NetworkSessionType.BOTH;
        }
    }
}
