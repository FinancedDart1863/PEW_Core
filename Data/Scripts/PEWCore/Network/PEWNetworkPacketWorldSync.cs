//© 2023 FinancedDart
//© 2023 Phobos Engineered Weaponry Group
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using Sandbox.ModAPI;
using VRage.Utils;

namespace PEWCore.Network
{
    [ProtoContract]
    public class PEWNetworkPacketWorldSync : PEWNetworkPacket
    {
        //Tag numbers in this class won't collide with tag numbers from the base class
        [ProtoMember(1)]
        public string Text;

        [ProtoMember(2)]
        public int Number;

        public PEWNetworkPacketWorldSync() { } //Empty constructor required for deserialization

        public PEWNetworkPacketWorldSync(string text, int number)
        {
            Text = text;
            Number = number;
        }

        public override bool Received()
        {
            var msg = $"PacketSimpleExample received: Text='{Text}'; Number={Number}";
            MyLog.Default.WriteLineAndConsole(msg);
            MyAPIGateway.Utilities.ShowNotification(msg, Number);

            return true;//Relay packet to other clients (only works if server receives it)
        }
    }
}
