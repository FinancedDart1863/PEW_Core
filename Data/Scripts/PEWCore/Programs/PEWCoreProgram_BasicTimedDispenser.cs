//© 2023 FinancedDart
//© 2023 Phobos Engineered Weaponry Group
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EmptyKeys.UserInterface.Generated;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.VisualScripting;
using VRage.Library.Collections;
using VRage.ModAPI;

namespace PEWCore.Programs
{
    public class PEWCoreProgram_BasicTimedDispenser
    {
        public static MyTuple<int, PEWCoreNonVolatileMemory> execute(VRage.ModAPI.IMyEntity entity, string[] instructionSet, PEWCoreNonVolatileMemory nonVolatileMemory)
        {
            //We need to be careful in program code sections as failures, either with memory accesses or the logic itself, can crash the server. Everything needs to be in a try block.

            if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[Program | BasicTimedDispenser] Execution", VRageMath.Color.White); }

            int ISspecifiedExecInterval = 1;
            int dispenseItem1IntervalAmount = 1;
            int dispenseItem1MaxAtOnceAmount = 1;
            int dispenseItem1Interval = 1;
            int dispenseItem2IntervalAmount = 1;
            int dispenseItem2MaxAtOnceAmount = 1;
            int dispenseItem2Interval = 1;
            int dispenseItem3IntervalAmount = 1;
            int dispenseItem3MaxAtOnceAmount = 1;
            int dispenseItem3Interval = 1;

            try
            {
                string programName = instructionSet[0];
                try { ISspecifiedExecInterval = Int32.Parse(instructionSet[1]); } catch (FormatException) { return new MyTuple<int, PEWCoreNonVolatileMemory>(0, nonVolatileMemory); }
                string programDirective = instructionSet[2];
                string containerReferenceName = instructionSet[3];
                string dispenseItem1Type = instructionSet[4];
                string dispenseItem1 = instructionSet[5];

                try { dispenseItem1IntervalAmount = Int32.Parse(instructionSet[6]); } catch (FormatException) { return new MyTuple<int, PEWCoreNonVolatileMemory>(0, nonVolatileMemory); }
                try { dispenseItem1MaxAtOnceAmount = Int32.Parse(instructionSet[7]); } catch (FormatException) { return new MyTuple<int, PEWCoreNonVolatileMemory>(0, nonVolatileMemory); }
                try { dispenseItem1Interval = Int32.Parse(instructionSet[8]); } catch (FormatException) { return new MyTuple<int, PEWCoreNonVolatileMemory>(0, nonVolatileMemory); }

                string dispenseItem2Type = instructionSet[9];
                string dispenseItem2 = instructionSet[10];
                try { dispenseItem2IntervalAmount = Int32.Parse(instructionSet[11]); } catch (FormatException) { return new MyTuple<int, PEWCoreNonVolatileMemory>(0, nonVolatileMemory); }
                try { dispenseItem2MaxAtOnceAmount = Int32.Parse(instructionSet[12]); } catch (FormatException) { return new MyTuple<int, PEWCoreNonVolatileMemory>(0, nonVolatileMemory); }
                try { dispenseItem2Interval = Int32.Parse(instructionSet[13]); } catch (FormatException) { return new MyTuple<int, PEWCoreNonVolatileMemory>(0, nonVolatileMemory); }

                string dispenseItem3Type = instructionSet[14];
                string dispenseItem3 = instructionSet[15];
                try { dispenseItem3IntervalAmount = Int32.Parse(instructionSet[16]); } catch (FormatException) { return new MyTuple<int, PEWCoreNonVolatileMemory>(0, nonVolatileMemory); }
                try { dispenseItem3MaxAtOnceAmount = Int32.Parse(instructionSet[17]); } catch (FormatException) { return new MyTuple<int, PEWCoreNonVolatileMemory>(0, nonVolatileMemory); }
                try { dispenseItem3Interval = Int32.Parse(instructionSet[18]); } catch (FormatException) { return new MyTuple<int, PEWCoreNonVolatileMemory>(0, nonVolatileMemory); }

                //Configure required program PEWCore memory abstracts
                string[] thisProgramMemoryStrings = new string[1] { "" };
                bool[] thisProgramMemoryBools = new bool[3] { false, false, false };
                int[] thisProgramMemoryInts = new int[3] { 0, 0, 0 }; //[int 1] Number of executions since refresh

                //Load program PEWCore memory
                MyCubeBlock thisLogicalCore = entity as MyCubeBlock;
                MyTuple<int, MyTuple<string[], bool[], int[]>> thisProgramMemorySegment = PEWCoreNonVolatileMemory.GetMemorySegment(thisLogicalCore.Name, nonVolatileMemory.NonVolatileMemoryNonShared);
                if (thisProgramMemorySegment.Item1 == 0)
                { PEWCoreNonVolatileMemory.SetMemorySegment(thisLogicalCore.Name, nonVolatileMemory.NonVolatileMemoryNonShared, thisProgramMemoryStrings, thisProgramMemoryBools, thisProgramMemoryInts); }
                else
                {
                    thisProgramMemoryStrings = thisProgramMemorySegment.Item2.Item1;
                    thisProgramMemoryBools = thisProgramMemorySegment.Item2.Item2;
                    thisProgramMemoryInts = thisProgramMemorySegment.Item2.Item3;
                }
                //Program PEWCore memory
                int timeSinceLastFullCompletion = thisProgramMemoryInts[0];
                bool item1Intervaled = thisProgramMemoryBools[0];
                bool item2Intervaled = thisProgramMemoryBools[1];
                bool item3Intervaled = thisProgramMemoryBools[2];
                timeSinceLastFullCompletion += ISspecifiedExecInterval;

                VRage.ModAPI.IMyEntity parent = entity.GetTopMostParent(); //Topmost parent of logical core is the grid on which it is installed.
                MyCubeGrid grid = parent as MyCubeGrid; //Convert grid entity to grid
                foreach (MyCubeBlock block in grid.GetFatBlocks()) //Walk each block in the grid
                {
                    try {
                        if (block.GetType() == typeof(MyCargoContainer))
                        {
                            var currentContainer = block as IMyCargoContainer;
                            if (currentContainer != null)
                            {
                                if (currentContainer.IsFunctional && currentContainer.IsWorking)
                                {
                                    if (currentContainer.CustomName.Equals(containerReferenceName))
                                    {
                                        VRage.Game.ModAPI.IMyInventory inventory = currentContainer.GetInventory();
                                        Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("here1 ", VRageMath.Color.White);
                                        //Valid type and object for item 1
                                        if (dispenseItem1 != "Null" && dispenseItem1Type != "Null")
                                        {
                                            Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("here2 ", VRageMath.Color.White);
                                            if (!item1Intervaled)
                                            {
                                                Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("here3 ", VRageMath.Color.White);
                                                if (timeSinceLastFullCompletion >= dispenseItem1Interval)
                                                {
                                                    Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("here4 ", VRageMath.Color.White);
                                                    Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("dbgitem1 ", VRageMath.Color.White);
                                                    switch (dispenseItem1Type)
                                                    {
                                                        case "Component":
                                                            Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("dbgitem1component ", VRageMath.Color.White);
                                                            BTDispenserAddComponent(programDirective, inventory, new MyItemType("MyObjectBuilder_Component", dispenseItem1), dispenseItem1, dispenseItem1IntervalAmount, dispenseItem1MaxAtOnceAmount);
                                                            break;
                                                        case "PhysicalObject":
                                                            BTDispenserAddPhysicalObject(programDirective, inventory, new MyItemType("MyObjectBuilder_PhysicalObject", dispenseItem1), dispenseItem1, dispenseItem1IntervalAmount, dispenseItem1MaxAtOnceAmount);
                                                            break;
                                                        case "ConsumableItem":
                                                            BTDispenserAddPhysicalObject(programDirective, inventory, new MyItemType("MyObjectBuilder_ConsumableItem", dispenseItem1), dispenseItem1, dispenseItem1IntervalAmount, dispenseItem1MaxAtOnceAmount);
                                                            break;
                                                        case "Datapad":
                                                            BTDispenserAddDatapad(programDirective, inventory, new MyItemType("MyObjectBuilder_Datapad", dispenseItem1), dispenseItem1, dispenseItem1IntervalAmount, dispenseItem1MaxAtOnceAmount);
                                                            break;
                                                        case "Ore":
                                                            BTDispenserAddOre(programDirective, inventory, new MyItemType("MyObjectBuilder_Ore", dispenseItem1), dispenseItem1, dispenseItem1IntervalAmount, dispenseItem1MaxAtOnceAmount);
                                                            break;
                                                        case "Ingot":
                                                            BTDispenserAddIngot(programDirective, inventory, new MyItemType("MyObjectBuilder_Ingot", dispenseItem1), dispenseItem1, dispenseItem1IntervalAmount, dispenseItem1MaxAtOnceAmount);
                                                            break;
                                                        default:
                                                            return new MyTuple<int, PEWCoreNonVolatileMemory>(0, nonVolatileMemory);
                                                            break;
                                                    }
                                                    item1Intervaled = true;
                                                }
                                            }
                                        } else { item1Intervaled = true; }

                                        if (dispenseItem2 != "Null" && dispenseItem2Type != "Null")
                                        {
                                            if (!item2Intervaled)
                                            {
                                                if (timeSinceLastFullCompletion >= dispenseItem2Interval)
                                                {
                                                    switch (dispenseItem2Type)
                                                    {
                                                        case "Component":
                                                            BTDispenserAddComponent(programDirective, inventory, new MyItemType("MyObjectBuilder_Component", dispenseItem2), dispenseItem2, dispenseItem2IntervalAmount, dispenseItem2MaxAtOnceAmount);
                                                            break;
                                                        case "PhysicalObject":
                                                            BTDispenserAddPhysicalObject(programDirective, inventory, new MyItemType("MyObjectBuilder_PhysicalObject", dispenseItem2), dispenseItem2, dispenseItem2IntervalAmount, dispenseItem2MaxAtOnceAmount);
                                                            break;
                                                        case "ConsumableItem":
                                                            BTDispenserAddPhysicalObject(programDirective, inventory, new MyItemType("MyObjectBuilder_ConsumableItem", dispenseItem2), dispenseItem2, dispenseItem2IntervalAmount, dispenseItem2MaxAtOnceAmount);
                                                            break;
                                                        case "Datapad":
                                                            BTDispenserAddDatapad(programDirective, inventory, new MyItemType("MyObjectBuilder_Datapad", dispenseItem2), dispenseItem2, dispenseItem2IntervalAmount, dispenseItem2MaxAtOnceAmount);
                                                            break;
                                                        case "Ore":
                                                            BTDispenserAddOre(programDirective, inventory, new MyItemType("MyObjectBuilder_Ore", dispenseItem2), dispenseItem2, dispenseItem2IntervalAmount, dispenseItem2MaxAtOnceAmount);
                                                            break;
                                                        case "Ingot":
                                                            BTDispenserAddIngot(programDirective, inventory, new MyItemType("MyObjectBuilder_Ingot", dispenseItem2), dispenseItem2, dispenseItem2IntervalAmount, dispenseItem2MaxAtOnceAmount);
                                                            break;
                                                        default:
                                                            return new MyTuple<int, PEWCoreNonVolatileMemory>(0, nonVolatileMemory);
                                                            break;
                                                    }
                                                    item2Intervaled = true;
                                                }
                                            }
                                        } else { item2Intervaled = true; }

                                        if (dispenseItem3 != "Null" && dispenseItem3Type != "Null")
                                        {
                                            if (!item3Intervaled)
                                            {
                                                if (timeSinceLastFullCompletion >= dispenseItem3Interval)
                                                {
                                                    switch (dispenseItem3Type)
                                                    {
                                                        case "Component":
                                                            BTDispenserAddComponent(programDirective, inventory, new MyItemType("MyObjectBuilder_Component", dispenseItem3), dispenseItem3, dispenseItem3IntervalAmount, dispenseItem3MaxAtOnceAmount);
                                                            break;
                                                        case "PhysicalObject":
                                                            BTDispenserAddPhysicalObject(programDirective, inventory, new MyItemType("MyObjectBuilder_PhysicalObject", dispenseItem3), dispenseItem3, dispenseItem3IntervalAmount, dispenseItem3MaxAtOnceAmount);
                                                            break;
                                                        case "ConsumableItem":
                                                            BTDispenserAddPhysicalObject(programDirective, inventory, new MyItemType("MyObjectBuilder_ConsumableItem", dispenseItem3), dispenseItem3, dispenseItem3IntervalAmount, dispenseItem3MaxAtOnceAmount);
                                                            break;
                                                        case "Datapad":
                                                            BTDispenserAddDatapad(programDirective, inventory, new MyItemType("MyObjectBuilder_Datapad", dispenseItem3), dispenseItem3, dispenseItem3IntervalAmount, dispenseItem3MaxAtOnceAmount);
                                                            break;
                                                        case "Ore":
                                                            BTDispenserAddOre(programDirective, inventory, new MyItemType("MyObjectBuilder_Ore", dispenseItem3), dispenseItem3, dispenseItem3IntervalAmount, dispenseItem3MaxAtOnceAmount);
                                                            break;
                                                        case "Ingot":
                                                            BTDispenserAddIngot(programDirective, inventory, new MyItemType("MyObjectBuilder_Ingot", dispenseItem3), dispenseItem3, dispenseItem3IntervalAmount, dispenseItem3MaxAtOnceAmount);
                                                            break;
                                                        default:
                                                            return new MyTuple<int, PEWCoreNonVolatileMemory>(0, nonVolatileMemory);
                                                            break;
                                                    }
                                                    item3Intervaled = true;
                                                }
                                            }
                                        } else { item3Intervaled = true; }
                                        //MyObjectBuilder_
                                        //MyItemType steelPlateType = new MyItemType("MyObjectBuilder_Component", "SteelPlate");
                                        //MyItemType ironIngotType = new MyItemType
                                        //Try to add item1

                                        //if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { MyVisualScriptLogicProvider.SendChatMessageColored("[Program | BasicTimedDispenser] Container found", VRageMath.Color.White); }
                                    }
                                }
                            }
                        }
                    } catch (Exception ex) {
                        return new MyTuple<int, PEWCoreNonVolatileMemory>(0, nonVolatileMemory);
                    }

                    if (item1Intervaled && item2Intervaled & item3Intervaled)
                    {
                        timeSinceLastFullCompletion = 0;
                        item1Intervaled = false;
                        item2Intervaled = false;
                        item3Intervaled = false;
                    }

                    //We need to write the program memory segment at the end of the core program logic
                    thisProgramMemoryInts[0] = timeSinceLastFullCompletion;
                    thisProgramMemoryBools[0] = item1Intervaled;
                    thisProgramMemoryBools[1] = item2Intervaled;
                    thisProgramMemoryBools[2] = item3Intervaled;

                    PEWCoreNonVolatileMemory.SetMemorySegment(thisLogicalCore.Name, nonVolatileMemory.NonVolatileMemoryNonShared, thisProgramMemoryStrings, thisProgramMemoryBools, thisProgramMemoryInts);
                }
                return new MyTuple<int, PEWCoreNonVolatileMemory>(ISspecifiedExecInterval, nonVolatileMemory);
            }
            catch (Exception ex)
            {
                return new MyTuple<int, PEWCoreNonVolatileMemory>(0, nonVolatileMemory);
            }
        }

