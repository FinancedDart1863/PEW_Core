//© 2023 FinancedDart
//© 2023 Phobos Engineered Weaponry Group
using ProtoBuf;

namespace PEWCore
{
    [ProtoContract(UseProtoMembersOnly = true)]
    public class PEWCoreConfig
    {
        [ProtoMember(1)]
        public PEWGeneralConfig PEWGeneralConfig = new PEWGeneralConfig();
        [ProtoMember(2)]
        public PEWHVTConfig PEWHVTConfig = new PEWHVTConfig();
        [ProtoMember(3)]
        public PEWKOTHConfig PEWKOTHConfig = new PEWKOTHConfig();
        [ProtoMember(4)]
        public PEWSSZConfig PEWSSZConfig = new PEWSSZConfig();
    }

    [ProtoContract(UseProtoMembersOnly = true)]
    public class PEWGeneralConfig
    {
        [ProtoMember(1)]
        public string PEWCore_Faction1Tag = "PPG"; //Tag of faction 1
        [ProtoMember(2)]
        public string PEWCore_Faction2Tag = "SHB"; //Tag of faction 2
        [ProtoMember(3)]
        public string PEWCore_Faction3Tag = "IWD"; //Tag of faction 3
        [ProtoMember(4)]
        public bool DeveloperMode = false; //In developer mode, code execution is described in the in-game chat
        [ProtoMember(5)]
        public bool PEWHVT_ModuleEnable = true; //Enable/disable the HVT subsystem. Do not touch this if you don't know know what you're doing. (Disabling this may cause modules with HVT subsystem dependencies to fail, such as the safezone subsystem)
        [ProtoMember(6)]
        public int PEWHVT_ExecutionInterval = 1; // Execution interval for module logic in seconds. Do not touch this if you don't know know what you're doing.
        [ProtoMember(7)]
        public bool PEWKOTH_ModuleEnable = false; //Enable/disable the HVT subsystem. Do not touch this if you don't know know what you're doing. (Disabling this may cause modules with HVT subsystem dependencies to fail, such as the safezone subsystem)
        [ProtoMember(8)]
        public int PEWKOTH_ExecutionInterval = 1; // Execution interval for module logic in seconds. Do not touch this if you don't know know what you're doing.
        [ProtoMember(9)]
        public int PEWCore_NonVolatileProgramMemory_FlushInterval = 20; ////Set time between flushes of logical core memory to 1 minuted (assuming PEWCoreExecutionInterval = 1)
    }

    [ProtoContract(UseProtoMembersOnly = true)]
    public class PEWHVTConfig
    {
        [ProtoMember(1)]
        public int PEWHVT_CheckInterval = 1200; // How often to check world for HVT grids. (seconds)
        [ProtoMember(2)]
        public int PEWHVT_HVTThreshold = 8500; //Standard threshold for a grid/grid-group to be flagged as an HVT. (Block count)
        [ProtoMember(3)]
        public int PEWHVT_HVTThresholdE52 = 11000; //Threshold for a grid/grid-group employed the E52 Special Weapon to be flagged as an HVT. (Block count)
        [ProtoMember(4)]
        public int PEWHVT_HVTClusterRadius = 500;
        [ProtoMember(5)]
        public double PEWHVT_HVTScanRadius = 150000f;
    }

    [ProtoContract(UseProtoMembersOnly = true)]
    public class PEWKOTHConfig
    {
        [ProtoMember(1)]
        public PEWKOTHInstanceConfig Hill1Config = new PEWKOTHInstanceConfig();
        [ProtoMember(2)]
        public PEWKOTHInstanceConfig Hill2Config = new PEWKOTHInstanceConfig();
        [ProtoMember(3)]
        public PEWKOTHInstanceConfig Hill3Config = new PEWKOTHInstanceConfig();
    }

    [ProtoContract(UseProtoMembersOnly = true)]
    public class PEWSSZConfig
    {
        [ProtoMember(1)]
        public float PEWSSZ_Radius = 35000f; //Amount of award item 3 to be dispensed into the controlling factions container at each interval
        [ProtoMember(2)]
        public bool PEWSSZ_AllowEnemyCharacters = false; //Amount of award item 3 to be dispensed into the controlling factions container at each interval
        [ProtoMember(3)]
        public bool PEWSSZ_AllowEnemyRemoteControlBlocks = false; //Amount of award item 3 to be dispensed into the controlling factions container at each interval
        [ProtoMember(4)]
        public int PEWSSZ_HVTActionInterval = 1200;
        [ProtoMember(5)]
        public float PEWSSZ_ShrinkDistance = 5000f;
        [ProtoMember(6)]
        public float PEWSSZ_RegenerateDistance = 1000f;
    }

