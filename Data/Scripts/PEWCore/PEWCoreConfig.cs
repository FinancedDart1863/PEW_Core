//© FinancedDart
//© Phobos Engineered Weaponry Group
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
    }

    public class PEWGeneralConfig
    {
        [ProtoMember(1)]
        public string PEWCore_Faction1Tag = "xxxxx"; //Tag of faction 1
        [ProtoMember(2)]
        public string PEWCore_Faction2Tag = "xxxxx"; //Tag of faction 2
        [ProtoMember(3)]
        public string PEWCore_Faction3Tag = "xxxxx"; //Tag of faction 3
        [ProtoMember(4)]
        public bool DeveloperMode = true; //In developer mode, code execution is described in the in-game chat
        [ProtoMember(5)]
        public bool PEWHVT_ModuleEnable = true; //Enable/disable the HVT subsystem. Do not touch this if you don't know know what you're doing. (Disabling this may cause modules with HVT subsystem dependencies to fail, such as the safezone subsystem)
        [ProtoMember(6)]
        public int PEWHVT_ExecutionInterval = 1; // Execution interval for module logic in seconds. Do not touch this if you don't know know what you're doing.
        [ProtoMember(7)]
        public bool PEWKOTH_ModuleEnable = true; //Enable/disable the HVT subsystem. Do not touch this if you don't know know what you're doing. (Disabling this may cause modules with HVT subsystem dependencies to fail, such as the safezone subsystem)
        [ProtoMember(8)]
        public int PEWKOTH_ExecutionInterval = 1; // Execution interval for module logic in seconds. Do not touch this if you don't know know what you're doing.
    }

    [ProtoContract(UseProtoMembersOnly = true)]
    public class PEWHVTConfig
    {
        [ProtoMember(1)]
        public int PEWHVT_CheckInterval = 3600; // How often to check world for HVT grids. (seconds)
        [ProtoMember(2)]
        public int PEWHVT_HVTThreshold = 7500; //Standard threshold for a grid/grid-group to be flagged as an HVT. (Block count)
        [ProtoMember(3)]
        public int PEWHVT_HVTThresholdE52 = 11000; //Threshold for a grid/grid-group employed the E52 Special Weapon to be flagged as an HVT. (Block count)
    }

    [ProtoContract(UseProtoMembersOnly = true)]
    public class PEWKOTHConfig
    {
        //King of the hill default settings
        [ProtoMember(1)]
        public bool PEWKOTH_Hill1Enable = true; //Do we enable hill 1?
        [ProtoMember(2)]
        public string PEWKOTH_Hill1Name = "Hill 1"; //What do we call hill 1?
        [ProtoMember(3)]
        public bool PEWKOTH_Hill1AlwaysOn = false; //Is hill 1 always active if its enabled?
        [ProtoMember(4)]
        public int PEWKOTH_Hill1TurnOnTime = 19; //If PEWKOTH_Hill1AlwaysOn is false and PEWKOTH_Hill1Enable and true, then we assume the configurator would like the hill active at certain times. Specify turn on hour (0-23)(3 = 3 am UTC, 18 = 6 pm UTC, etc)
        [ProtoMember(5)]
        public int PEWKOTH_Hill1TurnOffTime = 20; //If PEWKOTH_Hill1AlwaysOn is false and PEWKOTH_Hill1Enable and true, then we assume the configurator would like the hill active at certain times. Specify turn off hour (0-23)(3 = 3 am UTC, 18 = 6 pm UTC, etc)
        [ProtoMember(6)]
        public bool PEWKOTH_Hill1OnSunday = true; //Is hill 1 active during the time frame on this day? 
        [ProtoMember(7)]
        public bool PEWKOTH_Hill1OnMonday = false; //Is hill 1 active during the time frame on this day? 
        [ProtoMember(8)]
        public bool PEWKOTH_Hill1OnTuesday = false; //Is hill 1 active during the time frame on this day? 
        [ProtoMember(9)]
        public bool PEWKOTH_Hill1OnWednesday = false; //Is hill 1 active during the time frame on this day? 
        [ProtoMember(10)]
        public bool PEWKOTH_Hill1OnThursday = false; //Is hill 1 active during the time frame on this day? 
        [ProtoMember(11)]
        public bool PEWKOTH_Hill1OnFriday = false; //Is hill 1 active during the time frame on this day? 
        [ProtoMember(12)]
        public bool PEWKOTH_Hill1OnSaturday = true; //Is hill 1 active during the time frame on this day? 
        [ProtoMember(13)]
        public string PEWKOTH_Hill1Faction1ContainerName = "xxxxx"; //Name of the cargo container for faction 1 at hill 1 in which to deposit reward items
        [ProtoMember(14)]
        public string PEWKOTH_Hill1Faction2ContainerName = "xxxxx"; //Name of the cargo container for faction 2 at hill 1 in which to deposit reward items
        [ProtoMember(15)]
        public string PEWKOTH_Hill1Faction3ContainerName = "xxxxx"; //Name of the cargo container for faction 3 at hill 1 in which to deposit reward items
        [ProtoMember(16)]
        public string PEWKOTH_Hill1AwardItem1Name = "xxxxx"; //Classname of the first type of item to be dispensed as an award at hill 1. Leave blank for to disable this reward slot
        [ProtoMember(17)]
        public int PEWKOTH_Hill1AwardItem1Interval = 300; //Delay in seconds between dispensing of this award item 1 into the controlling factions container
        [ProtoMember(18)]
        public int PEWKOTH_Hill1AwardItem1Amount = 1; //Amount of award item 1 to be dispensed into the controlling factions container at each interval
        [ProtoMember(19)]
        public string PEWKOTH_Hill1AwardItem2Name = "xxxxx"; //Classname of the second type of item to be dispensed as an award at hill 1. Leave blank for to disable this reward slot
        [ProtoMember(20)]
        public int PEWKOTH_Hill1AwardItem2Interval = 300; //Delay in seconds between dispensing of this award item 2 into the controlling factions container
        [ProtoMember(21)]
        public int PEWKOTH_Hill1AwardItem2Amount = 1; //Amount of award item 2 to be dispensed into the controlling factions container at each interval
        [ProtoMember(22)]
        public string PEWKOTH_Hill1AwardItem3Name = "xxxxx"; //Classname of the third type of item to be dispensed as an award at hill 1. Leave blank for to disable this reward slot
        [ProtoMember(23)]
        public int PEWKOTH_Hill1AwardItem3Interval = 300; //Delay in seconds between dispensing of this award item 3 into the controlling factions container
        [ProtoMember(24)]
        public int PEWKOTH_Hill1AwardItem3Amount = 1; //Amount of award item 3 to be dispensed into the controlling factions container at each interval
        
        [ProtoMember(25)]
        public bool PEWKOTH_Hill2Enable = true; //Do we enable hill 2?
        [ProtoMember(26)]
        public string PEWKOTH_Hill2Name = "Hill 2"; //What do we call hill 2?
        [ProtoMember(27)]
        public bool PEWKOTH_Hill2AlwaysOn = false; //Is hill 2 always active if its enabled?
        [ProtoMember(28)]
        public int PEWKOTH_Hill2TurnOnTime = 19; //If PEWKOTH_Hill2AlwaysOn is false and PEWKOTH_Hill2Enable and true, then we assume the configurator would like the hill active at certain times. Specify turn on hour (0-23)(3 = 3 am UTC, 18 = 6 pm UTC, etc)
        [ProtoMember(29)]
        public int PEWKOTH_Hill2TurnOffTime = 20; //If PEWKOTH_Hill2AlwaysOn is false and PEWKOTH_Hill2Enable and true, then we assume the configurator would like the hill active at certain times. Specify turn off hour (0-23)(3 = 3 am UTC, 18 = 6 pm UTC, etc)
        [ProtoMember(30)]
        public bool PEWKOTH_Hill2OnSunday = true; //Is hill 2 active during the time frame on this day? 
        [ProtoMember(31)]
        public bool PEWKOTH_Hill2OnMonday = false; //Is hill 2 active during the time frame on this day? 
        [ProtoMember(32)]
        public bool PEWKOTH_Hill2OnTuesday = false; //Is hill 2 active during the time frame on this day? 
        [ProtoMember(33)]
        public bool PEWKOTH_Hill2OnWednesday = false; //Is hill 2 active during the time frame on this day? 
        [ProtoMember(34)]
        public bool PEWKOTH_Hill2OnThursday = false; //Is hill 2 active during the time frame on this day? 
        [ProtoMember(35)]
        public bool PEWKOTH_Hill2OnFriday = false; //Is hill 2 active during the time frame on this day? 
        [ProtoMember(36)]
        public bool PEWKOTH_Hill2OnSaturday = true; //Is hill 2 active during the time frame on this day? 
        [ProtoMember(37)]
        public string PEWKOTH_Hill2Faction1ContainerName = "xxxxx"; //Name of the cargo container for faction 1 at hill 2 in which to deposit reward items
        [ProtoMember(38)]
        public string PEWKOTH_Hill2Faction2ContainerName = "xxxxx"; //Name of the cargo container for faction 2 at hill 2 in which to deposit reward items
        [ProtoMember(39)]
        public string PEWKOTH_Hill2Faction3ContainerName = "xxxxx"; //Name of the cargo container for faction 3 at hill 2 in which to deposit reward items
        [ProtoMember(40)]
        public string PEWKOTH_Hill2AwardItem1Name = "xxxxx"; //Classname of the first type of item to be dispensed as an award at hill 2. Leave blank for to disable this reward slot
        [ProtoMember(41)]
        public int PEWKOTH_Hill2AwardItem1Interval = 300; //Delay in seconds between dispensing of this award item 1 into the controlling factions container
        [ProtoMember(42)]
        public int PEWKOTH_Hill2AwardItem1Amount = 1; //Amount of award item 1 to be dispensed into the controlling factions container at each interval
        [ProtoMember(43)]
        public string PEWKOTH_Hill2AwardItem2Name = "xxxxx"; //Classname of the second type of item to be dispensed as an award at hill 2. Leave blank for to disable this reward slot
        [ProtoMember(44)]
        public int PEWKOTH_Hill2AwardItem2Interval = 300; //Delay in seconds between dispensing of this award item 2 into the controlling factions container
        [ProtoMember(45)]
        public int PEWKOTH_Hill2AwardItem2Amount = 1; //Amount of award item 2 to be dispensed into the controlling factions container at each interval
        [ProtoMember(46)]
        public string PEWKOTH_Hill2AwardItem3Name = "xxxxx"; //Classname of the third type of item to be dispensed as an award at hill 2. Leave blank for to disable this reward slot
        [ProtoMember(47)]
        public int PEWKOTH_Hill2AwardItem3Interval = 300; //Delay in seconds between dispensing of this award item 3 into the controlling factions container
        [ProtoMember(48)]
        public int PEWKOTH_Hill2AwardItem3Amount = 1; //Amount of award item 3 to be dispensed into the controlling factions container at each interval

        [ProtoMember(49)]
        public bool PEWKOTH_Hill3Enable = true; //Do we enable hill 3?
        [ProtoMember(50)]
        public string PEWKOTH_Hill3Name = "Hill 3"; //What do we call hill 3?
        [ProtoMember(51)]
        public bool PEWKOTH_Hill3AlwaysOn = false; //Is hill 3 always active if its enabled?
        [ProtoMember(52)]
        public int PEWKOTH_Hill3TurnOnTime = 19; //If PEWKOTH_Hill3AlwaysOn is false and PEWKOTH_Hill3Enable and true, then we assume the configurator would like the hill active at certain times. Specify turn on hour (0-23)(3 = 3 am UTC, 18 = 6 pm UTC, etc)
        [ProtoMember(53)]
        public int PEWKOTH_Hill3TurnOffTime = 20; //If PEWKOTH_Hill3AlwaysOn is false and PEWKOTH_Hill3Enable and true, then we assume the configurator would like the hill active at certain times. Specify turn off hour (0-23)(3 = 3 am UTC, 18 = 6 pm UTC, etc)
        [ProtoMember(54)]
        public bool PEWKOTH_Hill3OnSunday = true; //Is hill 3 active during the time frame on this day?
        [ProtoMember(55)]
        public bool PEWKOTH_Hill3OnMonday = false; //Is hill 3 active during the time frame on this day?
        [ProtoMember(56)]
        public bool PEWKOTH_Hill3OnTuesday = false; //Is hill 3 active during the time frame on this day?
        [ProtoMember(57)]
        public bool PEWKOTH_Hill3OnWednesday = false; //Is hill 3 active during the time frame on this day?
        [ProtoMember(58)]
        public bool PEWKOTH_Hill3OnThursday = false; //Is hill 3 active during the time frame on this day?
        [ProtoMember(59)]
        public bool PEWKOTH_Hill3OnFriday = false; //Is hill 3 active during the time frame on this day?
        [ProtoMember(60)]
        public bool PEWKOTH_Hill3OnSaturday = true; //Is hill 3 active during the time frame on this day?
        [ProtoMember(61)]
        public string PEWKOTH_Hill3Faction1ContainerName = "xxxxx"; //Name of the cargo container for faction 1 at hill 3 in which to deposit reward items
        [ProtoMember(62)]
        public string PEWKOTH_Hill3Faction2ContainerName = "xxxxx"; //Name of the cargo container for faction 2 at hill 3 in which to deposit reward items
        [ProtoMember(63)]
        public string PEWKOTH_Hill3Faction3ContainerName = "xxxxx"; //Name of the cargo container for faction 3 at hill 3 in which to deposit reward items
        [ProtoMember(64)]
        public string PEWKOTH_Hill3AwardItem1Name = "xxxxx"; //Classname of the first type of item to be dispensed as an award at hill 3. Leave blank for to disable this reward slot
        [ProtoMember(65)]
        public int PEWKOTH_Hill3AwardItem1Interval = 300; //Delay in seconds between dispensing of this award item 1 into the controlling factions container
        [ProtoMember(66)]
        public int PEWKOTH_Hill3AwardItem1Amount = 1; //Amount of award item 1 to be dispensed into the controlling factions container at each interval
        [ProtoMember(67)]
        public string PEWKOTH_Hill3AwardItem2Name = "xxxxx"; //Classname of the second type of item to be dispensed as an award at hill 3. Leave blank for to disable this reward slot
        [ProtoMember(68)]
        public int PEWKOTH_Hill3AwardItem2Interval = 300; //Delay in seconds between dispensing of this award item 2 into the controlling factions container
        [ProtoMember(69)]
        public int PEWKOTH_Hill3AwardItem2Amount = 1; //Amount of award item 2 to be dispensed into the controlling factions container at each interval
        [ProtoMember(70)]
        public string PEWKOTH_Hill3AwardItem3Name = "xxxxx"; //Classname of the third type of item to be dispensed as an award at hill 3. Leave blank for to disable this reward slot
        [ProtoMember(71)]
        public int PEWKOTH_Hill3AwardItem3Interval = 300; //Delay in seconds between dispensing of this award item 3 into the controlling factions container
        [ProtoMember(72)]
        public int PEWKOTH_Hill3AwardItem3Amount = 1; //Amount of award item 3 to be dispensed into the controlling factions container at each interval
    }
}