        public static void BTDispenserAddComponent(string programDirective, VRage.Game.ModAPI.IMyInventory inventory, MyItemType componentType, string dispenseItem, int dispenseItemIntervalAmount, int dispenseItemMaxAtOnceAmount)
        {
            Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("dbg0 ", VRageMath.Color.White);
            if (programDirective.Equals("TopOff"))
            {
                Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("dbg1 ", VRageMath.Color.White);
                if (inventory.GetItemAmount(componentType) < dispenseItemMaxAtOnceAmount)
                {
                    Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("dbg2 ", VRageMath.Color.White);
                    if ((inventory.GetItemAmount(componentType) + dispenseItemIntervalAmount) > dispenseItemMaxAtOnceAmount)
                    {
                        Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("dbg3 ", VRageMath.Color.White);
                        if (inventory.CanItemsBeAdded(dispenseItemMaxAtOnceAmount - inventory.GetItemAmount(componentType), componentType))
                        {
                            Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("dbg4 ", VRageMath.Color.White);
                            inventory.AddItems(dispenseItemMaxAtOnceAmount - inventory.GetItemAmount(componentType), new MyObjectBuilder_Component() { SubtypeName = dispenseItem });
                        }
                    }
                    else
                    {
                        if (inventory.CanItemsBeAdded(dispenseItemIntervalAmount, componentType))
                        {
                            inventory.AddItems(dispenseItemIntervalAmount, new MyObjectBuilder_Component() { SubtypeName = dispenseItem });
                        }
                    }
                }
            }
            else 
            {
                if (programDirective.Equals("BlindFill"))
                {
                    if (inventory.CanItemsBeAdded(dispenseItemIntervalAmount, componentType))
                    {
                        inventory.AddItems(dispenseItemIntervalAmount, new MyObjectBuilder_Component() { SubtypeName = dispenseItem });
                    }
                }
            }
        }

