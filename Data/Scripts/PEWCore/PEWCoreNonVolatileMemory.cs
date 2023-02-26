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
    public class PEWCoreNonVolatileMemory
    {
        [ProtoMember(1)]
        public SerializableDictionary<string, MyTuple<string[], bool[], int[]>> NonVolatileMemoryNonShared = new SerializableDictionary<string, MyTuple<string[], bool[], int[]>>();
        [ProtoMember(2)]
        public SerializableDictionary<string, MyTuple<string[], bool[], int[]>> NonVolatileMemoryShared = new SerializableDictionary<string, MyTuple<string[], bool[], int[]>>();

        public static MyTuple<int, MyTuple<string[], bool[], int[]>> GetMemorySegment(string address, SerializableDictionary<string, MyTuple<string[], bool[], int[]>> memory)
        {
            if (memory.Dictionary.ContainsKey(address))
            {
                return new MyTuple<int, MyTuple<string[], bool[], int[]>>(1, memory.Dictionary.GetValueOrDefault(address));
            }
            else
            { 
                return new MyTuple<int, MyTuple<string[], bool[], int[]>>(0, new MyTuple<string[], bool[], int[]>(null, null, null));
            }
        }
        public static MyTuple<int, MyTuple<string[], bool[], int[]>> SetMemorySegment(string address, SerializableDictionary<string, MyTuple<string[], bool[], int[]>> memory, string[] stringArray, bool[] boolArray, int[] intArray)
        {
            if (memory.Dictionary.ContainsKey(address))
            {
                memory.Dictionary.Remove(address);
                memory.Dictionary.Add(address, new MyTuple<string[], bool[], int[]>(stringArray, boolArray, intArray));
                return new MyTuple<int, MyTuple<string[], bool[], int[]>>(1, new MyTuple<string[], bool[], int[]>(stringArray, boolArray, intArray));
            }
            else
            {
                memory.Dictionary.Add(address, new MyTuple<string[], bool[], int[]>(stringArray, boolArray, intArray));
                return new MyTuple<int, MyTuple<string[], bool[], int[]>>(1, new MyTuple<string[], bool[], int[]>(stringArray, boolArray, intArray));
            }
        }
    }
}