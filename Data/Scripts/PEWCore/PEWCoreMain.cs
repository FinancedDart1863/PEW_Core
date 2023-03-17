//© 2023 FinancedDart
//© 2023 Phobos Engineered Weaponry Group
using System.Collections;
using PEWCore.Network;
using Sandbox.ModAPI;
using VRage;
using VRage.Game.Components;
using System.IO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine;
using Sandbox.Game;

using VRage.Game;
using VRage.Game.ModAPI;
using Sandbox.Game.GUI;
using VRage.Game.ObjectBuilders.Definitions;
using ProtoBuf;
using VRage.Serialization;

namespace PEWCore
{
    //Default values for HVT detector should a config not be found
    public struct PEWCoreSettings
    {
        //PEWCore loads most settings at mod initialization. We take a modular approach for settings as we do for core gameplay features.
        //The settings here are default values unless overridden by configuration
        public static int PEWCoreExecutionInterval = 1; //Execution interval for unconfigured logical core blocks. Execution interval may be modified accordingly when logic cores read their customData configuration data.
        public static bool PEWHVT_ModuleEnable = true; //Enable/disable the HVT subsystem. Do not touch this if you don't know know what you're doing. (Disabling this may cause modules with HVT subsystem dependencies to fail, such as the safezone subsystem)
        public static int PEWHVT_ExecutionInterval = 1; // Execution interval for module logic in seconds. Do not touch this if you don't know know what you're doing.
        public static bool PEWKOTH_ModuleEnable = true; //Enable/disable the HVT subsystem. Do not touch this if you don't know know what you're doing. (Disabling this may cause modules with HVT subsystem dependencies to fail, such as the safezone subsystem)
        public static int PEWKOTH_ExecutionInterval = 1; // Execution interval for module logic in seconds. Do not touch this if you don't know know what you're doing.
    }

    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class PEWCoreMain : MySessionComponentBase
    {
        //Prepare to read configuration file
        public static PEWCoreConfig ConfigData; //All configuration data will become accessible via this handle
        public PEWCoreNonVolatileMemory PEWCoreNonVolatileMemory; //Nonvolatile program memory is accessible via this handle
        private const string CONFIG_FILE_NAME = "PEWCoreConfig.xml";
        private const string PROGRAM_MEM_NONVOLATILE = "PEWCoreProgramMemory.mem";

        private static DateTime lastUpdate; //PEWCore is heavily driven by PEWCoreLogicalCores, we setup to check timing with respect to their execution intervals
        private bool CoreInitialized; //Initialized flag for PEWCore basis
        private int NonVolatileFlushInterval = 20; //Default flush interval for nonvolatile memory to disk is 1 minute
        private int NonVolatileFlushLast = 0;

        //Allocate channel 42747 for PEWCore network communications system
        public PEWNetworkMain PEWNetworkHandle = new PEWNetworkMain(42747);
        public PEWNetworkGPSManager PEWNetworkGPSManager = new PEWNetworkGPSManager();

        private void Initialize()
        {
            CoreInitialized = true;

            //Network is setup by everyone
            //PEWNetworkHandle.Register();

            ReadConfig(); //Both clients and servers read config, but clients will have ConfigData overwritten with synchronization packet from server

            if (!MyAPIGateway.Multiplayer.IsServer) //Execute the following if we are client
            {
                PEWCoreLogging.Instance.WriteLine("[PEWCore | Initialize] Execute client code");
                MyAPIGateway.Utilities.ShowMessage("PCCore", "[Client]");
            }

            if (MyAPIGateway.Multiplayer.IsServer) //Execute the following if we are server
            {
                PEWCoreLogging.Instance.WriteLine("[PEWCore | Initialize] Execute server code");
                MyAPIGateway.Utilities.ShowMessage("PCCore", "[Server]");
                //PEWNetworkGPSManager.initialization();
                InitMemory(); //Start memory management
            }

            lastUpdate = DateTime.Now;
        }

        //Read the configuration file during initialization
        public void ReadConfig()
        {
            //For the server, if the configuration data doesn't exist, try to create it
            if (MyAPIGateway.Session.IsServer)
            {
                PEWCoreLogging.Instance.WriteLine("[PEWCore | ReadConfig] Server execute block");
                if (!MyAPIGateway.Utilities.FileExistsInWorldStorage(CONFIG_FILE_NAME, typeof(PEWCoreConfig)))
                {
                    PEWCoreLogging.Instance.WriteLine("[PEWCore | ReadConfig] Configuration XML file doesn't exist. Attempting to create a new one");
                    MyAPIGateway.Parallel.Start(() =>
                    {
                        using (var sw = MyAPIGateway.Utilities.WriteFileInWorldStorage(CONFIG_FILE_NAME, typeof(PEWCoreConfig))) sw.Write(MyAPIGateway.Utilities.SerializeToXML<PEWCoreConfig>(new PEWCoreConfig()));
                    });
                    ConfigData = new PEWCoreConfig();
                }
                else
                {
                    //Attempt to read the configuration file
                    try
                    {
                        PEWCoreLogging.Instance.WriteLine("[PEWCore | ReadConfig] Attempting to read existing configuration XML");
                        ConfigData = null;
                        var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage(CONFIG_FILE_NAME, typeof(PEWCoreConfig));
                        string configcontents = reader.ReadToEnd();
                        ConfigData = MyAPIGateway.Utilities.SerializeFromXML<PEWCoreConfig>(configcontents);
                        PEWCoreLogging.Instance.WriteLine("[PEWCore | ReadConfig] Success");
                    }
                    catch (Exception exc)
                    {
                        PEWCoreLogging.Instance.WriteLine("[PEWCore | ReadConfig] Error: Could not read configuration XML document. Exception: " + exc.ToString());
                        ConfigData = new PEWCoreConfig();
                    }
                }
            }
            else
            {
                ConfigData = new PEWCoreConfig(); //If client, just load default ConfigData and wait for synchronization from server
                ConfigData.PEWGeneralConfig.DeveloperMode = false; //Don't show developer output on clients
            }

            NonVolatileFlushInterval = ConfigData.PEWGeneralConfig.PEWCore_NonVolatileProgramMemory_FlushInterval;

            if (ConfigData.PEWGeneralConfig.DeveloperMode)
            {
                PEWCoreLogging.Instance.WriteLine("[PEWCore | ReadConfig] Developer mode is enabled");
                MyAPIGateway.Utilities.ShowMessage("PCCore", "Developer mode enabled");
            }
        }

        public void InitMemory()
        {
            PEWCoreLogging.Instance.WriteLine("[PEWCore | InitMemory]");
            if (ConfigData.PEWGeneralConfig.DeveloperMode) {MyVisualScriptLogicProvider.SendChatMessageColored("[PEWCore | InitMemory] Execution", VRageMath.Color.White);}
            if (MyAPIGateway.Session.IsServer)
            {
                PEWCoreLogging.Instance.WriteLine("[PEWCore | InitMemory] Server execute block");
                if (!MyAPIGateway.Utilities.FileExistsInWorldStorage(PROGRAM_MEM_NONVOLATILE, typeof(PEWCoreNonVolatileMemory)))
                {
                    PEWCoreLogging.Instance.WriteLine("[PEWCore | InitMemory] Nonvolatile program storage file not detected. Attempting to create a blank one");
                    MyAPIGateway.Parallel.Start(() =>
                    {
                        using (var sw = MyAPIGateway.Utilities.WriteFileInWorldStorage(PROGRAM_MEM_NONVOLATILE, typeof(PEWCoreNonVolatileMemory))) sw.Write(MyAPIGateway.Utilities.SerializeToXML<PEWCoreNonVolatileMemory>(new PEWCoreNonVolatileMemory()));
                    });
                    PEWCoreNonVolatileMemory = new PEWCoreNonVolatileMemory();
                }
                else
                {
                    //Attempt to read the nonvolatile program storage file
                    try
                    {
                        PEWCoreLogging.Instance.WriteLine("[PEWCore | InitMemory] Attempting to read existing nonvolatile program storage file");
                        PEWCoreNonVolatileMemory = null;
                        var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage(PROGRAM_MEM_NONVOLATILE, typeof(PEWCoreNonVolatileMemory));
                        string nonvolatilememory = reader.ReadToEnd();
                        PEWCoreNonVolatileMemory = MyAPIGateway.Utilities.SerializeFromXML<PEWCoreNonVolatileMemory>(nonvolatilememory);
                        PEWCoreLogging.Instance.WriteLine("[PEWCore | ReadConfig] Success");
                    }
                    catch (Exception exc)
                    {
                        PEWCoreLogging.Instance.WriteLine("[PEWCore | InitMemory] Error: Could not read nonvolatile program storage file. Exception: " + exc.ToString());
                        PEWCoreNonVolatileMemory = new PEWCoreNonVolatileMemory();
                    }
                }
                //Generate sample entries. Programs and modules can dynamically size (if needed, otherwise static) the type arrays according to their logic
                /*
                bool[] defaultBoolArray = new bool[3] { false, false, false };
                bool[] defaultBoolArray2 = new bool[5] { false, false, false, false, false};
                string[] defaultStringArray = new string[3] {"string1", "string2", "string3"};
                int[] defaultIntArray = new int[3] {1, 2, 3};
                MyTuple<string, string[], bool[], int[]> tuple = new MyTuple<string, string[], bool[], int[]>("test", defaultStringArray, defaultBoolArray, defaultIntArray);
                MyTuple<string, string[], bool[], int[]> tuple2 = new MyTuple<string, string[], bool[], int[]>("test", defaultStringArray, defaultBoolArray2, defaultIntArray);
                PEWCoreNonVolatileMemoryHandle.NonVolatileMemory.Dictionary.Add("1127537383970307059", tuple);
                PEWCoreNonVolatileMemoryHandle.NonVolatileMemory.Dictionary.Add("1127537383970307060", tuple2);
                */
            }
        }

        public void FlushNonVolatileMemory(PEWCoreNonVolatileMemory memory)
        {
            MyAPIGateway.Parallel.Start(() =>
            {
                using (var sw = MyAPIGateway.Utilities.WriteFileInWorldStorage(PROGRAM_MEM_NONVOLATILE, typeof(PEWCoreNonVolatileMemory))) sw.Write(MyAPIGateway.Utilities.SerializeToXML<PEWCoreNonVolatileMemory>(memory));
            });
        }

        public override void BeforeStart()
        {
            //Setup network for both servers and clients
            PEWNetworkHandle.Register();
        }

        public override void UpdateBeforeSimulation()
        {
            //Sanity check
            if (MyAPIGateway.Session == null)
                return;

            //Initalize mod if we haven't already
            if (!CoreInitialized)
            {
                PEWCoreLogging.Instance.WriteLine("[PEWCore | UpdateBeforeSimulation] First execution. Execute initalization.");
                Initialize();
            }

            //Execute perodically according to the defined execution interval. If we don't do this, we execute every frame and consume too many resources.
            if (DateTime.Now - lastUpdate > TimeSpan.FromSeconds(PEWCoreSettings.PEWCoreExecutionInterval))
            {


                //Client side execution routines
                if (!MyAPIGateway.Multiplayer.IsServer) //Execute code if client
                {
                    //MyAPIGateway.Gui.
                    //PEWCoreClient.ClientUpdateWorldGPSWaypoints();
                }

                //Server side execution routines
                if (MyAPIGateway.Multiplayer.IsServer) //Execute code if server
                {
                }

                //Process logical cores [Server]
                if (MyAPIGateway.Multiplayer.IsServer)
                {
                    PEWCoreNonVolatileMemory = PEWCoreLogicalCoreProcess.Process(PEWCoreNonVolatileMemory);

                    //Flush nonvolatile memory to disk according to NonVolatileFlushInterval | ConfigData.PEWGeneralConfig.PEWCore_NonVolatileProgramMemory_FlushInterval
                    if (NonVolatileFlushLast <= NonVolatileFlushInterval)
                    {
                        NonVolatileFlushLast++;
                    }
                    else
                    {
                        NonVolatileFlushLast = 0;
                        FlushNonVolatileMemory(PEWCoreNonVolatileMemory);
                    }
                }


                lastUpdate = DateTime.Now;
            }

            base.UpdateBeforeSimulation();
        }

        //Handle mod unloading
        protected override void UnloadData()
        {
            try
            {
                if (PEWCoreLogging.Instance != null)
                    PEWCoreLogging.Instance.Close();
            }
            catch { }

            PEWCoreLogicalCoreProcess.clearTimer.Stop();

            PEWNetworkHandle?.Unregister();
            PEWNetworkHandle = null;

            base.UnloadData();
        }
    }
}