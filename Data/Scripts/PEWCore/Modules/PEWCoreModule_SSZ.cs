//© 2023 FinancedDart
//© 2023 Phobos Engineered Weaponry Group
using System;
using System.Collections.Generic;
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
using System.Text.RegularExpressions;

namespace PEWCore.Modules
{
    internal class PEWCoreModule_SSZ
    {
        public static MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>> execute(VRage.ModAPI.IMyEntity entity, string[] instructionSet, PEWCoreVolatileMemory volatileMemory, PEWCoreNonVolatileMemory nonVolatileMemory)
        {
            //We need to be careful in program code sections as failures, either with memory accesses or the logic itself, can crash the server. Everything needs to be in a try block.
            if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[Module | SSZ] Execution", VRageMath.Color.White); }

            int ISspecifiedExecInterval = 1;
            int faction = 0;
            int timeUntilLastExecution = 0; //Time until last execution in seconds
            int currentSafezoneradius = (int)PEWCoreMain.ConfigData.PEWSSZConfig.PEWSSZ_Radius;
            string safeZoneFaction = "";
            string markerColor = "White";

            try
            {
                string programName = instructionSet[0];
                try { ISspecifiedExecInterval = Int32.Parse(instructionSet[1]); } catch (FormatException) { return new MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>>(0, new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(volatileMemory, nonVolatileMemory)); }
                try { faction = Int32.Parse(instructionSet[2]);}catch (FormatException) { return new MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>>(0, new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(volatileMemory, nonVolatileMemory)); }
                //try { markerColor = instructionSet[3]; } catch (FormatException) { return new MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>>(0, new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(volatileMemory, nonVolatileMemory)); }
                //Configure memory abstracts
                string[] thisProgramMemoryStrings = new string[0] { };
                bool[] thisProgramMemoryBools = new bool[0] { };
                int[] thisProgramMemoryInts = new int[2] { 1 , currentSafezoneradius};


                MyCubeBlock thisLogicalCore = entity as MyCubeBlock;
                MyTuple<int, MyTuple<string[], bool[], int[]>> thisProgramNonVolatileMemorySegment = PEWCoreNonVolatileMemory.GetMemorySegment(thisLogicalCore.Name, nonVolatileMemory.NonVolatileMemoryNonShared);
                if (thisProgramNonVolatileMemorySegment.Item1 == 0)
                { PEWCoreNonVolatileMemory.SetMemorySegment(thisLogicalCore.Name, nonVolatileMemory.NonVolatileMemoryNonShared, thisProgramMemoryStrings, thisProgramMemoryBools, thisProgramMemoryInts); }
                else
                {
                    thisProgramMemoryStrings = thisProgramNonVolatileMemorySegment.Item2.Item1;
                    thisProgramMemoryBools = thisProgramNonVolatileMemorySegment.Item2.Item2;
                    thisProgramMemoryInts = thisProgramNonVolatileMemorySegment.Item2.Item3;
                }

                timeUntilLastExecution = thisProgramMemoryInts[0];
                currentSafezoneradius = thisProgramMemoryInts[1];

                //MyAPIGateway.Utilities.ShowMessage("DBG", "Entry");

                VRage.ModAPI.IMyEntity parent = entity.GetTopMostParent(); //Topmost parent of logical core is the grid on which it is installed.

                //MyAPIGateway.Utilities.ShowMessage("DBG", "Proper");

                switch (faction)
                {
                    case 1:
                        if ((PEWCoreMain.ConfigData.PEWGeneralConfig.PEWCore_Faction1Tag != "xxxxx") && (PEWCoreMain.ConfigData.PEWGeneralConfig.PEWCore_Faction1Tag != ""))
                        {
                            safeZoneFaction = PEWCoreMain.ConfigData.PEWGeneralConfig.PEWCore_Faction1Tag;
                        } else {
                            Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[PEWCoreModule_SSZ] System failure. PEWCore_Faction1Tag is not configured.", VRageMath.Color.White);
                            return new MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>>(0, new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(volatileMemory, nonVolatileMemory)); }
                    break;
                    case 2:
                        if ((PEWCoreMain.ConfigData.PEWGeneralConfig.PEWCore_Faction2Tag != "xxxxx") && (PEWCoreMain.ConfigData.PEWGeneralConfig.PEWCore_Faction2Tag != ""))
                        {
                            safeZoneFaction = PEWCoreMain.ConfigData.PEWGeneralConfig.PEWCore_Faction2Tag;
                        }
                        else {
                            Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[PEWCoreModule_SSZ] System failure. PEWCore_Faction2Tag is not configured.", VRageMath.Color.White);
                            return new MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>>(0, new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(volatileMemory, nonVolatileMemory)); }
                    break;
                    case 3:
                        if ((PEWCoreMain.ConfigData.PEWGeneralConfig.PEWCore_Faction3Tag != "xxxxx") && (PEWCoreMain.ConfigData.PEWGeneralConfig.PEWCore_Faction3Tag != ""))
                        {
                            safeZoneFaction = PEWCoreMain.ConfigData.PEWGeneralConfig.PEWCore_Faction3Tag;
                        }
                        else {
                            Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[PEWCoreModule_SSZ] System failure. PEWCore_Faction3Tag is not configured.", VRageMath.Color.White);
                            return new MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>>(0, new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(volatileMemory, nonVolatileMemory)); }
                    break;
                }

                System.Collections.Generic.List<IMyPlayer> connectedPlayers = new System.Collections.Generic.List<IMyPlayer>();
                MyAPIGateway.Players.GetPlayers(connectedPlayers); //Populate the IMyPlayer List

                Vector3D position = parent.GetPosition();
                BoundingSphereD sphere = new BoundingSphereD(position, Convert.ToDouble(currentSafezoneradius));
                List<VRage.ModAPI.IMyEntity> entities = MyAPIGateway.Entities.GetEntitiesInSphere(ref sphere);
                IMyFaction Faction = MyAPIGateway.Session.Factions.TryGetFactionByTag(safeZoneFaction);

                foreach (IMyEntity foundEntity in entities)
                {
                    // Projection or invalid object
                    if (foundEntity.Physics == null)
                        continue;

                    // Waypoints and other things that are free of physics
                    if (!foundEntity.Physics.Enabled)
                        continue;

                    // Ignore our own ship
                    if (foundEntity == entity.GetTopMostParent())
                        break;


                    if (foundEntity is IMyCharacter)
                    {
                        if (!PEWCoreMain.ConfigData.PEWSSZConfig.PEWSSZ_AllowEnemyCharacters)
                        {
                            IMyCharacter foundPlayer = (IMyCharacter)foundEntity;
                            bool playerIsAdmin = false;
                            for (int x = 0; x < connectedPlayers.Count; ++x)
                            {
                                if (connectedPlayers[x].IdentityId == foundPlayer.ControllerInfo.ControllingIdentityId)
                                {
                                    if (connectedPlayers[x].PromoteLevel == MyPromoteLevel.Admin || connectedPlayers[x].PromoteLevel == MyPromoteLevel.Owner)
                                    {
                                        playerIsAdmin = true;
                                    }
                                }
                            }

                            if (foundPlayer.IsPlayer && !playerIsAdmin)
                            {
                                if (Faction != MyAPIGateway.Session.Factions.TryGetPlayerFaction(foundPlayer.ControllerInfo.ControllingIdentityId))
                                {
                                    Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("Player " + foundPlayer.DisplayName + " tried to enter " + safeZoneFaction + "'s soft safezone and was killed by the guardians.", VRageMath.Color.Red);
                                    foundPlayer.Kill();
                                }
                            }
                        }
                    }

                    if (foundEntity is IMyCubeGrid)
                    {
                        if (!PEWCoreMain.ConfigData.PEWSSZConfig.PEWSSZ_AllowEnemyRemoteControlBlocks)
                        {
                            MyCubeGrid grid = (MyCubeGrid)foundEntity;
                            IMyCubeGrid igrid = (IMyCubeGrid)foundEntity;
                            bool hostileGrid = false;

                            for (int i = 0; i < igrid.BigOwners.Count; i++)
                            {
                                if (MyAPIGateway.Session.Factions.TryGetPlayerFaction(igrid.BigOwners[i]) != Faction)
                                {
                                    hostileGrid = true;
                                }
                            }
                            if (hostileGrid)
                            {
                                foreach (MyCubeBlock block in grid.GetFatBlocks())
                                {
                                    var remoteControl = block as IMyRemoteControl;
                                    if (remoteControl != null)
                                    {
                                        var factionTag = block.GetOwnerFactionTag();
                                        if (block.GetType() == typeof(MyRemoteControl))
                                        {
                                            if (remoteControl.IsFunctional)
                                            {
                                                if (factionTag != null)
                                                {
                                                    if (factionTag != "")
                                                    {
                                                        if (factionTag != safeZoneFaction)
                                                        {
                                                            var iblock = block as IMyCubeBlock;
                                                            ((IMySlimBlock)iblock.SlimBlock).DecreaseMountLevel(999999f, null);
                                                            Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("A player on " + factionTag + " tried to send one or more remote control block(s) into " + safeZoneFaction + "'s soft safezone. Any remote control blocks were destroyed by the guardians.", VRageMath.Color.White);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var iblock = block as IMyCubeBlock;
                                                        ((IMySlimBlock)iblock.SlimBlock).DecreaseMountLevel(999999f, null);
                                                        Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("A factionless player tried to send one or more remote control block(s) into " + safeZoneFaction + "'s soft safezone. Any remote control blocks were destroyed by the guardians.", VRageMath.Color.White);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                List<MyTuple<string, int, IMyGps>> systemGPSTable = PEWCoreMain.PEWNetworkGPSManager.GPSManagerSystemGPSTable();
                bool HVTinSafezone = false;
                for (int z = 0; z < systemGPSTable.Count; z++)
                {
                    if (Regex.IsMatch(systemGPSTable[z].Item3.Name, "HVT_"))
                    {
                        if (Regex.IsMatch(systemGPSTable[z].Item3.Name, safeZoneFaction))
                        {
                            HVTinSafezone = true;
                            if (timeUntilLastExecution >= PEWCoreMain.ConfigData.PEWSSZConfig.PEWSSZ_HVTActionInterval)
                            {
                                if (Vector3D.Distance(systemGPSTable[z].Item3.Coords, position) < (double)currentSafezoneradius)
                                {
                                    currentSafezoneradius = currentSafezoneradius - (int)Math.Round(PEWCoreMain.ConfigData.PEWSSZConfig.PEWSSZ_ShrinkDistance);
                                    if (currentSafezoneradius < 0) { currentSafezoneradius = 0; }
                                    Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored(safeZoneFaction + " has an HVT inside their soft safezone! " + safeZoneFaction + "'s soft safezone effective range has been reduced to " + currentSafezoneradius.ToString() + " meters.", VRageMath.Color.White);
                                }
                                timeUntilLastExecution = 0;
                            }
                        }
                    }
                }

                if (!HVTinSafezone)
                {
                    if (currentSafezoneradius < (int)Math.Round(PEWCoreMain.ConfigData.PEWSSZConfig.PEWSSZ_Radius))
                    {
                        currentSafezoneradius = currentSafezoneradius + (int)Math.Round(PEWCoreMain.ConfigData.PEWSSZConfig.PEWSSZ_RegenerateDistance);
                        if (currentSafezoneradius > (int)Math.Round(PEWCoreMain.ConfigData.PEWSSZConfig.PEWSSZ_Radius)) { currentSafezoneradius = (int)Math.Round(PEWCoreMain.ConfigData.PEWSSZConfig.PEWSSZ_Radius); }
                        Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored(safeZoneFaction + "'s soft safezone effective range has regenerated to " + currentSafezoneradius.ToString() + " meters.", VRageMath.Color.White);
                    }
                }


                IMyGps temp = MyAPIGateway.Session.GPS.Create("System_Soft_SafeZone [" + safeZoneFaction + "][" + currentSafezoneradius.ToString() + "]","This is the soft safezone for " + safeZoneFaction + ".",position,true);
                PEWCoreMain.PEWNetworkGPSManager.GPSManagerCleanSystemGPSByRegex("System_Soft_SafeZone", safeZoneFaction);
                PEWCoreMain.PEWNetworkGPSManager.GPSManagerAddOrUpdateSystemGPS(safeZoneFaction + "_SSZ", temp);
                timeUntilLastExecution = timeUntilLastExecution + ISspecifiedExecInterval;
                thisProgramMemoryInts[0] = timeUntilLastExecution;

                thisProgramMemoryInts[1] = currentSafezoneradius;

                PEWCoreNonVolatileMemory.SetMemorySegment(thisLogicalCore.Name, nonVolatileMemory.NonVolatileMemoryNonShared, thisProgramMemoryStrings, thisProgramMemoryBools, thisProgramMemoryInts);

                return new MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>>(ISspecifiedExecInterval, new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(volatileMemory, nonVolatileMemory));
            }
            catch (Exception ex)
            {
                return new MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>>(0, new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(volatileMemory, nonVolatileMemory));
            }
        }

    }
}
