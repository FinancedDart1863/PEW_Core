﻿//© 2023 FinancedDart
//© 2023 Phobos Engineered Weaponry Group
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.Game;
using VRage;
using VRageMath;

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

namespace PEWCore
{
    internal class PEWCoreLogicalCoreProcess
    {
        private static Dictionary<IMyEntity, int> updatelist = null;
        private static bool Initialized = false;

        internal static System.Timers.Timer clearTimer = new System.Timers.Timer();

        //Process logical cores
        public static MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory> Process(PEWCoreVolatileMemory volatileMemoryHandle,PEWCoreNonVolatileMemory nonVolatileMemoryHandle)
        {
            //Sanity check
            if (MyAPIGateway.Session == null)
                return new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(volatileMemoryHandle, nonVolatileMemoryHandle);
            if (PEWCoreLogicalCore.LastPEWCoreLogicalCoreUpdate == null)
                return new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(volatileMemoryHandle, nonVolatileMemoryHandle);
            if (!Initialized)
            {
                Initialized = true;
                Initialize();
            }

            //Iterate through each logical core. If the logical core's execution interval dictates execution of its corresponding logic, then do so.
            foreach (KeyValuePair<IMyEntity, MyTuple<IMyEntity, DateTime, int>> p in PEWCoreLogicalCore.LastPEWCoreLogicalCoreUpdate)
            {
                if (DateTime.Now - p.Value.Item2 > TimeSpan.FromSeconds(p.Value.Item3))
                {
                    //MyAPIGateway.Utilities.ShowMessage("dbg","Scheduler");
                    MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory> memoryReturn = new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(volatileMemoryHandle, nonVolatileMemoryHandle);
                    MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>> temp = ProcessLogicalCore(p.Value.Item1, volatileMemoryHandle, nonVolatileMemoryHandle);
                    volatileMemoryHandle = memoryReturn.Item1;
                    nonVolatileMemoryHandle = memoryReturn.Item2;
                    if (temp.Item1 != 0) 
                    {
                        updatelist.Add(p.Key, temp.Item1);
                        volatileMemoryHandle = temp.Item2.Item1;
                        nonVolatileMemoryHandle = temp.Item2.Item2;
                    }
                    else
                    {
                        updatelist.Add(p.Key, 1);
                    }
                }
            }

            //Update execution interval timer within the logical core
            foreach(KeyValuePair<IMyEntity, int> updatedItem in updatelist)
            {
                if (PEWCoreLogicalCore.LastPEWCoreLogicalCoreUpdate.ContainsKey(updatedItem.Key))
                    PEWCoreLogicalCore.LastPEWCoreLogicalCoreUpdate[updatedItem.Key] = new MyTuple<IMyEntity, DateTime, int>(updatedItem.Key, DateTime.Now, updatedItem.Value);
            }

            updatelist.Clear();
            return new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(volatileMemoryHandle, nonVolatileMemoryHandle);
        }

        private static void Initialize()
        {
            PEWCoreLogging.Instance.WriteLine("[PEWCoreLogicalCoreProcess | Initialize] Execute");
            if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { MyVisualScriptLogicProvider.SendChatMessageColored("[PEWCoreLogicalCoreProcess | Initialize] Execute", VRageMath.Color.White); }
            updatelist = new Dictionary<IMyEntity, int>();
        }

        private static MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>> ProcessLogicalCore(IMyEntity entity, PEWCoreVolatileMemory volatileMemory, PEWCoreNonVolatileMemory nonVolatileMemory)
        {
            //Sanity check
            if (!(entity is IMyProgrammableBlock))
                return new MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>>(0, new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(volatileMemory, nonVolatileMemory));

            //IMyEntity parent is the parent entity of the logical core entity
            IMyEntity parent = entity.GetTopMostParent();

            // Handle for logical core
            IMyProgrammableBlock CurrentPEWCoreLogicalCore = (IMyProgrammableBlock)entity;

            // Logical core needs to be on and working to continue
            //if (!CurrentPEWCoreLogicalCore.IsWorking || !CurrentPEWCoreLogicalCore.IsFunctional)
                //return false;

            string[] instructionArray = LogicalCoreReadCustomData(CurrentPEWCoreLogicalCore.CustomData);

            switch (instructionArray[0])
            {
                case "Undefined":
                    //MyAPIGateway.Utilities.ShowMessage("[PEWCoreLogicalCoreProcess | ProcessLogicalCore]", "Switch segment: Standard undefined InsSet");
                    CurrentPEWCoreLogicalCore.CustomData = "istr0:Undefined*";
                    return new MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>>(0, new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(volatileMemory, nonVolatileMemory));
                default:
                    {
                        MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>> result = PEWCoreExecutableLibrary.LoadAndExecuteProgram(entity, instructionArray, volatileMemory, nonVolatileMemory);
                        if (result.Item1 == 0) { CurrentPEWCoreLogicalCore.CustomData = "istr0:Undefined*"; return new MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>>(0, new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(volatileMemory, nonVolatileMemory));} 
                        else { return new MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>>(result.Item1, new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(result.Item2.Item1,result.Item2.Item2));}
                    }
            }
        }

        private static string[] LogicalCoreReadCustomData(string customData)
        {
            //MyVisualScriptLogicProvider.SendChatMessageColored("Logical core process", VRageMath.Color.White);
            if (customData.Length < 2)
            {
                //MyAPIGateway.Utilities.ShowMessage("dbg","debug1");
                return new String[20] {"Undefined", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""};
                if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { MyVisualScriptLogicProvider.SendChatMessageColored("[PEWCore | LogicalCoreReadCustomData] New logical core or illegal instruction set", VRageMath.Color.White); }
            }
            else
            {
                //MyAPIGateway.Utilities.ShowMessage("dbg", "debug2");
                //customData = customData.ToLowerInvariant();
                string[] instructionSet = new String[20] { "Undefined*", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                //MyAPIGateway.Utilities.ShowMessage("DEBUG", customData);
                for (int x = 0; x < 20; x++) //Loop 20 times, starting with x = 0 and x ending at 19
                {
                    string tempString = x.ToString();
                    string instructionLinePrefix = "istr";
                    string instructionLineHeader = instructionLinePrefix + tempString + ":";
                    if (customData.Contains(instructionLineHeader))
                    {
                        int pos = 0;
                        if (x <= 9) { pos = customData.IndexOf(instructionLineHeader) + 6; } else { pos = customData.IndexOf(instructionLineHeader) + 7; }
                        string minString = customData.Substring(pos, customData.Length - pos);
                        for (int r = 0; r < minString.Length; r++)
                        {
                            if (minString[r].Equals('*'))
                            {
                                if (r == 0)
                                {
                                    instructionSet[x] = "";
                                    //MyAPIGateway.Utilities.ShowMessage("[PEWCoreLogicalCoreProcess | LogicalCoreReadCustomData]", "Instruction set builder: blank instruction found");
                                }
                                else
                                {
                                    instructionSet[x] = customData.Substring(pos, (r));
                                    //MyAPIGateway.Utilities.ShowMessage("[PEWCoreLogicalCoreProcess | LogicalCoreReadCustomData]", instructionSet[x]);
                                }
                                break;
                            }
                            if (r == (minString.Length - 1))
                            {
                                if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { MyVisualScriptLogicProvider.SendChatMessageColored("[PEWCore | LogicalCoreReadCustomData] Invalid or illegal instruction set", VRageMath.Color.White); }
                                return new String[20] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                            }
                        }
                    }
                    else
                    {
                        return instructionSet;
                    }
                }
                return instructionSet;
            }
        }
    }
}
