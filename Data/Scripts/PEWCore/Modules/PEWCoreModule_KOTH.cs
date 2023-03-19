using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Game.Entities;
using VRage.Game.ModAPI.Ingame;
using VRage;

namespace PEWCore.Modules
{
    public class PEWCoreModule_KOTH
    {
        public static MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>> execute(VRage.ModAPI.IMyEntity entity, string[] instructionSet, PEWCoreVolatileMemory volatileMemory,PEWCoreNonVolatileMemory nonVolatileMemory)
        {
            //We need to be careful in program code sections as failures, either with memory accesses or the logic itself, can crash the server. Everything needs to be in a try block.

            if (PEWCoreMain.ConfigData.PEWGeneralConfig.DeveloperMode) { Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessageColored("[Module | KOTH] Execution", VRageMath.Color.White); }

            try
            {
                string programName = instructionSet[0];
                string hillReference = instructionSet[1];

                //KOTH instruction set is only two lines.
                //Use config for KOTH information
                return new MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>>(1, new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(volatileMemory, nonVolatileMemory));
            }
            catch (Exception ex)
            {
                return new MyTuple<int, MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>>(0, new MyTuple<PEWCoreVolatileMemory, PEWCoreNonVolatileMemory>(volatileMemory, nonVolatileMemory));
            }
        }
    }
}
