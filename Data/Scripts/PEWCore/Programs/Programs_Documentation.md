# PEW_Core - 8616941723
 
## Phobos Engineered Weaponry - MP Core
## Programs Documentation

This document contains explanatory information regarding all generic programs implemented within PEW core
as well as sample instruction sets and listing of valid arguments, classname, commands, etc.

## Warning

Double check your instruction sets before entering them into logical cores. Theoretically, it should be 
impossible to cause a server crash via the instruction set of a logical core, but lets not try to poke 
the bear by trying experimental instruction sets on the production/public server :)

## Expansion/Adding capability

If you need something added to any of the programs, just let me know and I'll consider it.

## [01] - Basic Timed Dispenser

This program allows a user to dispense up to three different types of item classes into one or more cargo
containers with a specific name at defined intervals.

The dispensable item MyObjectBuilder types and supported items are:

1). Component
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
	
2). PhysicalObject
	- SpaceCredit
	
3). ConsumableItem
	- Medkit
	- Powerkit
	- ClangCola
	- CosmicCoffee
	
4). Datapad
	- Datapad
	
5). Ore
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
	
6). Ingot
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

## Contact

Discord: FinancedDart#1863

Project Link: https://github.com/FinancedDart1863/PEW_Core

Steam Workshop: [Not yet available]

## Copyright

© 2023 FinancedDart <br />
© 2023 Phobos Engineered Weaponry Group <br />
© 2023 Sadragos (Affiliate - Tiered Tech) <br />
© 2023 spiritplumber (Affiliate - Active Radar) <br />

You are not permitted to use code from this repository without my express permission. Unauthorized re-use, re-upload, or duplication of this code and/or SE mod will
result in a DMCA and legal action. Contact me for permission requests and any questions. Any derivative mods for which permission has been granted must be unlisted.