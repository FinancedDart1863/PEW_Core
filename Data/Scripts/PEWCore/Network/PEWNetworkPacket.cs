//© 2023 FinancedDart
//© 2023 Phobos Engineered Weaponry Group
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using Sandbox.ModAPI;

namespace PEWCore.Network
{
    [ProtoInclude(1000, typeof(PEWNetworkPacketWorldSync))]
    [ProtoContract]
    public abstract class PEWNetworkPacket
    {
        //This field's value will be sent if it's not the default value. To define a default value you must use the [DefaultValue(...)] attribute.
        [ProtoMember(1)]
        public readonly ulong SenderId;

        public PEWNetworkPacket()
        {
            SenderId = MyAPIGateway.Multiplayer.MyId;
        }

        //Called when this packet is received on this machine.
        //<returns>Return true if you want the packet to be sent to other clients (only works server side)</returns>
        public abstract bool Received();
    }
}
