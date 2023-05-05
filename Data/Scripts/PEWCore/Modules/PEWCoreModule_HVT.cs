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
using VRage.Scripting;
using VRage.Game.VisualScripting;
using System.Linq.Expressions;

namespace PEWCore.Modules
{
    internal class PEWCoreModule_HVT
    {

        public static MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>> execute(VRage.ModAPI.IMyEntity entity, string[] instructionSet, PEWCoreVolatileMemory volatileMemory, PEWCoreNonVolatileMemory nonVolatileMemory)
        {
            if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[Module | HVT] Execution", VRageMath.Color.White); }
            int ISspecifiedExecInterval = 1;
            int timeUntilLastExecution = 0; //Time until last execution in seconds
            int PEWHVT_HVTThreshold = 0;

            try
            {
                string programName = instructionSet[0];
                try { ISspecifiedExecInterval = Int32.Parse(instructionSet[1]); } catch (FormatException) { return new MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>>(0, new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(volatileMemory, nonVolatileMemory)); }
                string[] thisProgramMemoryStrings = new string[0] { };
                bool[] thisProgramMemoryBools = new bool[0] { };
                int[] thisProgramMemoryInts = new int[1] { 0 };

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
                VRage.ModAPI.IMyEntity parent = entity.GetTopMostParent(); //Topmost parent of logical core is the grid on which it is installed.

                if (timeUntilLastExecution >= PEWCoreMain.ConfigData.PEWHVTConfig.PEWHVT_CheckInterval)
                {
                    timeUntilLastExecution = 0;
                    Vector3D position = parent.GetPosition();
                    BoundingSphereD sphere = new BoundingSphereD(position, PEWCoreMain.ConfigData.PEWHVTConfig.PEWHVT_HVTScanRadius);
                    List<VRage.ModAPI.IMyEntity> entities = MyAPIGateway.Entities.GetEntitiesInSphere(ref sphere);
                    List<VRageMath.Vector3D> HVT_coordinates = new List<VRageMath.Vector3D>();
                    bool HVTViolation = false;

                    PEWCoreMain.PEWNetworkGPSManager.GPSManagerCleanHVTSystemGPS();

                    List<IMyIdentity> allHistoryPlayersIdentities = new List<IMyIdentity>();
                    allHistoryPlayersIdentities = new List<IMyIdentity>(); //Start cleanly
                    MyAPIGateway.Players.GetAllIdentites(allHistoryPlayersIdentities);
                    for (int i = 0; i < allHistoryPlayersIdentities.Count; i++)
                    {
                        PEWCoreMain.PEWNetworkGPSManager.ClientCleanHVTSystemGPS(allHistoryPlayersIdentities[i].PlayerId);
                    }

                    foreach (IMyEntity foundEntity in entities)
                    {

                        // Projection or invalid object
                        if (foundEntity.Physics == null)
                            continue;

                        // Waypoints and other things that are free of physics
                        if (!foundEntity.Physics.Enabled)
                            continue;

                        // Ignore our own grid
                        if (foundEntity == entity.GetTopMostParent())
                            break;


                        if (foundEntity is IMyCubeGrid)
                        {
                            bool adjacentHVTPresent = false;
                            IMyCubeGrid Igrid = foundEntity as IMyCubeGrid;
                            MyCubeGrid grid = foundEntity as MyCubeGrid;
                            List<long> bigowners = grid.BigOwners;
                            if ((Igrid != null) && (grid != null))
                            {
                                IMyGridGroupData gridGroup = MyAPIGateway.GridGroups.GetGridGroup(GridLinkTypeEnum.Physical, Igrid);
                                List<IMyCubeGrid> gridGroupGrids = new List<IMyCubeGrid>(); //Define list grids within the gridGroup
                                gridGroup.GetGrids(gridGroupGrids);
                                int gridGroupTotalBlocks = 0;
                                foreach (IMyCubeGrid gridX in gridGroupGrids)
                                {
                                    MyCubeGrid gridXCast = gridX as MyCubeGrid;
                                    gridGroupTotalBlocks += gridXCast.BlocksCount;
                                }

                                if ((grid.BlocksCount > PEWCoreMain.ConfigData.PEWHVTConfig.PEWHVT_HVTThreshold) || (gridGroupTotalBlocks > PEWCoreMain.ConfigData.PEWHVTConfig.PEWHVT_HVTThreshold))
                                {
                                    foreach (Vector3D temp in HVT_coordinates)
                                    {
                                        if (Vector3D.Distance(temp, foundEntity.GetPosition()) < (double)PEWCoreMain.ConfigData.PEWHVTConfig.PEWHVT_HVTClusterRadius)
                                        {
                                            adjacentHVTPresent = true;
                                        }
                                    }
                                    if (!adjacentHVTPresent)
                                    {
                                        bool gridBigOwnerSuccess = false;
                                        try 
                                        {
                                            long BigOwnerID = grid.BigOwners[0];
                                            gridBigOwnerSuccess = true;
                                        } catch (Exception e) 
                                        {
                                        }

                                        if (gridBigOwnerSuccess)
                                        {
                                            string gridBigOwnerFactionTag = "";
                                            try
                                            {
                                                IMyFaction gridBigOwnerFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(grid.BigOwners[0]);
                                                gridBigOwnerFactionTag = gridBigOwnerFaction.Tag;
                                            } catch (Exception e) { }
                                            if ((gridBigOwnerFactionTag != "SPRT") && (gridBigOwnerFactionTag != "SPID"))
                                            {
                                                HVTViolation = true;
                                                if (gridBigOwnerFactionTag != "")
                                                {
                                                    IMyGps temp = MyAPIGateway.Session.GPS.Create("HVT_High Value Target [" + gridBigOwnerFactionTag + "]", "A grid or grid-group above the threshold has been detected at these coordinates! Go attack it!", foundEntity.GetPosition(), true);
                                                    temp.GPSColor = VRageMath.Color.DarkRed;
                                                    PEWCoreMain.PEWNetworkGPSManager.GPSManagerAddSystemGPS("HVT_High Value Target", temp);
                                                }
                                                if (gridBigOwnerFactionTag == "")
                                                {
                                                    IMyGps temp = MyAPIGateway.Session.GPS.Create("HVT_High Value Target", "A grid or grid-group above the threshold has been detected at these coordinates! Go attack it!", foundEntity.GetPosition(), true);
                                                    temp.GPSColor = VRageMath.Color.DarkRed;
                                                    PEWCoreMain.PEWNetworkGPSManager.GPSManagerAddSystemGPS("HVT_High Value Target", temp);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                    }
                                }
                            }
                        }
                    }

                    if (HVTViolation)
                    {
                        Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("One or more high value target have been spotted!", VRageMath.Color.Red);
                    }

                }
                timeUntilLastExecution = timeUntilLastExecution + ISspecifiedExecInterval;
                thisProgramMemoryInts[0] = timeUntilLastExecution;

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