        public static void BTDispenserAddPhysicalObject(string programDirective, VRage.Game.ModAPI.IMyInventory inventory, MyItemType componentType, string dispenseItem, int dispenseItemIntervalAmount, int dispenseItemMaxAtOnceAmount)
        {
            if (programDirective.Equals("TopOff"))
            {
                if (inventory.GetItemAmount(componentType) < dispenseItemMaxAtOnceAmount)
                {
                    if ((inventory.GetItemAmount(componentType) + dispenseItemIntervalAmount) > dispenseItemMaxAtOnceAmount)
                    {
                        if (inventory.CanItemsBeAdded(dispenseItemMaxAtOnceAmount - inventory.GetItemAmount(componentType), componentType))
                        {
                            inventory.AddItems(dispenseItemMaxAtOnceAmount - inventory.GetItemAmount(componentType), new MyObjectBuilder_PhysicalObject() { SubtypeName = dispenseItem });
                        }
                    }
                    else
                    {
                        if (inventory.CanItemsBeAdded(dispenseItemIntervalAmount, componentType))
                        {
                            inventory.AddItems(dispenseItemIntervalAmount, new MyObjectBuilder_PhysicalObject() { SubtypeName = dispenseItem });
                        }
                    }
                }
            }
            else
            {
                if (programDirective.Equals("BlindFill"))
                {
                    if (inventory.CanItemsBeAdded(dispenseItemIntervalAmount, componentType))
                    {
                        inventory.AddItems(dispenseItemIntervalAmount, new MyObjectBuilder_PhysicalObject() { SubtypeName = dispenseItem });
                    }
                }
            }
        }

