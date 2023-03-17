using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VRage;
using Sandbox.Game;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.Library.Collections;
using VRage.Serialization;
using System.Runtime.CompilerServices;

namespace PEWCore
{
    public class PEWCoreClient
    {
        //Reminder -------------------------------------------------------------new hash, name,           vector3D,       
        public static void ClientUpdateWorldGPSWaypoints(Dictionary<int, MyTuple<int, string, VRageMath.Vector3D, VRageMath.Color>> latestGPSDictionary)
        {
            /*
            long myid = (long)MyAPIGateway.Multiplayer.MyId;
            //List<IMyGps> myGpsCollection = MyAPIGateway.Session.GPS.Crea
            //MyAPIGateway.Session.GPS.

            if (latestGPSDictionary.Count > 0)
            {
                int[] latestGPSDictionaryKeys = latestGPSDictionary.Keys.ToArray();
                if (myGpsCollection.Count > 0)
                {
                    for (int z = 0; z < latestGPSDictionary.Count; z++)
                    {
                        //IMyGps currentGPSMarker = myGpsCollection.ElementAt(myGpsCollection.Count - 1);
                        MyTuple<int, string, VRageMath.Vector3D, VRageMath.Color> currentLatestGPSDictionaryTuple = latestGPSDictionary.GetValueOrDefault(latestGPSDictionaryKeys[z]);
                        int oldHash = latestGPSDictionaryKeys[z];
                        int newHash = currentLatestGPSDictionaryTuple.Item1;
                        if ((isIMyGpsHashInIMyGpsList(oldHash, myGpsCollection) == -1) && (isIMyGpsHashInIMyGpsList(newHash, myGpsCollection) == -1))
                        {
                            IMyGps tempGPSObject = null;
                            tempGPSObject = MyAPIGateway.Session.GPS.Create(currentLatestGPSDictionaryTuple.Item2, "Automatically created and updated by PEWCore", currentLatestGPSDictionaryTuple.Item3, true);
                            tempGPSObject.GPSColor = currentLatestGPSDictionaryTuple.Item4;
                            MyAPIGateway.Session.GPS.AddLocalGps(tempGPSObject);
                        }
                        else
                        {
                            if ((isIMyGpsHashInIMyGpsList(oldHash, myGpsCollection) == -1) && (isIMyGpsHashInIMyGpsList(newHash, myGpsCollection) != -1))
                            {
                            }
                            else
                            {
                                if ((isIMyGpsHashInIMyGpsList(oldHash, myGpsCollection) != -1) && (isIMyGpsHashInIMyGpsList(newHash, myGpsCollection) == -1))
                                {
                                    int indexOfWaypointToUpdate = isIMyGpsHashInIMyGpsList(oldHash, myGpsCollection);
                                    IMyGps wayPointToUpdate = myGpsCollection[indexOfWaypointToUpdate];
                                    wayPointToUpdate.Name = currentLatestGPSDictionaryTuple.Item2;
                                    wayPointToUpdate.Coords = currentLatestGPSDictionaryTuple.Item3;
                                    wayPointToUpdate.GPSColor = currentLatestGPSDictionaryTuple.Item4;

                                }
                                else
                                {
                                    if ((isIMyGpsHashInIMyGpsList(oldHash, myGpsCollection) != -1) && (isIMyGpsHashInIMyGpsList(newHash, myGpsCollection) != -1))
                                    {
                                        int indexOfWaypointToRemove = isIMyGpsHashInIMyGpsList(oldHash, myGpsCollection);
                                        IMyGps wayPointToRemote= myGpsCollection[indexOfWaypointToRemove];


                                    }
                                }
                            }
                        }
                    }

                    //Now we clean up system gps waypoints from player GPS list that 
                }
                else
                {
                    for (int u = 0; u < latestGPSDictionary.Count; u++)
                    {
                        IMyGps tempGPSObject = null;
                        tempGPSObject = MyAPIGateway.Session.GPS.Create(latestGPSDictionary.GetValueOrDefault(latestGPSDictionaryKeys[u]).Item2, "Automatically created and updated by PEWCore", latestGPSDictionary.GetValueOrDefault(latestGPSDictionaryKeys[u]).Item3, true);
                        tempGPSObject.GPSColor = latestGPSDictionary.GetValueOrDefault(latestGPSDictionaryKeys[u]).Item4;
                        MyAPIGateway.Session.GPS.AddLocalGps(tempGPSObject);
                    }
                }
            }
        }
        public static int isIMyGpsHashInIMyGpsList(int hash, List<IMyGps> myGpsList)
        {
            int result = -1;
            for (int i = 0; i < myGpsList.Count; i++)
            {
                if (hash == myGpsList[i].Hash)
                    result = i;
            }
            return result;

            */
        }
    }
}
