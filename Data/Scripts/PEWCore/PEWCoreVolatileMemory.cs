using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ProtoBuf;
using VRage;
using VRage.Serialization;

namespace PEWCore
{
    [ProtoContract(UseProtoMembersOnly = true)]
    public class PEWCoreVolatileMemory
    {
        [ProtoMember(1)]
        public Dictionary<string, MyTuple<string[], bool[], int[]>> VolatileMemoryNonShared = new Dictionary<string, MyTuple<string[], bool[], int[]>>();
        [ProtoMember(2)]
        public Dictionary<string, MyTuple<string[], bool[], int[]>> VolatileMemoryShared = new Dictionary<string, MyTuple<string[], bool[], int[]>>();

        public static MyTuple<int, MyTuple<string[], bool[], int[]>> GetMemorySegment(string address, Dictionary<string, MyTuple<string[], bool[], int[]>> memory)
        {
            if (memory.ContainsKey(address))
            {
                return new MyTuple<int, MyTuple<string[], bool[], int[]>>(1, memory.GetValueOrDefault(address));
            }
            else
            { 
                return new MyTuple<int, MyTuple<string[], bool[], int[]>>(0, new MyTuple<string[], bool[], int[]>(null, null, null));
            }
        }
        public static MyTuple<int, MyTuple<string[], bool[], int[]>> SetMemorySegment(string address, Dictionary<string, MyTuple<string[], bool[], int[]>> memory, string[] stringArray, bool[] boolArray, int[] intArray)
        {
            if (memory.ContainsKey(address))
            {
                memory.Remove(address);
                memory.Add(address, new MyTuple<string[], bool[], int[]>(stringArray, boolArray, intArray));
                return new MyTuple<int, MyTuple<string[], bool[], int[]>>(1, new MyTuple<string[], bool[], int[]>(stringArray, boolArray, intArray));
            }
            else
            {
                memory.Add(address, new MyTuple<string[], bool[], int[]>(stringArray, boolArray, intArray));
                return new MyTuple<int, MyTuple<string[], bool[], int[]>>(1, new MyTuple<string[], bool[], int[]>(stringArray, boolArray, intArray));
            }
        }
    }
}