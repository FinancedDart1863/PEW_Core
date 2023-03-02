//© 2023 FinancedDart
//© 2023 Phobos Engineered Weaponry Group
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PEWCore.Programs;
using Sandbox.ModAPI;
using VRage;
using VRage.ModAPI;

namespace PEWCore
{
    public class PEWCoreExecutableLibrary
    {
        public static MyTuple<int, PEWCoreNonVolatileMemory> LoadAndExecuteProgram(IMyEntity entity, string[] instructionSet, PEWCoreNonVolatileMemory nonVolatileMemory)
        {
            //A return from exectuable logic will be a zero or nonzero. Zero indicates an error within the exectuable code, while a nonzero indicates a success.
            //The result is forwarded back to the scheduler so the logic can adjust its execution interval if needed.
            //Note the returning a positive execution indication will cause the nonvolatile memory to be updated
            int result = 1;
            MyAPIGateway.Utilities.ShowMessage("[PEWCoreExecutableLibrary | Switch]", instructionSet[0]);
            switch (instructionSet[0])
            {
                case "Undefined":
                    return new MyTuple<int, PEWCoreNonVolatileMemory>(0, nonVolatileMemory);
                case "BasicTimedDispenser":
                    MyAPIGateway.Utilities.ShowMessage("[PEWCoreExecutableLibrary | BasicTimedDispenserExecute]", "BasicTimedDispenser");
                    return PEWCoreProgram_BasicTimedDispenser.execute(entity, instructionSet, nonVolatileMemory);
                default:
                    return new MyTuple<int, PEWCoreNonVolatileMemory>(0, nonVolatileMemory);
            }
        }
    }
}
