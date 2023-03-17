//© 2023 FinancedDart
//© 2023 Phobos Engineered Weaponry Group
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using Sandbox.ModAPI;
using VRage;
using VRage.Serialization;
using VRage.Utils;

namespace PEWCore.Network
{
    [ProtoContract]
    public class PEWNetworkPacketWorldSync : PEWNetworkPacket
    {
        [ProtoMember(1)]
        //public SerializableDictionary
        public Dictionary<int, MyTuple<int, string, VRageMath.Vector3D, VRageMath.Color>> WorldSyncGPSDictionaryReference;
        //public SerializableDictionary<string, MyTuple<VRageMath.Vector3D, VRageMath.Color>> GPSDictionaryReference = new SerializableDictionary<string, MyTuple<VRageMath.Vector3D, VRageMath.Color>>();
        //Tag numbers in this class won't collide with tag numbers from the base class
        [ProtoMember(2)]
        public string Text;

        [ProtoMember(3)]
        public int Number;

        public PEWNetworkPacketWorldSync() { } //Empty constructor required for deserialization

        public PEWNetworkPacketWorldSync(Dictionary<int, MyTuple<int, string, VRageMath.Vector3D, VRageMath.Color>> latestGPSDictionary, string text, int number)
        {
            WorldSyncGPSDictionaryReference = latestGPSDictionary;
            Text = text;
            Number = number;
        }

        public override bool Received()
        {
            var msg = $"PacketSimpleExample received: Text='{Text}'; Number={Number}";
            MyLog.Default.WriteLineAndConsole(msg);
            MyAPIGateway.Utilities.ShowNotification(msg, Number);
            //PEWCoreClient.ClientUpdateWorldGPSWaypoints(WorldSyncGPSDictionaryReference);

            return true;//Relay packet to other clients (only works if server receives it)
        }
    }
}
