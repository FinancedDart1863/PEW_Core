//© FinancedDart
//© Phobos Engineered Weaponry Group
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.Game;
using VRage;
using VRageMath;

using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;


using VRage;
using VRageMath;
using VRage.Common;
using VRage.Definitions;
using VRage.ModAPI;
using VRage.Serialization;
using VRage.Game.Entities;
using VRage.Game.Components;
using VRage.Game;
using VRage.Game.ModAPI;
using System.Runtime.CompilerServices;

namespace PEWCore
{
    internal class PEWCoreLogicalCoreProcess
    {
        private static List<IMyEntity> updateList;
        private static bool Initialized = false;
        private int PEWCoreLogicalCoreExecutionIntervalHoldover;

        internal static System.Timers.Timer clearTimer = new System.Timers.Timer();

        /// <summary>
        /// Process logical cores
        /// </summary>
        public static void Process()
        {
            if (MyAPIGateway.Session == null)
                return;
            if (!MyAPIGateway.Multiplayer.IsServer)
                return;
            if (PEWCoreLogicalCore.LastPEWCoreLogicalCoreUpdate == null)
                return;
            if (!Initialized)
            {
                Initialized = true;
                Initialize();
            }

            //Check PEWCore logical cores once per second
            foreach (KeyValuePair<IMyEntity, MyTuple<IMyEntity, DateTime, int>> p in PEWCoreLogicalCore.LastPEWCoreLogicalCoreUpdate)
            {
                if (DateTime.Now - p.Value.Item2 > TimeSpan.FromSeconds(p.Value.Item3))
                {
                    ProcessLogicalCore(p.Value.Item1);
                    updateList.Add(p.Key);
                }
            }

            //Update timer within the logical core
            foreach (IMyEntity updatedItem in updateList)
            {
                if (PEWCoreLogicalCore.LastPEWCoreLogicalCoreUpdate.ContainsKey(updatedItem))
                    PEWCoreLogicalCore.LastPEWCoreLogicalCoreUpdate[updatedItem] = new MyTuple<IMyEntity, DateTime, int>(updatedItem, DateTime.Now, PEWCoreLogicalCore.LastPEWCoreLogicalCoreUpdate[updatedItem].Item3);
            }

            updateList.Clear();
        }

        private static void Initialize()
        {
            //MyAPIGateway.Utilities.ShowMessage("PEW HVT Subsystem", "Detector Initialization...");
            MyAPIGateway.Utilities.ShowMessage("[PEWCoreLogicalCoreProcess | Initialize]", "Initialize");
            PEWCoreLogging.Instance.WriteLine("[PEWCoreLogicalCoreProcess | Initialize] Initialize");
            //MyVisualScriptLogicProvider.SendChatMessageColored("PEW HVT Subsystem: Detector Initialization...", VRageMath.Color.White);
            updateList = new List<IMyEntity>();
        }

        private static bool ProcessLogicalCore(IMyEntity entity)
        {
            MyVisualScriptLogicProvider.SendChatMessageColored("Logical core process", VRageMath.Color.White);
            //Sanity check
            if (!(entity is IMyProgrammableBlock))
                return false;

            //IMyEntity parent is the entity
            IMyEntity parent = entity.GetTopMostParent();

            // Handle for beacon object
            IMyProgrammableBlock CurrentPEWCoreLogicalCore = (IMyProgrammableBlock)entity;

            // Logical core needs to be on and working for a successful process runthrough
            if (!CurrentPEWCoreLogicalCore.IsWorking || !CurrentPEWCoreLogicalCore.IsFunctional)
                return false;

            return true;
        }
    }
}
