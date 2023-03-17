//© 2023 FinancedDart
//© 2023 Phobos Engineered Weaponry Group
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.Game;
using VRage.Utils;

namespace PEWCore.Network
{
    /// <summary>
    /// Simple network communication example.
    /// 
    /// Always send to server as clients can't send to eachother directly.
    /// Then decide in the packet if it should be relayed to everyone else (except sender and server of course).
    /// 
    /// Security note:
    ///  SenderId is not reliable and can be altered by sender to claim they're someone else (like an admin).
    ///  If you need senderId to be secure, a more complicated process is required involving sending
    ///   every player a unique random ID and they sending that ID would confirm their identity.
    /// </summary>
    public class PEWNetworkMain
    {
        public readonly ushort allocatedChannel;
        private List<IMyPlayer> playersListTemp = null;

        //Assign channel allocation
        public PEWNetworkMain(ushort channelId)
        {
            allocatedChannel = channelId;
        }

        //Register packet monitoring. Do so for both client server as per execution trace until this point
        public  void Register()
        {
            MyAPIGateway.Multiplayer.RegisterMessageHandler(allocatedChannel, ReceivedPacket);
        }

        //Cleanup properly
        public void Unregister()
        {
            MyAPIGateway.Multiplayer.UnregisterMessageHandler(allocatedChannel, ReceivedPacket);
        }

        private void ReceivedPacket(byte[] rawData) //Process packets recieved by this machine prior to forwarding to HandlePacket
        {
            try
            {
                var packet = MyAPIGateway.Utilities.SerializeFromBinary<PEWNetworkPacket>(rawData);
                HandlePacket(packet, rawData);
            }
            catch (Exception e)
            {
                // Handle packet receive errors
                MyLog.Default.WriteLineAndConsole($"{e.Message}\n{e.StackTrace}");

                if (MyAPIGateway.Session?.Player != null)
                    MyAPIGateway.Utilities.ShowNotification($"[ERROR: {GetType().FullName}: {e.Message} | Send SpaceEngineers.Log to mod author]", 10000, MyFontEnum.Red);
            }
        }

        private void HandlePacket(PEWNetworkPacket packet, byte[] rawData = null)
        {
            var relay = packet.Received();
            if (relay)
                RelayToClients(packet, rawData);
        }

        //Send a packet to the server. Works from clients and server.
        public void SendToServer(PEWNetworkPacket packet)
        {
            if (MyAPIGateway.Multiplayer.IsServer)
            {
                HandlePacket(packet);
                return;
            }
            var bytes = MyAPIGateway.Utilities.SerializeToBinary(packet);
            MyAPIGateway.Multiplayer.SendMessageToServer(allocatedChannel, bytes);
        }

        ///Send a packet to a specific player. Server only.
        public void SendToPlayer(PEWNetworkPacket packet, ulong steamId)
        {
            if (!MyAPIGateway.Multiplayer.IsServer)
                return;

            var bytes = MyAPIGateway.Utilities.SerializeToBinary(packet);
            MyAPIGateway.Multiplayer.SendMessageTo(allocatedChannel, bytes, steamId);
        }

        /// Sends packet (or supplied bytes) to all players except server player and supplied packet's sender. Server only.
        public void RelayToClients(PEWNetworkPacket packet, byte[] rawData = null)
        {
            if (!MyAPIGateway.Multiplayer.IsServer)
                return;

            if (playersListTemp == null)
                playersListTemp = new List<IMyPlayer>(MyAPIGateway.Session.SessionSettings.MaxPlayers);
            else
                playersListTemp.Clear();

            MyAPIGateway.Players.GetPlayers(playersListTemp);
            foreach (var p in playersListTemp)
            {
                if (p.IsBot)
                    continue;

                if (p.SteamUserId == MyAPIGateway.Multiplayer.ServerId)
                    continue;

                if (p.SteamUserId == packet.SenderId)
                    continue;

                if (rawData == null)
                    rawData = MyAPIGateway.Utilities.SerializeToBinary(packet);

                MyAPIGateway.Multiplayer.SendMessageTo(allocatedChannel, rawData, p.SteamUserId);
            }

            playersListTemp.Clear();
        }
    }
}
