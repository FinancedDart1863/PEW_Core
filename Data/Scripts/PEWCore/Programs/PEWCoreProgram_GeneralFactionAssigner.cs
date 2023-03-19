using System;
using System.Linq;
using System.Text;

using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;


using VRage;
using VRageMath;
using VRage.Common;
using VRage.Definitions;
using VRage.ModAPI;
using VRage.Serialization;
using VRage.Game.Entities;
using VRage.Game.Components;
using VRage.Game;
using VRage.Game.ModAPI;

namespace PEWCore.Programs
{
    internal class PEWCoreProgram_GeneralFactionAssigner
    {
        public static MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>> execute(VRage.ModAPI.IMyEntity entity, string[] instructionSet, PEWCoreVolatileMemory volatileMemory,PEWCoreNonVolatileMemory nonVolatileMemory)
        {

            //We need to be careful in program code sections as failures, either with memory accesses or the logic itself, can crash the server. Everything needs to be in a try block.
            if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[Program | GeneralFactionAssigner] Execution", VRageMath.Color.White); }

            int ISspecifiedExecInterval = 1;
            double logicalZoneRadius = (double)3;
            try
            {
                string programName = instructionSet[0];
                try { ISspecifiedExecInterval = Int32.Parse(instructionSet[1]); } catch (FormatException) { return new MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>>(0, new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(volatileMemory, nonVolatileMemory)); }
                string programDirective = instructionSet[2];
                string faction1Tag = instructionSet[3];
                string faction1ZoneAssignName = instructionSet[4];
                string faction2Tag = instructionSet[5];
                string faction2ZoneAssignName = instructionSet[6];
                string faction3Tag = instructionSet[7];
                string faction3ZoneAssignName = instructionSet[8];

                MyCubeBlock thisLogicalCore = entity as MyCubeBlock;

                //Configure memory abstracts
                string[] thisProgramMemoryStrings = new string[0] {};
                bool[] thisProgramMemoryBools = new bool[0] {};
                int[] thisProgramMemoryInts = new int[0] {};

                //Load program PEWCore memory. Add event trigger functions.
                MyTuple<int, MyTuple<string[], bool[], int[]>> thisProgramNonSharedVolatileMemorySegment = PEWCoreNonVolatileMemory.GetMemorySegment("GeneralFactionAssigner", nonVolatileMemory.NonVolatileMemoryShared);
                if (thisProgramNonSharedVolatileMemorySegment.Item1 == 0)
                {
                    PEWCoreNonVolatileMemory.SetMemorySegment("GeneralFactionAssigner", nonVolatileMemory.NonVolatileMemoryShared, thisProgramMemoryStrings, thisProgramMemoryBools, thisProgramMemoryInts);
                }
                else
                {
                    thisProgramMemoryStrings = thisProgramNonSharedVolatileMemorySegment.Item2.Item1;
                    thisProgramMemoryBools = thisProgramNonSharedVolatileMemorySegment.Item2.Item2;
                    thisProgramMemoryInts = thisProgramNonSharedVolatileMemorySegment.Item2.Item3;
                }

                VRage.ModAPI.IMyEntity parent = entity.GetTopMostParent(); //Topmost parent of logical core is the grid on which it is installed.
                MyCubeGrid grid = parent as MyCubeGrid; //Convert grid entity to grid
                foreach (MyCubeBlock block in grid.GetFatBlocks()) //Walk each block in the grid
                {
                    var currentLogicalZone = block as IMyTerminalBlock;
                    if (currentLogicalZone != null)
                    {
                        if (currentLogicalZone.CustomName.Equals(faction1ZoneAssignName))
                        {
                            Vector3D position = currentLogicalZone.GetPosition();
                            BoundingSphereD sphere = new BoundingSphereD(position, logicalZoneRadius);
                            System.Collections.Generic.List<IMyEntity> entities = MyAPIGateway.Entities.GetEntitiesInSphere(ref sphere);
                            foreach (IMyEntity foundEntity in entities)
                            {
                                // Projection or invalid object
                                if (foundEntity.Physics == null)
                                    continue;

                                // Waypoints and other things that are free of physics
                                if (!foundEntity.Physics.Enabled)
                                    continue;

                                // Ignore our own ship
                                if (foundEntity == parent)
                                    break;

                                if (foundEntity is IMyCharacter)
                                {
                                    //if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[Program | GeneralFactionAssigner] IMyCharacter detected", VRageMath.Color.White); }
                                    IMyCharacter foundPlayer = (IMyCharacter)foundEntity;
                                    if (foundPlayer.IsPlayer)
                                    {
                                        //if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[Program | GeneralFactionAssigner] player IMyCharacter", VRageMath.Color.White); }
                                        IMyFaction Faction1 = MyAPIGateway.Session.Factions.TryGetFactionByTag(faction1Tag);
                                        IMyFaction Faction2 = MyAPIGateway.Session.Factions.TryGetFactionByTag(faction2Tag);
                                        IMyFaction Faction3 = MyAPIGateway.Session.Factions.TryGetFactionByTag(faction3Tag);
                                        if (Faction1 != MyAPIGateway.Session.Factions.TryGetPlayerFaction(foundPlayer.ControllerInfo.ControllingIdentityId))
                                        {
                                           if ((Faction2 != MyAPIGateway.Session.Factions.TryGetPlayerFaction(foundPlayer.ControllerInfo.ControllingIdentityId)) && (Faction3 != MyAPIGateway.Session.Factions.TryGetPlayerFaction(foundPlayer.ControllerInfo.ControllingIdentityId)))
                                           {
                                                MyVisualScriptLogicProvider.KickPlayerFromFaction(foundPlayer.ControllerInfo.ControllingIdentityId);

                                                //Add them to new faction
                                                MyAPIGateway.Session.Factions.SendJoinRequest(Faction1.FactionId, foundPlayer.ControllerInfo.ControllingIdentityId);
                                                MyAPIGateway.Session.Factions.AcceptJoin(Faction1.FactionId, foundPlayer.ControllerInfo.ControllingIdentityId);
                                                //if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[Program | GeneralFactionAssigner] Player added to faction!", VRageMath.Color.White); }
                                            }
                                           else
                                           {
                                                //if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[Program | GeneralFactionAssigner] Player already in another faction!", VRageMath.Color.White); }
                                           }
                                        }
                                        else
                                        {
                                            //if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[Program | GeneralFactionAssigner] Player already in faction!", VRageMath.Color.White); }
                                        }
                                          
                                    }
                                }
                            }
                        }
                        if (currentLogicalZone.CustomName.Equals(faction2ZoneAssignName))
                        {
                            Vector3D position = currentLogicalZone.GetPosition();
                            BoundingSphereD sphere = new BoundingSphereD(position, logicalZoneRadius);
                            System.Collections.Generic.List<IMyEntity> entities = MyAPIGateway.Entities.GetEntitiesInSphere(ref sphere);
                            foreach (IMyEntity foundEntity in entities)
                            {
                                // Projection or invalid object
                                if (foundEntity.Physics == null)
                                    continue;

                                // Waypoints and other things that are free of physics
                                if (!foundEntity.Physics.Enabled)
                                    continue;

                                // Ignore our own ship
                                if (foundEntity == parent)
                                    break;

                                if (foundEntity is IMyCharacter)
                                {
                                    //if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[Program | GeneralFactionAssigner] IMyCharacter detected", VRageMath.Color.White); }
                                    IMyCharacter foundPlayer = (IMyCharacter)foundEntity;
                                    if (foundPlayer.IsPlayer)
                                    {
                                        //if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[Program | GeneralFactionAssigner] player IMyCharacter", VRageMath.Color.White); }
                                        IMyFaction Faction1 = MyAPIGateway.Session.Factions.TryGetFactionByTag(faction1Tag);
                                        IMyFaction Faction2 = MyAPIGateway.Session.Factions.TryGetFactionByTag(faction2Tag);
                                        IMyFaction Faction3 = MyAPIGateway.Session.Factions.TryGetFactionByTag(faction3Tag);
                                        if (Faction2 != MyAPIGateway.Session.Factions.TryGetPlayerFaction(foundPlayer.ControllerInfo.ControllingIdentityId))
                                        {
                                            if ((Faction1 != MyAPIGateway.Session.Factions.TryGetPlayerFaction(foundPlayer.ControllerInfo.ControllingIdentityId)) && (Faction3 != MyAPIGateway.Session.Factions.TryGetPlayerFaction(foundPlayer.ControllerInfo.ControllingIdentityId)))
                                            {
                                                MyVisualScriptLogicProvider.KickPlayerFromFaction(foundPlayer.ControllerInfo.ControllingIdentityId);

                                                //Add them to new faction
                                                MyAPIGateway.Session.Factions.SendJoinRequest(Faction2.FactionId, foundPlayer.ControllerInfo.ControllingIdentityId);
                                                MyAPIGateway.Session.Factions.AcceptJoin(Faction2.FactionId, foundPlayer.ControllerInfo.ControllingIdentityId);
                                                //if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[Program | GeneralFactionAssigner] Player added to faction!", VRageMath.Color.White); }
                                            }
                                            else
                                            {
                                                //if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[Program | GeneralFactionAssigner] Player already in another faction!", VRageMath.Color.White); }
                                            }
                                        }
                                        else
                                        {
                                            //if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[Program | GeneralFactionAssigner] Player already in faction!", VRageMath.Color.White); }
                                        }

                                    }
                                }
                            }
                        }
                        if (currentLogicalZone.CustomName.Equals(faction3ZoneAssignName))
                        {
                            Vector3D position = currentLogicalZone.GetPosition();
                            BoundingSphereD sphere = new BoundingSphereD(position, logicalZoneRadius);
                            System.Collections.Generic.List<IMyEntity> entities = MyAPIGateway.Entities.GetEntitiesInSphere(ref sphere);
                            foreach (IMyEntity foundEntity in entities)
                            {
                                // Projection or invalid object
                                if (foundEntity.Physics == null)
                                    continue;

                                // Waypoints and other things that are free of physics
                                if (!foundEntity.Physics.Enabled)
                                    continue;

                                // Ignore our own ship
                                if (foundEntity == parent)
                                    break;

                                if (foundEntity is IMyCharacter)
                                {
                                    //if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[Program | GeneralFactionAssigner] IMyCharacter detected", VRageMath.Color.White); }
                                    IMyCharacter foundPlayer = (IMyCharacter)foundEntity;
                                    if (foundPlayer.IsPlayer)
                                    {
                                        //if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[Program | GeneralFactionAssigner] player IMyCharacter", VRageMath.Color.White); }
                                        IMyFaction Faction1 = MyAPIGateway.Session.Factions.TryGetFactionByTag(faction1Tag);
                                        IMyFaction Faction2 = MyAPIGateway.Session.Factions.TryGetFactionByTag(faction2Tag);
                                        IMyFaction Faction3 = MyAPIGateway.Session.Factions.TryGetFactionByTag(faction3Tag);
                                        if (Faction3 != MyAPIGateway.Session.Factions.TryGetPlayerFaction(foundPlayer.ControllerInfo.ControllingIdentityId))
                                        {
                                            if ((Faction1 != MyAPIGateway.Session.Factions.TryGetPlayerFaction(foundPlayer.ControllerInfo.ControllingIdentityId)) && (Faction2 != MyAPIGateway.Session.Factions.TryGetPlayerFaction(foundPlayer.ControllerInfo.ControllingIdentityId)))
                                            {
                                                MyVisualScriptLogicProvider.KickPlayerFromFaction(foundPlayer.ControllerInfo.ControllingIdentityId);

                                                //Add them to new faction
                                                MyAPIGateway.Session.Factions.SendJoinRequest(Faction3.FactionId, foundPlayer.ControllerInfo.ControllingIdentityId);
                                                MyAPIGateway.Session.Factions.AcceptJoin(Faction3.FactionId, foundPlayer.ControllerInfo.ControllingIdentityId);
                                                //if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[Program | GeneralFactionAssigner] Player added to faction!", VRageMath.Color.White); }
                                            }
                                            else
                                            {
                                                //if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[Program | GeneralFactionAssigner] Player already in another faction!", VRageMath.Color.White); }
                                            }
                                        }
                                        else
                                        {
                                            //if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[Program | GeneralFactionAssigner] Player already in faction!", VRageMath.Color.White); }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }

                System.Collections.Generic.List<IMyPlayer> connectedPlayers = new System.Collections.Generic.List<IMyPlayer>();
                System.Collections.Generic.List<ulong> connectedPlayersSteamID = new System.Collections.Generic.List<ulong>();
                MyAPIGateway.Players.GetPlayers(connectedPlayers); //Populate the IMyPlayer List
                connectedPlayersSteamID = new System.Collections.Generic.List<ulong>(); //Clear the connected player steamID list

                for (int x = 0; x < connectedPlayers.Count; x++)
                {
                    connectedPlayersSteamID.Add(connectedPlayers[x].SteamUserId);
                }

                for (int x = 0; x < connectedPlayers.Count; x++)
                {
                    /*
                    if (connectedPlayers[x].PromoteLevel == MyPromoteLevel.Admin || connectedPlayers[x].PromoteLevel == MyPromoteLevel.Owner)
                    {
                        MyAPIGateway.Utilities.ShowMessage("DBG", "here1");
                    }
                    else
                    {
                    */
                    
                    IMyFaction currentPlayerFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(connectedPlayers[x].PlayerID);
                    string currentPlayerFactionTag = "";
                    if (currentPlayerFaction != null)
                    {
                        currentPlayerFactionTag = currentPlayerFaction.Tag;
                    }
                    if (!(thisProgramMemoryStrings.Contains(connectedPlayersSteamID[x].ToString())))
                    {
                        //MyAPIGateway.Utilities.ShowMessage("DBG", "PlayerNotRegistered");
                        if (currentPlayerFactionTag == "") {
                            //MyAPIGateway.Utilities.ShowMessage("DBG", "PlayerNotInFaction");
                        }
                        else
                        {
                            //MyAPIGateway.Utilities.ShowMessage("DBG", "PlayerInFaction");
                            if (currentPlayerFactionTag == "SPRT" || currentPlayerFactionTag == "SPID")
                            {
                                MyVisualScriptLogicProvider.KickPlayerFromFaction(connectedPlayers[x].PlayerID);
                            }
                            else
                            {
                                int temp = thisProgramMemoryStrings.Length;
                                Array.Resize(ref thisProgramMemoryStrings, thisProgramMemoryStrings.Length + 2);
                                thisProgramMemoryStrings[temp] = connectedPlayersSteamID[x].ToString();
                                thisProgramMemoryStrings[temp + 1] = currentPlayerFactionTag;
                            }
                        }
                    } 
                    else
                    {
                        //MyAPIGateway.Utilities.ShowMessage("DBG", "PlayerRegistered");
                        int playerRegistryIndex = Array.IndexOf(thisProgramMemoryStrings, connectedPlayersSteamID[x].ToString());
                        string playerRegisteredFactionTag = thisProgramMemoryStrings[(playerRegistryIndex + 1)];
                        if (currentPlayerFactionTag != playerRegisteredFactionTag)
                        {
                            //MyAPIGateway.Utilities.ShowMessage("DBG", "Lock player to faction");
                            if (playerRegisteredFactionTag != "")
                            {
                                MyVisualScriptLogicProvider.KickPlayerFromFaction(connectedPlayers[x].PlayerID);
                            }
                            MyAPIGateway.Session.Factions.SendJoinRequest(MyAPIGateway.Session.Factions.TryGetFactionByTag(playerRegisteredFactionTag).FactionId, connectedPlayers[x].PlayerID);
                            MyAPIGateway.Session.Factions.AcceptJoin(MyAPIGateway.Session.Factions.TryGetFactionByTag(playerRegisteredFactionTag).FactionId, connectedPlayers[x].PlayerID);
                        }
                    //}
                    }
                }

                //We need to write the program memory segment at the end of the core program logic

                PEWCoreNonVolatileMemory.SetMemorySegment("GeneralFactionAssigner", nonVolatileMemory.NonVolatileMemoryShared, thisProgramMemoryStrings, thisProgramMemoryBools, thisProgramMemoryInts);

                return new MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>>(ISspecifiedExecInterval, new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(volatileMemory, nonVolatileMemory));
            }
            catch (Exception ex)
            {
                MyAPIGateway.Utilities.ShowMessage("[GeneralFactionAssigner]", "Execute failure");
                return new MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>>(0, new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(volatileMemory, nonVolatileMemory));
            }
        }
    }
}