        public static void BTDispenserAddConsumableItem(string programDirective, VRage.Game.ModAPI.IMyInventory inventory, MyItemType componentType, string dispenseItem, int dispenseItemIntervalAmount, int dispenseItemMaxAtOnceAmount)
        {
            if (programDirective.Equals("TopOff"))
            {
                if (inventory.GetItemAmount(componentType) < dispenseItemMaxAtOnceAmount)
                {
                    if ((inventory.GetItemAmount(componentType) + dispenseItemIntervalAmount) > dispenseItemMaxAtOnceAmount)
                    {
                        if (inventory.CanItemsBeAdded(dispenseItemMaxAtOnceAmount - inventory.GetItemAmount(componentType), componentType))
                        {
                            inventory.AddItems(dispenseItemMaxAtOnceAmount - inventory.GetItemAmount(componentType), new MyObjectBuilder_ConsumableItem() { SubtypeName = dispenseItem });
                        }
                    }
                    else
                    {
                        if (inventory.CanItemsBeAdded(dispenseItemIntervalAmount, componentType))
                        {
                            inventory.AddItems(dispenseItemIntervalAmount, new MyObjectBuilder_ConsumableItem() { SubtypeName = dispenseItem });
                        }
                    }
                }
            }
            else
            {
                if (programDirective.Equals("BlindFill"))
                {
                    if (inventory.CanItemsBeAdded(dispenseItemIntervalAmount, componentType))
                    {
                        inventory.AddItems(dispenseItemIntervalAmount, new MyObjectBuilder_ConsumableItem() { SubtypeName = dispenseItem });
                    }
                }
            }
        }

