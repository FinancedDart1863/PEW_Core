using ProtoBuf;

namespace PEWCore
{
    [ProtoContract(UseProtoMembersOnly = true)]
    public class PEWCoreConfig
    {
        [ProtoMember(1)]
        public PEWGeneralConfig PEWGeneralConfigReference = new PEWGeneralConfig();
        [ProtoMember(2)]
        public PEWHVTConfig PEWHVTConfigReference = new PEWHVTConfig();
        [ProtoMember(3)]
        public PEWKOTHConfig PEWKOTHConfigReference = new PEWKOTHConfig();
    }

    public class PEWGeneralConfig
    {
        [ProtoMember(1)]
        public static string PEWCore_Faction1Tag = ""; //Tag of faction 1
        [ProtoMember(2)]
        public static string PEWCore_Faction2Tag = ""; //Tag of faction 2
        [ProtoMember(3)]
        public static string PEWCore_Faction3Tag = ""; //Tag of faction 3
        [ProtoMember(4)]
        public static bool DeveloperMode = true; //In developer mode, code execution is described in the in-game chat
    }

    [ProtoContract(UseProtoMembersOnly = true)]
    public class PEWHVTConfig
    {
        [ProtoMember(1)]
        public static int PEWHVT_CheckInterval = 3600; // How often to check world for HVT grids. (seconds)
        [ProtoMember(2)]
        public static int PEWHVT_ExecutionInterval = 1; // Execution interval for mod logic. (seconds) 
        [ProtoMember(3)]
        public static int PEWHVT_HVTThreshold = 7500; //Standard threshold for a grid/grid-group to be flagged as an HVT. (Block count)
        [ProtoMember(4)]
        public static int PEWHVT_HVTThresholdE52 = 11000; //Threshold for a grid/grid-group employed the E52 Special Weapon to be flagged as an HVT. (Block count)
    }

    [ProtoContract(UseProtoMembersOnly = true)]
    public class PEWKOTHConfig
    {
        //King of the hill default settings
        [ProtoMember(1)]
        public static bool PEWKOTH_Hill1Enable = true; //Do we enable hill 1?
        [ProtoMember(2)]
        public static string PEWKOTH_Hill1Name = "Hill 1"; //What do we call hill 1?
        [ProtoMember(3)]
        public static bool PEWKOTH_Hill1AlwaysOn = false; //Is hill 1 always active if its enabled?
        [ProtoMember(4)]
        public static int PEWWKOTH_Hill1TurnOnTime = 19; //If PEWKOTH_Hill1AlwaysOn is false and PEWKOTH_Hill1Enable and true, then we assume the configurator would like the hill active at certain times. Specify turn on hour (0-23)(3 = 3 am UTC, 18 = 6 pm UTC, etc)
        [ProtoMember(5)]
        public static int PEWWKOTH_Hill1TurnOffTime = 20; //If PEWKOTH_Hill1AlwaysOn is false and PEWKOTH_Hill1Enable and true, then we assume the configurator would like the hill active at certain times. Specify turn off hour (0-23)(3 = 3 am UTC, 18 = 6 pm UTC, etc)
        [ProtoMember(6)]
        public static bool PEWKOTH_Hill1OnSunday = true; //Is hill 1 active during the time frame on this day? 
        [ProtoMember(7)]
        public static bool PEWKOTH_Hill1OnMonday = false; //Is hill 1 active during the time frame on this day? 
        [ProtoMember(8)]
        public static bool PEWKOTH_Hill1OnTuesday = false; //Is hill 1 active during the time frame on this day? 
        [ProtoMember(9)]
        public static bool PEWKOTH_Hill1OnWednesday = false; //Is hill 1 active during the time frame on this day? 
        [ProtoMember(10)]
        public static bool PEWKOTH_Hill1OnThursday = false; //Is hill 1 active during the time frame on this day? 
        [ProtoMember(11)]
        public static bool PEWKOTH_Hill1OnFriday = false; //Is hill 1 active during the time frame on this day? 
        [ProtoMember(12)]
        public static bool PEWKOTH_Hill1OnSaturday = true; //Is hill 1 active during the time frame on this day? 
        [ProtoMember(13)]
        public static string PEWKOTH_Hill1Faction1ContainerName = ""; //Name of the cargo container for faction 1 at hill 1 in which to deposit reward items
        [ProtoMember(14)]
        public static string PEWKOTH_Hill1Faction2ContainerName = ""; //Name of the cargo container for faction 2 at hill 1 in which to deposit reward items
        [ProtoMember(15)]
        public static string PEWKOTH_Hill1Faction3ContainerName = ""; //Name of the cargo container for faction 3 at hill 1 in which to deposit reward items
        [ProtoMember(16)]
        public static string PEWKOTH_Hill1AwardItem1Name = ""; //Classname of the first type of item to be dispensed as an award at hill 1. Leave blank for to disable this reward slot
        [ProtoMember(17)]
        public static int PEWKOTH_Hill1AwardItem1Interval = 300; //Delay in seconds between dispensing of this award item 1 into the controlling factions container
        [ProtoMember(18)]
        public static int PEWKOTH_Hill1AwardItem1Amount = 1; //Amount of award item 1 to be dispensed into the controlling factions container at each interval
        [ProtoMember(19)]
        public static string PEWKOTH_Hill1AwardItem2Name = ""; //Classname of the second type of item to be dispensed as an award at hill 1. Leave blank for to disable this reward slot
        [ProtoMember(20)]
        public static int PEWKOTH_Hill1AwardItem2Interval = 300; //Delay in seconds between dispensing of this award item 2 into the controlling factions container
        [ProtoMember(21)]
        public static int PEWKOTH_Hill1AwardItem2Amount = 1; //Amount of award item 2 to be dispensed into the controlling factions container at each interval
        [ProtoMember(22)]
        public static string PEWKOTH_Hill1AwardItem3Name = ""; //Classname of the third type of item to be dispensed as an award at hill 1. Leave blank for to disable this reward slot
        [ProtoMember(23)]
        public static int PEWKOTH_Hill1AwardItem3Interval = 300; //Delay in seconds between dispensing of this award item 3 into the controlling factions container
        [ProtoMember(24)]
        public static int PEWKOTH_Hill1AwardItem3Amount = 1; //Amount of award item 3 to be dispensed into the controlling factions container at each interval
        
