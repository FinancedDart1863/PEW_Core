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
using System.Reflection;
using System.Text.RegularExpressions;

namespace PEWCore.Network
{
    public class PEWNetworkGPSManager
    {

        List<IMyPlayer> connectedPlayers = new List<IMyPlayer>();
        List<IMyIdentity> allHistoryPlayersIdentities = new List<IMyIdentity>();
        List<string> connectedPlayersSteamID = new List<string>();
        string[] reservedGPSExpressions = new string[] { "KOTH_", "System_", "Story_", "HVT_", "Hint_", "Mission_" };
        string reservedHVTExpression = "HVT_";

        /// <summary>
        /// Official system GPS table for GPS markers to be managed across the network to clients.
        /// Key is unique name for GPS (i.e. KothKill1, KothHill2, HVT1, HVT2, etc) managed by the respective subsystems
        /// Tuple is old hash and latest IMyGPS
        /// </summary>
        ///
        public static List<MyTuple<string, int, IMyGps>> systemGPSTable = new List<MyTuple<string, int, IMyGps>>();

        public void initialization()
        {
            allHistoryPlayersIdentities = new List<IMyIdentity>(); //Start cleanly
            MyAPIGateway.Players.GetAllIdentites(allHistoryPlayersIdentities);
            //We clear all players GPS waypoints, regardless of actively connected or having only joined once, at startup
            for (int i = 0; i < allHistoryPlayersIdentities.Count; i++)
            {
                ClientCleanAllSystemGPS(allHistoryPlayersIdentities[i].PlayerId);
            }

            //MyAPIGateway.Utilities.ShowMessage("DBG", "GPSManager init");
        }


