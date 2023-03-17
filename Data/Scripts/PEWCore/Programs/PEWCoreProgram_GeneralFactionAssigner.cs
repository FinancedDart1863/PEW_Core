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
using SpaceEngineers.Game.ModAPI;

namespace PEWCore.Programs
{
    internal class PEWCoreProgram_GeneralFactionAssigner
    {
        public static MyTuple<int, PEWCoreNonVolatileMemory> execute(VRage.ModAPI.IMyEntity entity, string[] instructionSet, PEWCoreNonVolatileMemory nonVolatileMemory)
        {
            //We need to be careful in program code sections as failures, either with memory accesses or the logic itself, can crash the server. Everything needs to be in a try block.
            if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[Program | GeneralFactionAssigner] Execution", VRageMath.Color.White); }

            int ISspecifiedExecInterval = 1;
            double logicalZoneRadius = (double)3;
            try
            {
                string programName = instructionSet[0];
                try { ISspecifiedExecInterval = Int32.Parse(instructionSet[1]); } catch (FormatException) { return new MyTuple<int, PEWCoreNonVolatileMemory>(0, nonVolatileMemory); }
                string programDirective = instructionSet[2];
                string faction1Tag = instructionSet[3];
                string faction1ZoneAssignName = instructionSet[4];
                string faction2Tag = instructionSet[5];
                string faction2ZoneAssignName = instructionSet[6];
                string faction3Tag = instructionSet[7];
                string faction3ZoneAssignName = instructionSet[8];

                //Load program PEWCore memory
                MyCubeBlock thisLogicalCore = entity as MyCubeBlock;

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
                            List<IMyEntity> entities = MyAPIGateway.Entities.GetEntitiesInSphere(ref sphere);
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
                                                MyAPIGateway.Session.Factions.AddPlayerToFaction(foundPlayer.ControllerInfo.ControllingIdentityId, Faction1.FactionId);
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
                            List<IMyEntity> entities = MyAPIGateway.Entities.GetEntitiesInSphere(ref sphere);
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
                                                MyAPIGateway.Session.Factions.AddPlayerToFaction(foundPlayer.ControllerInfo.ControllingIdentityId, Faction2.FactionId);
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
                            List<IMyEntity> entities = MyAPIGateway.Entities.GetEntitiesInSphere(ref sphere);
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
                                                MyAPIGateway.Session.Factions.AddPlayerToFaction(foundPlayer.ControllerInfo.ControllingIdentityId, Faction3.FactionId);
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
                return new MyTuple<int, PEWCoreNonVolatileMemory>(ISspecifiedExecInterval, nonVolatileMemory);
            }
            catch (Exception ex)
            {
                MyAPIGateway.Utilities.ShowMessage("GeneralFactionAssigner]", "Execute failure");
                return new MyTuple<int, PEWCoreNonVolatileMemory>(0, nonVolatileMemory);
            }
        }
    }
}