        [ProtoMember(25)]
        public static bool PEWKOTH_Hill2Enable = true; //Do we enable hill 2?
        [ProtoMember(26)]
        public static string PEWKOTH_Hill2Name = "Hill 2"; //What do we call hill 2?
        [ProtoMember(27)]
        public static bool PEWKOTH_Hill2AlwaysOn = false; //Is hill 2 always active if its enabled?
        [ProtoMember(28)]
        public static int PEWWKOTH_Hill2TurnOnTime = 19; //If PEWKOTH_Hill2AlwaysOn is false and PEWKOTH_Hill2Enable and true, then we assume the configurator would like the hill active at certain times. Specify turn on hour (0-23)(3 = 3 am UTC, 18 = 6 pm UTC, etc)
        [ProtoMember(29)]
        public static int PEWWKOTH_Hill2TurnOffTime = 20; //If PEWKOTH_Hill2AlwaysOn is false and PEWKOTH_Hill2Enable and true, then we assume the configurator would like the hill active at certain times. Specify turn off hour (0-23)(3 = 3 am UTC, 18 = 6 pm UTC, etc)
        [ProtoMember(30)]
        public static bool PEWKOTH_Hill2OnSunday = true; //Is hill 2 active during the time frame on this day? 
        [ProtoMember(31)]
        public static bool PEWKOTH_Hill2OnMonday = false; //Is hill 2 active during the time frame on this day? 
        [ProtoMember(32)]
        public static bool PEWKOTH_Hill2OnTuesday = false; //Is hill 2 active during the time frame on this day? 
        [ProtoMember(33)]
        public static bool PEWKOTH_Hill2OnWednesday = false; //Is hill 2 active during the time frame on this day? 
        [ProtoMember(34)]
        public static bool PEWKOTH_Hill2OnThursday = false; //Is hill 2 active during the time frame on this day? 
        [ProtoMember(35)]
        public static bool PEWKOTH_Hill2OnFriday = false; //Is hill 2 active during the time frame on this day? 
        [ProtoMember(36)]
        public static bool PEWKOTH_Hill2OnSaturday = true; //Is hill 2 active during the time frame on this day? 
        [ProtoMember(37)]
        public static string PEWKOTH_Hill2Faction1ContainerName = ""; //Name of the cargo container for faction 1 at hill 2 in which to deposit reward items
        [ProtoMember(38)]
        public static string PEWKOTH_Hill2Faction2ContainerName = ""; //Name of the cargo container for faction 2 at hill 2 in which to deposit reward items
        [ProtoMember(39)]
        public static string PEWKOTH_Hill2Faction3ContainerName = ""; //Name of the cargo container for faction 3 at hill 2 in which to deposit reward items
        [ProtoMember(40)]
        public static string PEWKOTH_Hill2AwardItem1Name = ""; //Classname of the first type of item to be dispensed as an award at hill 2. Leave blank for to disable this reward slot
        [ProtoMember(41)]
        public static int PEWKOTH_Hill2AwardItem1Interval = 300; //Delay in seconds between dispensing of this award item 1 into the controlling factions container
        [ProtoMember(42)]
        public static int PEWKOTH_Hill2AwardItem1Amount = 1; //Amount of award item 1 to be dispensed into the controlling factions container at each interval
        [ProtoMember(43)]
        public static string PEWKOTH_Hill2AwardItem2Name = ""; //Classname of the second type of item to be dispensed as an award at hill 2. Leave blank for to disable this reward slot
        [ProtoMember(44)]
        public static int PEWKOTH_Hill2AwardItem2Interval = 300; //Delay in seconds between dispensing of this award item 2 into the controlling factions container
        [ProtoMember(45)]
        public static int PEWKOTH_Hill2AwardItem2Amount = 1; //Amount of award item 2 to be dispensed into the controlling factions container at each interval
        [ProtoMember(46)]
        public static string PEWKOTH_Hill2AwardItem3Name = ""; //Classname of the third type of item to be dispensed as an award at hill 2. Leave blank for to disable this reward slot
        [ProtoMember(47)]
        public static int PEWKOTH_Hill2AwardItem3Interval = 300; //Delay in seconds between dispensing of this award item 3 into the controlling factions container
        [ProtoMember(48)]
        public static int PEWKOTH_Hill2AwardItem3Amount = 1; //Amount of award item 3 to be dispensed into the controlling factions container at each interval