        public static void BTDispenserAddDatapad(string programDirective, VRage.Game.ModAPI.IMyInventory inventory, MyItemType componentType, string dispenseItem, int dispenseItemIntervalAmount, int dispenseItemMaxAtOnceAmount)
        {
            if (programDirective.Equals("TopOff"))
            {
                if (inventory.GetItemAmount(componentType) < dispenseItemMaxAtOnceAmount)
                {
                    if ((inventory.GetItemAmount(componentType) + dispenseItemIntervalAmount) > dispenseItemMaxAtOnceAmount)
                    {
                        if (inventory.CanItemsBeAdded(dispenseItemMaxAtOnceAmount - inventory.GetItemAmount(componentType), componentType))
                        {
                            inventory.AddItems(dispenseItemMaxAtOnceAmount - inventory.GetItemAmount(componentType), new MyObjectBuilder_Datapad() { SubtypeName = dispenseItem });
                        }
                    }
                    else
                    {
                        if (inventory.CanItemsBeAdded(dispenseItemIntervalAmount, componentType))
                        {
                            inventory.AddItems(dispenseItemIntervalAmount, new MyObjectBuilder_Datapad() { SubtypeName = dispenseItem });
                        }
                    }
                }
            }
            else
            {
                if (programDirective.Equals("BlindFill"))
                {
                    if (inventory.CanItemsBeAdded(dispenseItemIntervalAmount, componentType))
                    {
                        inventory.AddItems(dispenseItemIntervalAmount, new MyObjectBuilder_Datapad() { SubtypeName = dispenseItem });
                    }
                }
            }
        }

