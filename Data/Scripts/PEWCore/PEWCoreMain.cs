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

namespace PEWCore
{
    //Default values for HVT detector should a config not be found
    public struct PEWCoreSettings
    {
        //PEWCore loads most settings at mod initialization. We take a modular approach for settings as we do for core gameplay features.
        //The settings here are not available for customization via the config
        public static int PEWCoreExecutionInterval = 1;
    }

    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class PEWCoreMain : MySessionComponentBase
    {
        //Prepare to read configuration file
        public PEWCoreConfig ConfigData; //All configuration data will become accessible via this handle
        private const string CONFIG_FILE_NAME = "PEWCoreConfig.xml";

        private static DateTime lastUpdate;
        private bool CoreInitialized;

        private void Initialize()
        {
            CoreInitialized = true;

            //SetupNetwork();

            if (!MyAPIGateway.Multiplayer.IsServer) //Do not proceed if this mod isn't being run on a server
            {
                PEWCoreLogging.Instance.WriteLine("PCCore - Client Mode");
                MyAPIGateway.Utilities.ShowMessage("PCCore", "Client Mode");
            }

            if (MyAPIGateway.Multiplayer.IsServer)
            {
                PEWCoreLogging.Instance.WriteLine("PCCore - Server Mode");
                MyAPIGateway.Utilities.ShowMessage("PCCore", "Server Mode");
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
                if (!MyAPIGateway.Utilities.FileExistsInWorldStorage(CONFIG_FILE_NAME, typeof(PEWCoreConfig)))
                {
                    MyAPIGateway.Parallel.Start(() =>
                    {
                        using (var sw = MyAPIGateway.Utilities.WriteFileInWorldStorage(CONFIG_FILE_NAME, typeof(PEWCoreConfig))) sw.Write(MyAPIGateway.Utilities.SerializeToXML<PEWCoreConfig>(new PEWCoreConfig()));
                    });
                }

                //Attempt to read the configuration file
                try
                {
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
                    ConfigData = new PEWCoreConfig();
                    //MyLog.Default.WriteLineAndConsole($"ERROR: {exc.Message} : {exc.StackTrace} : {exc.InnerException}");
                }
            }
            else
            {
                ConfigData = new PEWCoreConfig(); //Don't bother with configuration files if client. Just load default ConfigData and wait for synchronization from server
            }
        }

        public void SetupNetwork()
        {

        }

        public override void UpdateBeforeSimulation()
        {
            //Sanity check
            if (MyAPIGateway.Session == null)
                return;

            //Initalize mod if we haven't already
            if (!CoreInitialized)
                Initialize();

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
                //HVTDetectorProcess.Process();


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

            //HVTDetectorProcess.clearTimer.Stop();

            base.UnloadData();
        }
    }
}