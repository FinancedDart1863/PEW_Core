using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.ModAPI;

namespace PEWCore
{
    public class PEWCoreProgramLibrary
    {
        //
        public static int LoadAndExecuteProgram(IMyEntity entity, string[] instructionSet)
        {
            switch (instructionSet[0])
            {
                case "Undefined":
                    return 1;
                case "schedulerMod":
                    return 2;
                default:
                    return 1;
            }
        }
    }
}