        [ProtoMember(49)]
        public static bool PEWKOTH_Hill3Enable = true; //Do we enable hill 3?
        [ProtoMember(50)]
        public static string PEWKOTH_Hill3Name = "Hill 3"; //What do we call hill 3?
        [ProtoMember(51)]
        public static bool PEWKOTH_Hill3AlwaysOn = false; //Is hill 3 always active if its enabled?
        [ProtoMember(52)]
        public static int PEWWKOTH_Hill3TurnOnTime = 19; //If PEWKOTH_Hill3AlwaysOn is false and PEWKOTH_Hill3Enable and true, then we assume the configurator would like the hill active at certain times. Specify turn on hour (0-23)(3 = 3 am UTC, 18 = 6 pm UTC, etc)
        [ProtoMember(53)]
        public static int PEWWKOTH_Hill3TurnOffTime = 20; //If PEWKOTH_Hill3AlwaysOn is false and PEWKOTH_Hill3Enable and true, then we assume the configurator would like the hill active at certain times. Specify turn off hour (0-23)(3 = 3 am UTC, 18 = 6 pm UTC, etc)
        [ProtoMember(54)]
        public static bool PEWKOTH_Hill3OnSunday = true; //Is hill 3 active during the time frame on this day?
        [ProtoMember(55)]
        public static bool PEWKOTH_Hill3OnMonday = false; //Is hill 3 active during the time frame on this day?
        [ProtoMember(56)]
        public static bool PEWKOTH_Hill3OnTuesday = false; //Is hill 3 active during the time frame on this day?
        [ProtoMember(57)]
        public static bool PEWKOTH_Hill3OnWednesday = false; //Is hill 3 active during the time frame on this day?
        [ProtoMember(58)]
        public static bool PEWKOTH_Hill3OnThursday = false; //Is hill 3 active during the time frame on this day?
        [ProtoMember(59)]
        public static bool PEWKOTH_Hill3OnFriday = false; //Is hill 3 active during the time frame on this day?
        [ProtoMember(60)]
        public static bool PEWKOTH_Hill3OnSaturday = true; //Is hill 3 active during the time frame on this day?
        [ProtoMember(61)]
        public static string PEWKOTH_Hill3Faction1ContainerName = ""; //Name of the cargo container for faction 1 at hill 3 in which to deposit reward items
        [ProtoMember(62)]
        public static string PEWKOTH_Hill3Faction2ContainerName = ""; //Name of the cargo container for faction 2 at hill 3 in which to deposit reward items
        [ProtoMember(63)]
        public static string PEWKOTH_Hill3Faction3ContainerName = ""; //Name of the cargo container for faction 3 at hill 3 in which to deposit reward items
        [ProtoMember(64)]
        public static string PEWKOTH_Hill3AwardItem1Name = ""; //Classname of the first type of item to be dispensed as an award at hill 3. Leave blank for to disable this reward slot
        [ProtoMember(65)]
        public static int PEWKOTH_Hill3AwardItem1Interval = 300; //Delay in seconds between dispensing of this award item 1 into the controlling factions container
        [ProtoMember(66)]
        public static int PEWKOTH_Hill3AwardItem1Amount = 1; //Amount of award item 1 to be dispensed into the controlling factions container at each interval
        [ProtoMember(67)]
        public static string PEWKOTH_Hill3AwardItem2Name = ""; //Classname of the second type of item to be dispensed as an award at hill 3. Leave blank for to disable this reward slot
        [ProtoMember(68)]
        public static int PEWKOTH_Hill3AwardItem2Interval = 300; //Delay in seconds between dispensing of this award item 2 into the controlling factions container
        [ProtoMember(69)]
        public static int PEWKOTH_Hill3AwardItem2Amount = 1; //Amount of award item 2 to be dispensed into the controlling factions container at each interval
        [ProtoMember(70)]
        public static string PEWKOTH_Hill3AwardItem3Name = ""; //Classname of the third type of item to be dispensed as an award at hill 3. Leave blank for to disable this reward slot
        [ProtoMember(71)]
        public static int PEWKOTH_Hill3AwardItem3Interval = 300; //Delay in seconds between dispensing of this award item 3 into the controlling factions container
        [ProtoMember(72)]
        public static int PEWKOTH_Hill3AwardItem3Amount = 1; //Amount of award item 3 to be dispensed into the controlling factions container at each interval
    }
}
