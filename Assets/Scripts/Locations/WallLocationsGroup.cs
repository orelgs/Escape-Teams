using System.Collections.Generic;
using UnityEngine;


public class WallLocationsGroup : PossibleLocationsGroup
{
    public WallLocationsGroup()
    {
        possibleObjectLocations.Add(new PossibleObjectLocation(new Vector3(-2.318f, 0.62f, -29.216f), new Quaternion(0f, 0f, 0f, 0f)));
        possibleObjectLocations.Add(new PossibleObjectLocation(new Vector3(-1.684f, 0.957f, -13.602f), new Quaternion(0f, 270.207f, 0f, 0f)));
        possibleObjectLocations.Add(new PossibleObjectLocation(new Vector3(1.979f, 0.926f, -19.928f), new Quaternion(0f, -90f, 0f, 0f)));
        possibleObjectLocations.Add(new PossibleObjectLocation(new Vector3(9.863f, 0.875f, -19.404f), new Quaternion(0f, 0f, 0f, 0f)));
        possibleObjectLocations.Add(new PossibleObjectLocation(new Vector3(10.878f, 0.528f, -12.191f), new Quaternion(0f, -90f, 0f, 0f)));
        possibleObjectLocations.Add(new PossibleObjectLocation(new Vector3(0.669f, 1.48f, -24.025f), new Quaternion(0f, 180f, 0f, 0f)));
        possibleObjectLocations.Add(new PossibleObjectLocation(new Vector3(2.381f, 0.664f, -0.1903f), new Quaternion(0f, -90f, 0f, 0f)));
    }

    public override KeyValuePair<GameObject, PossibleObjectLocation> getRandomObjectAndLocation()
    {
        PossibleObjectLocation objectLocation = getLocation();
        GameObject objectToSpawn = getObject();

        return new KeyValuePair<GameObject, PossibleObjectLocation>(objectToSpawn, objectLocation);
    }
}
