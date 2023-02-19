//© FinancedDart
//© Phobos Engineered Weaponry Group
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.ModAPI;
using Sandbox.Common.ObjectBuilders;

using VRage;
using VRage.ModAPI;
using VRage.Game.Components;
using VRage.ObjectBuilders;

namespace PEWCore
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_MyProgrammableBlock), true, "PEWCoreLOGICALCore")]
    internal class PEWCoreLogicalCore : MyGameLogicComponent
    {
        public bool PEWCoreLogicalCoreConfigured = false;
        public int PEWCoreLogicalCoreExecutionInterval = PEWCoreSettings.PEWCoreExecutionInterval;

        public static Dictionary<IMyEntity, MyTuple<IMyEntity, DateTime, int>> LastPEWCoreLogicalCoreUpdate
        {
            get 
            {
                return lastUpdate;
            }
        }

        private MyObjectBuilder_EntityBase m_objectBuilder = null;
        private static Dictionary<IMyEntity, MyTuple<IMyEntity, DateTime, int>> lastUpdate = null;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            if (lastUpdate == null)
                lastUpdate = new Dictionary<IMyEntity, MyTuple<IMyEntity, DateTime, int>>();

            IMyProgrammableBlock logiccore = (IMyProgrammableBlock)Entity;
            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;

            // Since our object builder is null, use Entity.
            if (logiccore == null)
            {
                //Logging.Instance.WriteLine("Entity is null");
            }
            else if (logiccore.BlockDefinition.SubtypeName.Contains("PEWCoreLOGICALCore"))
            {
                if (!lastUpdate.ContainsKey(Entity))
                {
                    lastUpdate.Add(Entity, new MyTuple<IMyEntity, DateTime, int>(Entity, DateTime.Now, PEWCoreSettings.PEWCoreExecutionInterval));
                }
            }
        }

        public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
        {
            return m_objectBuilder;
        }

        public override void Close()
        {
            //Logging.Instance.WriteLine(string.Format("Close PEWCoreLogicalCore logic"));

            if (Entity == null)
                return;

            if (lastUpdate.ContainsKey(Entity))
                lastUpdate.Remove(Entity);
        }
    }
}
