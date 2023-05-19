## Phobos Engineered Weaponry - MP Core Program Documentation

Documentation of programs, their inputs, and sample instruction sets are provided below

## Timed Dispenser

The timed dispenser program will dispense a predefined set of items into a cargo container on a specified
interval. The interval can be changed, as can the items to be dispensed, their maximum amounts, and the
rate of dispense.

=== Instruction Set Template ===

ProgramName
ProgramInterval (This must be either greatest common denominator or least common denominator of item dispense intervals)
ProgramMode  ("TopOff" or "BlindFill")
DispenseCargoContainerName (Name of cargo container you want to program to place the items in)
Component1ClassType (See class type and type list)
Component1Type
Component1DispenseInterval
Component1AmountPerDispense
Component1MaxInventoryAmount
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

=== Sample set ===
istr0:BasicTimedDispenser*
istr1:5*
istr2:topoff*
istr3:referencecargo01*
istr4:Component*
istr5:SteelPlate*
istr6:5*
istr7:25*
istr8:120*
istr9:PhysicalObject*
istr10:ZoneChip*
istr11:2*
istr12:100*
istr13:180*
istr14:MyObjectBuilder_Type
istr15:platinum_ingot*
istr16:10*
istr17:1000*
istr18:240*

### Class type and type list

= Component =
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
	
= PhysicalObject =
    - SpaceCredit
	
= ConsumableItem =
    - Medkit
    - Powerkit
    - ClangCola
    - CosmicCoffee
	
= Datapad =
    - Datapad
	
= Ore =
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
	
= Ingot =
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

## Timed Dispenser

The timed dispenser program will dispense a predefined set of items into a cargo container on a specified
interval. The interval can be changed, as can the items to be dispensed, their maximum amounts, and the
rate of dispense.

=== Instruction Set Template ===

ProgramName
ProgramInterval (This must be either greatest common denominator or least common denominator of item dispense intervals)
ProgramMode
DispenseCargoContainerName
Component1ClassType
Component1Type
Component1DispenseInterval
Component1AmountPerDispense
Component1MaxInventoryAmount
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

=== Sample set ===
istr0:BasicTimedDispenser*
istr1:5*
istr2:topoff*
istr3:referencecargo01*
istr4:Component*
istr5:SteelPlate*
istr6:5*
istr7:25*
istr8:120*
istr9:PhysicalObject*
istr10:ZoneChip*
istr11:2*
istr12:100*
istr13:180*
istr14:MyObjectBuilder_Type
istr15:platinum_ingot*
istr16:10*
istr17:1000*
istr18:240*