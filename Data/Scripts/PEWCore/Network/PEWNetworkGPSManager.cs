using System.Collections;
using PEWCore.Network;
using Sandbox.ModAPI;
using VRage;
using VRage.Game.Components;
using System.IO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine;
using Sandbox.Game;
using VRage.Game;
using VRage.Game.ModAPI;
using Sandbox.Game.GUI;
using VRage.Game.ObjectBuilders.Definitions;
using ProtoBuf;
using VRage.Serialization;
using VRage.Library.Collections;
using System.Runtime.CompilerServices;
using VRage.ObjectBuilder;
using System.Linq;

namespace PEWCore.Network
{
    public class PEWNetworkGPSManager
    {

        List<IMyPlayer> connectedPlayers = new List<IMyPlayer>();
        List<IMyIdentity> allHistoryPlayersIdentities = new List<IMyIdentity>();
        List<ulong> connectedPlayersSteamID = new List<ulong>();
        string[] reservedGPSExpressions = new string[] { "KOTH_", "SYSTEM_", "Story_", "HVT_", "Hint_" };

        /// <summary>
        /// Official system GPS table for GPS markers to be managed across the network to clients.
        /// /Key is unique name for GPS (i.e. KothKill1, KothHill2, HVT1, HVT2, etc) managed by the respective subsystems
        /// </summary>
        public static Dictionary<string, MyTuple<int, IMyGps>> systemGPSTable = new Dictionary<string, MyTuple<int, IMyGps>>();

        //Networked GPS matrix for managing marker synchronization across the network
        Dictionary<int, Dictionary<ulong, IMyGps>> synchronizedNetworkedGPSTable = new Dictionary<int, Dictionary<ulong, IMyGps>>();


        public void initialization()
        {
            allHistoryPlayersIdentities = new List<IMyIdentity>(); //Start cleanly
            MyAPIGateway.Players.GetAllIdentites(allHistoryPlayersIdentities);
            //We clear all players GPS waypoints, regardless of actively connected or having only joined once, at startup
            for (int i = 0; i < allHistoryPlayersIdentities.Count; i++)
            {
                ClientCleanAllSystemGPS(allHistoryPlayersIdentities[i].PlayerId);
            }
        }
        

        //This runs on an interval
        public void PEWNetworkGPSManagerMain()
        {
            connectedPlayers = new List<IMyPlayer>(); //Clear the connected player IMyPlayer list
            MyAPIGateway.Players.GetPlayers(connectedPlayers); //Populated the IMyPlayer List
            connectedPlayersSteamID = new List<ulong>(); //Clear the connected player steamID list

            try
            {
                for (int x = 0; x < connectedPlayers.Count; x++)
                {
                    connectedPlayersSteamID.Add(connectedPlayers[x].SteamUserId);
                }

                //Iterate through each player
                for (int x = 0; x < connectedPlayersSteamID.Count; x++)
                {
                    //Iterate through each up-to-date system waypoint
                    for (int z = 0; z < systemGPSTable.Count; z++)
                    {
                        //systemGPSTable[z].Item1
                    }
                }
            } catch (Exception e)
            {
                PEWCoreLogging.Instance.WriteLine("[PEWCore | PEWNetworkGPSManager] Cycle failure");
            }
        }

        public void ClientCleanAllSystemGPS(long clientIdentityId)
        {
            List<IMyGps> tempGpsList = new List<IMyGps>();
            tempGpsList = MyAPIGateway.Session.GPS.GetGpsList(clientIdentityId);
            for (int x = 0; x < tempGpsList.Count; x++)
            {
                if (reservedGPSExpressions.Any(tempGpsList[x].Name.Contains)) //We clear all GPS markers from clients GPS list that contain the reserved expressions
                {
                    MyAPIGateway.Session.GPS.RemoveGps(clientIdentityId, tempGpsList[x]);
                }
            }
        }

        public void ClientCleanAllOutdatedSystemGPS(long clientIdentityId)
        {
            List<IMyGps> tempGpsList = new List<IMyGps>();
            tempGpsList = MyAPIGateway.Session.GPS.GetGpsList(clientIdentityId);
            for (int x = 0; x < tempGpsList.Count; x++)
            {
                if (reservedGPSExpressions.Any(tempGpsList[x].Name.Contains)) //We clear all GPS markers from clients GPS list that contain the reserved expressions
                {
                    MyAPIGateway.Session.GPS.RemoveGps(clientIdentityId, tempGpsList[x]);
                }
            }
        }
    }
}