    [ProtoContract(UseProtoMembersOnly = true)]
    public class PEWKOTHInstanceConfig
    {
        [ProtoMember(1)]
        public bool PEWKOTH_HillEnable = true; //Do we enable this hill?
        [ProtoMember(2)]
        public string PEWKOTH_HillName = "This Hill"; //What do we call this hill?
        [ProtoMember(3)]
        public bool PEWKOTH_HillAlwaysOn = false; //Is this hill always active if its enabled?
        [ProtoMember(4)]
        public int PEWKOTH_HillTurnOnTime = 19; //If PEWKOTH_Hill1AlwaysOn is false and PEWKOTH_Hill1Enable and true, then we assume the configurator would like the hill active at certain times. Specify turn on hour (0-23)(3 = 3 am UTC, 18 = 6 pm UTC, etc)
        [ProtoMember(5)]
        public int PEWKOTH_HillTurnOffTime = 20; //If PEWKOTH_Hill1AlwaysOn is false and PEWKOTH_Hill1Enable and true, then we assume the configurator would like the hill active at certain times. Specify turn off hour (0-23)(3 = 3 am UTC, 18 = 6 pm UTC, etc)
        [ProtoMember(6)]
        public bool PEWKOTH_HillOnSunday = true; //Is this hill active during the time frame on this day? 
        [ProtoMember(7)]
        public bool PEWKOTH_HillOnMonday = false; //Is this hill active during the time frame on this day? 
        [ProtoMember(8)]
        public bool PEWKOTH_HillOnTuesday = false; //Is this hill active during the time frame on this day? 
        [ProtoMember(9)]
        public bool PEWKOTH_HillOnWednesday = false; //Is this hill active during the time frame on this day? 
        [ProtoMember(10)]
        public bool PEWKOTH_HillOnThursday = false; //Is this hill active during the time frame on this day? 
        [ProtoMember(11)]
        public bool PEWKOTH_HillOnFriday = false; //Is this hill active during the time frame on this day? 
        [ProtoMember(12)]
        public bool PEWKOTH_HillOnSaturday = true; //Is this hill active during the time frame on this day? 
        [ProtoMember(13)]
        public string PEWKOTH_HillFaction1ContainerName = "xxxxx"; //Name of the cargo container for faction 1 at this hill in which to deposit reward items
        [ProtoMember(14)]
        public string PEWKOTH_HillFaction2ContainerName = "xxxxx"; //Name of the cargo container for faction 2 at this hill in which to deposit reward items
        [ProtoMember(15)]
        public string PEWKOTH_HillFaction3ContainerName = "xxxxx"; //Name of the cargo container for faction 3 at this hill in which to deposit reward items
        [ProtoMember(16)]
        public string PEWKOTH_HillAwardItem1Name = "xxxxx"; //Classname of the first type of item to be dispensed as an award at this hill. Leave blank for to disable this reward slot
        [ProtoMember(17)]
        public int PEWKOTH_HillAwardItem1Interval = 300; //Delay in seconds between dispensing of this award item 1 into the controlling factions container
        [ProtoMember(18)]
        public int PEWKOTH_HillAwardItem1Amount = 1; //Amount of award item 1 to be dispensed into the controlling factions container at each interval
        [ProtoMember(19)]
        public string PEWKOTH_HillAwardItem2Name = "xxxxx"; //Classname of the second type of item to be dispensed as an award at this hill. Leave blank for to disable this reward slot
        [ProtoMember(20)]
        public int PEWKOTH_HillAwardItem2Interval = 300; //Delay in seconds between dispensing of this award item 2 into the controlling factions container
        [ProtoMember(21)]
        public int PEWKOTH_HillAwardItem2Amount = 1; //Amount of award item 2 to be dispensed into the controlling factions container at each interval
        [ProtoMember(22)]
        public string PEWKOTH_HillAwardItem3Name = "xxxxx"; //Classname of the third type of item to be dispensed as an award at this hill. Leave blank for to disable this reward slot
        [ProtoMember(23)]
        public int PEWKOTH_HillAwardItem3Interval = 300; //Delay in seconds between dispensing of this award item 3 into the controlling factions container
        [ProtoMember(24)]
        public int PEWKOTH_HillAwardItem3Amount = 1; //Amount of award item 3 to be dispensed into the controlling factions container at each interval
        [ProtoMember(25)]
        public double PEWKOTH_HillScanRadius = 150f; //Scan radius in meters for this hill
    }
}