        public static void BTDispenserAddOre(string programDirective, VRage.Game.ModAPI.IMyInventory inventory, MyItemType componentType, string dispenseItem, int dispenseItemIntervalAmount, int dispenseItemMaxAtOnceAmount)
        {
            if (programDirective.Equals("TopOff"))
            {
                if (inventory.GetItemAmount(componentType) < dispenseItemMaxAtOnceAmount)
                {
                    if ((inventory.GetItemAmount(componentType) + dispenseItemIntervalAmount) > dispenseItemMaxAtOnceAmount)
                    {
                        if (inventory.CanItemsBeAdded(dispenseItemMaxAtOnceAmount - inventory.GetItemAmount(componentType), componentType))
                        {
                            inventory.AddItems(dispenseItemMaxAtOnceAmount - inventory.GetItemAmount(componentType), new MyObjectBuilder_Ore() { SubtypeName = dispenseItem });
                        }
                    }
                    else
                    {
                        if (inventory.CanItemsBeAdded(dispenseItemIntervalAmount, componentType))
                        {
                            inventory.AddItems(dispenseItemIntervalAmount, new MyObjectBuilder_Ore() { SubtypeName = dispenseItem });
                        }
                    }
                }
            }
            else
            {
                if (programDirective.Equals("BlindFill"))
                {
                    if (inventory.CanItemsBeAdded(dispenseItemIntervalAmount, componentType))
                    {
                        inventory.AddItems(dispenseItemIntervalAmount, new MyObjectBuilder_Ore() { SubtypeName = dispenseItem });
                    }
                }
            }
        }

        public static void BTDispenserAddIngot(string programDirective, VRage.Game.ModAPI.IMyInventory inventory, MyItemType componentType, string dispenseItem, int dispenseItemIntervalAmount, int dispenseItemMaxAtOnceAmount)
        {
            if (programDirective.Equals("TopOff"))
            {
                if (inventory.GetItemAmount(componentType) < dispenseItemMaxAtOnceAmount)
                {
                    if ((inventory.GetItemAmount(componentType) + dispenseItemIntervalAmount) > dispenseItemMaxAtOnceAmount)
                    {
                        if (inventory.CanItemsBeAdded(dispenseItemMaxAtOnceAmount - inventory.GetItemAmount(componentType), componentType))
                        {
                            inventory.AddItems(dispenseItemMaxAtOnceAmount - inventory.GetItemAmount(componentType), new MyObjectBuilder_Ingot() { SubtypeName = dispenseItem });
                        }
                    }
                    else
                    {
                        if (inventory.CanItemsBeAdded(dispenseItemIntervalAmount, componentType))
                        {
                            inventory.AddItems(dispenseItemIntervalAmount, new MyObjectBuilder_Ingot() { SubtypeName = dispenseItem });
                        }
                    }
                }
            }
            else
            {
                if (programDirective.Equals("BlindFill"))
                {
                    if (inventory.CanItemsBeAdded(dispenseItemIntervalAmount, componentType))
                    {
                        inventory.AddItems(dispenseItemIntervalAmount, new MyObjectBuilder_Ingot() { SubtypeName = dispenseItem });
                    }
                }
            }
        }
    }
}