        //This runs on an interval
        public void PEWNetworkGPSManagerMain()
        {
            connectedPlayers = new List<IMyPlayer>(); //Clear the connected player IMyPlayer list
            MyAPIGateway.Players.GetPlayers(connectedPlayers); //Populated the IMyPlayer List
            connectedPlayersSteamID = new List<string>(); //Clear the connected player steamID list
            //MyAPIGateway.Utilities.ShowMessage("DBG", "GPSManager. Connected players: " + connectedPlayers.Count.ToString());
            try
            {
                for (int x = 0; x < connectedPlayers.Count; x++)
                {
                    connectedPlayersSteamID.Add(connectedPlayers[x].SteamUserId.ToString());
                }

                //Iterate through each player
                for (int x = 0; x < connectedPlayersSteamID.Count; x++)
                {
                    ClientSyncSystemGPS(connectedPlayers[x].IdentityId);
                }
            }
            catch (Exception e)
            {
                //PEWCoreLogging.Instance.WriteLine("[PEWCore | PEWNetworkGPSManager] Cycle failure");
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

        public void ClientCleanHVTSystemGPS(long clientIdentityId)  //Clear HVT markers
        {
            List<IMyGps> tempGpsList = new List<IMyGps>();
            tempGpsList = MyAPIGateway.Session.GPS.GetGpsList(clientIdentityId);
            for (int x = 0; x < tempGpsList.Count; x++)
            {
                if (Regex.IsMatch(tempGpsList[x].Name, reservedHVTExpression))
                {
                    MyAPIGateway.Session.GPS.RemoveGps(clientIdentityId, tempGpsList[x]);
                    //MyAPIGateway.Utilities.ShowMessage("DBG", "Client clean HVT GPS");
                }
            }
        }

        public void GPSManagerCleanHVTSystemGPS()
        {
            for (int x = 0; x < systemGPSTable.Count; x++)
            {
                if (Regex.IsMatch(systemGPSTable[x].Item3.Name, "HVT_"))
                {
                    systemGPSTable.Remove(systemGPSTable[x]);
                    //MyAPIGateway.Utilities.ShowMessage("DBG", "System clean HVT GPS");
                }
            }
        }
        
        public void GPSManagerCleanSystemGPSByRegex(string regexExp1, string regexExp2)
        {
            if (regexExp2 != null)
            {
                for (int x = 0; x < systemGPSTable.Count; x++)
                {
                    if (Regex.IsMatch(systemGPSTable[x].Item3.Name, regexExp1) && Regex.IsMatch(systemGPSTable[x].Item3.Name, regexExp2))
                    {
                        systemGPSTable.Remove(systemGPSTable[x]);
                        //MyAPIGateway.Utilities.ShowMessage("DBG", "System clean HVT GPS");
                    }
                }
            }
            else
            {
                if (regexExp1 != null)
                {
                    for (int x = 0; x < systemGPSTable.Count; x++)
                    {
                        if (Regex.IsMatch(systemGPSTable[x].Item3.Name, regexExp1))
                        {
                            systemGPSTable.Remove(systemGPSTable[x]);
                            //MyAPIGateway.Utilities.ShowMessage("DBG", "System clean HVT GPS");
                        }
                    }
                }
            }
        }
        public void ClientSyncSystemGPS(long clientIdentityId)
        {
            List<IMyGps> tempGpsList = new List<IMyGps>();
            tempGpsList = MyAPIGateway.Session.GPS.GetGpsList(clientIdentityId);
            List<int> tempGpsListHashes = new List<int>();

            for (int i = 0; i < tempGpsList.Count; i++)
            {
                tempGpsListHashes.Add(tempGpsList[i].Hash);
            }

            //Iterate through a players GPS markers. For regexed markers, update them if they're outdated. Leave them alone if they're updated. Delete them if neither.
            for (int x = 0; x < tempGpsList.Count; x++)
            {
                if (reservedGPSExpressions.Any(tempGpsList[x].Name.Contains))
                {
                    bool recentOrCurrentGPS = false;
                    for (int z = 0; z < systemGPSTable.Count; z++)
                    {
                        if (tempGpsListHashes[x] == systemGPSTable[z].Item2) //GPS marker is outdated (it has the old hash of a system GPS). Update it
                        {
                            recentOrCurrentGPS = true;
                            IMyGps tempGPS = MyAPIGateway.Session.GPS.Create(tempGpsList[x].Name, tempGpsList[x].Description, tempGpsList[x].Coords, tempGpsList[x].ShowOnHud);
                            tempGPS.UpdateHash();
                            tempGPS.Coords = systemGPSTable[z].Item3.Coords;
                            tempGPS.Name = systemGPSTable[z].Item3.Name;
                            tempGPS.Description = systemGPSTable[z].Item3.Description;
                            tempGPS.GPSColor = systemGPSTable[z].Item3.GPSColor;
                            tempGPS.ShowOnHud = systemGPSTable[z].Item3.ShowOnHud;
                            MyAPIGateway.Session.GPS.ModifyGps(clientIdentityId, tempGPS);
                         }
                        else
                        {
                            if (tempGpsListHashes[x] == systemGPSTable[z].Item3.Hash) //GPS marker is up to date
                            {
                                recentOrCurrentGPS = true;
                            }
                        }
                    }
                    if (!recentOrCurrentGPS) //This "system" GPS isn't an outdated or current GPS marker. Delete it
                    {
                        MyAPIGateway.Session.GPS.RemoveGps(clientIdentityId, tempGpsList[x]);
                    }
                }
            }

            //Iterate through the system GPS markers. Add any markers to player if they don't have them
            for (int u = 0; u < systemGPSTable.Count; u++)
            {
                bool playerHasGPSMarker = false;
                for (int x = 0; x < tempGpsList.Count; x++)
                {
                    if (tempGpsListHashes[x] == systemGPSTable[u].Item3.Hash)
                    {
                        playerHasGPSMarker = true;
                    }
                }
                if (!playerHasGPSMarker)
                {
                    MyAPIGateway.Session.GPS.AddGps(clientIdentityId, systemGPSTable[u].Item3);
                }
            }
        }

        //Add or update an existing system GPS marker
        public void GPSManagerAddOrUpdateSystemGPS(string systemGPSName, IMyGps gpsMarker)
        {
            bool newSystemGPSMarker = true;
            for (int u = 0; u < systemGPSTable.Count; u++)
            {
                if (systemGPSTable[u].Item1 == systemGPSName)
                {
                    newSystemGPSMarker = false;
                    systemGPSTable[u] = new MyTuple<string, int, IMyGps>(systemGPSTable[u].Item1, systemGPSTable[u].Item3.Hash, gpsMarker);
                }
            }
            if (newSystemGPSMarker)
            {
                systemGPSTable.Add(new MyTuple<string, int, IMyGps>(systemGPSName, gpsMarker.Hash, gpsMarker));
            }
        }

        //Add or update an existing system GPS marker
        public void GPSManagerAddSystemGPS(string systemGPSName, IMyGps gpsMarker)
        {
                systemGPSTable.Add(new MyTuple<string, int, IMyGps>(systemGPSName, gpsMarker.Hash, gpsMarker));
        }


        public void GPSManagerRemoveSystemGPS(string systemGPSName)
        {
            for (int u = 0; u < systemGPSTable.Count; u++)
            {
                if (systemGPSTable[u].Item1 == systemGPSName)
                {
                    systemGPSTable.RemoveAt(u);
                }
            }
        }

        public bool GPSManagerHasSystemGPS(string systemGPSName)
        {
            bool hasSystemGPS = false;
            for (int u = 0; u < systemGPSTable.Count; u++)
            {
                if (systemGPSTable[u].Item1 == systemGPSName)
                {
                    hasSystemGPS= true;
                }
            }
            return hasSystemGPS;
        }

        public List<MyTuple<string, int, IMyGps>> GPSManagerSystemGPSTable()
        {
            return systemGPSTable;
        }
    }
}
