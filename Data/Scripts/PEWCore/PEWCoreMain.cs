//© 2023 FinancedDart
//© 2023 Phobos Engineered Weaponry Group
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
using Sandbox.ModAPI;

using VRage.Game.Components;
using VRage.Game;
using VRage.Game.ModAPI;
using Sandbox.Game.GUI;
using VRage.Game.ObjectBuilders.Definitions;
using ProtoBuf;
using PEWCore.Network;

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
        public PEWCoreConfig ConfigData; //All configuration data will become accessible via this handle
        private const string CONFIG_FILE_NAME = "PEWCoreConfig.xml";


        //PEWCore is heavily driven by PEWCoreLogicalCores, we setup timing to check for them
        private static DateTime lastUpdate;

        //Initialized flag for PEWCore basis
        private bool CoreInitialized;

        //Allocate channel 42747 for PEWCore network communications system
        public PEWNetworkMain PEWNetworkHandle = new PEWNetworkMain(42747);

        private void Initialize()
        {
            CoreInitialized = true;

            //SetupNetwork();

            if (!MyAPIGateway.Multiplayer.IsServer) //Execute the following if we are client
            {
                PEWCoreLogging.Instance.WriteLine("[PEWCore | Initialize] Execute client code");
                MyAPIGateway.Utilities.ShowMessage("PCCore", "Client mode");
            }

            if (MyAPIGateway.Multiplayer.IsServer) //Execute the following if we are server
            {
                PEWCoreLogging.Instance.WriteLine("[PEWCore | Initialize] Execute server code");
                MyAPIGateway.Utilities.ShowMessage("PCCore", "Server mode");
            }

            ReadConfig(); //Both clients and servers read config, but clients will have ConfigData overwritten with synchronization packet from server

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
                    PEWCoreLogging.Instance.WriteLine("[PEWCore | ReadConfig] Configuration XML file doesn't exist. Try to create a new one.");
                    MyAPIGateway.Parallel.Start(() =>
                    {
                        using (var sw = MyAPIGateway.Utilities.WriteFileInWorldStorage(CONFIG_FILE_NAME, typeof(PEWCoreConfig))) sw.Write(MyAPIGateway.Utilities.SerializeToXML<PEWCoreConfig>(new PEWCoreConfig()));
                    });
                }

                //Attempt to read the configuration file
                try
                {
                    PEWCoreLogging.Instance.WriteLine("[PEWCore | ReadConfig] Attempting to read existing configuration XML");
                    ConfigData = null;
                    var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage(CONFIG_FILE_NAME, typeof(PEWCoreConfig));
                    string configcontents = reader.ReadToEnd();
                    ConfigData = MyAPIGateway.Utilities.SerializeFromXML<PEWCoreConfig>(configcontents);

                    byte[] bytes = MyAPIGateway.Utilities.SerializeToBinary(ConfigData);
                    string encodedConfig = Convert.ToBase64String(bytes);

                    MyAPIGateway.Utilities.SetVariable("PEWCoreConfig", encodedConfig);
                }
                catch (Exception exc)
                {
                    PEWCoreLogging.Instance.WriteLine("[PEWCore | ReadConfig] Error: Could not read configuration XML document");
                    ConfigData = new PEWCoreConfig();
                }
            }
            else
            {
                ConfigData = new PEWCoreConfig(); //Don't bother with configuration files if client. Just load default ConfigData and wait for synchronization from server
            }

            if (ConfigData.PEWGeneralConfig.DeveloperMode)
            {
                PEWCoreLogging.Instance.WriteLine("[PEWCore | ReadConfig] Developer mode is enabled");
                MyAPIGateway.Utilities.ShowMessage("PCCore", "Developer mode enabled");
            }
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
                if (!MyAPIGateway.Multiplayer.IsServer) //Execute code if server
                {
                }

                //Server side execution routines
                if (MyAPIGateway.Multiplayer.IsServer) //Execute code if server
                {
                }

                //Execute core sections here
                PEWCoreLogicalCoreProcess.Process();


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