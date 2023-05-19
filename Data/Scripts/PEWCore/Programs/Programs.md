## Phobos Engineered Weaponry - MP Core Program Documentation

Documentation of programs, their inputs, and sample instruction sets are provided below.  

## Timed Dispenser

The timed dispenser program will dispense a predefined set of items into a cargo container on a specified  
interval. The interval can be changed, as can the items to be dispensed, their maximum amounts, and the  
rate of dispense.

Place a logical core on the grid containing the cargo container that you wish to dispense items into.   
Name the cargo container accordingly.  

=== Instruction Set Template ===  

ProgramName  
ProgramInterval (This must be either greatest common denominator or least common denominator of item dispense intervals)  
ProgramMode  ("TopOff" or "BlindFill")  
DispenseCargoContainerName (Name of cargo container you want to program to place the items in)  
Component1ClassType (See class type and type list)  
Component1Type  (See class type and type list)  
Component1DispenseInterval (Interval to dispense item 1)  
Component1AmountPerDispense (How many of item 1 to dispense per interval)  
Component1MaxInventoryAmount (Max amount of item 1 to be in the cargo container at a given time)  
Component2ClassType  
Component2Type  
Component2DispenseInterval  
Component2AmountPerDispense  
Component2MaxInventoryAmount  
Component3ClassType  
Component3Type  
Component3DispenseInterval  
Component3AmountPerDispense  
Component3MaxInventoryAmount  

=== Instruction Set Sample ===  
istr0:BasicTimedDispenser*<br />
istr1:5*<br />
istr2:TopOff*<br />
istr3:referencecargo01*<br />
istr4:Component*<br />
istr5:SteelPlate*<br />
istr6:5*<br />
istr7:25*<br />
istr8:120*<br />
istr9:PhysicalObject*<br />
istr10:ZoneChip*<br />
istr11:20*<br />
istr12:100*<br />
istr13:180*<br />
istr14:MyObjectBuilder_Type*<br />
istr15:platinum_ingot*<br />
istr16:10*<br />
istr17:1000*<br />
istr18:240*<br />

### Class type and type list

**Component**  
    - Construction  
    - MetalGrid  
    - InteriorPlate  
    - SteelPlate  
    - Girder  
    - SmallTube  
    - LargeTube  
    - Motor  
    - Display  
    - BulletproofGlass  
    - Superconductor  
    - Computer  
    - Reactor  
    - Thrust  
    - GravityGenerator  
    - Medical  
    - RadioCommunication  
    - Detector  
    - Explosives  
    - SolarCell  
    - PowerCell  
    - Canvas  
    - EngineerPlushie  
**PhysicalObject**  
    - SpaceCredit  
**ConsumableItem**   
    - Medkit  
    - Powerkit  
    - ClangCola  
    - CosmicCoffee  
**Datapad**   
    - Datapad  
**Ore**  
    - Stone  
    - Iron  
    - Nickel  
    - Cobalt  
    - Magnesium  
    - Silicon  
    - Silver  
    - Gold  
    - Platinum  
    - Uranium  
    - Scrap  
    - Ice  
    - Organic   
**Ingot**  
    - Stone (Gravel)  
    - Iron  
    - Nickel  
    - Cobalt  
    - Magnesium  
    - Silicon  
    - Silver  
    - Gold  
    - Platinum  
    - Uranium  

## Faction assignment manager

The faction assignment manager handles adding players to factions and keeping them on the respective factions.  

Place a logical core. Place three logical zones and name them to something you can remember for referencing them.  

Add the following instruction set to the logical core, replacing bracketed prompts with your references and the   
appropriate information. Make sure to end each line with a * symbol!

=== Instruction Set Sample ===  
istr0:GeneralFactionAssigner*<br />
istr1:3*<br />
istr2:null*<br />
istr3:[faction1Tag]<br />
istr4:[faction1ZoneAssignName]<br />
istr5:[faction2Tag]<br />
istr6:[faction2ZoneAssignName]<br />
istr7:[faction3Tag]<br />
istr8:[faction3ZoneAssignName]<br />