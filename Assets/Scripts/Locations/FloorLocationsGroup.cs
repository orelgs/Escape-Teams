using System.Collections.Generic;
using UnityEngine;

public class FloorLocationsGroup : PossibleLocationsGroup
{
    public FloorLocationsGroup()
    {
        possibleObjectLocations.Add(new PossibleObjectLocation(new Vector3(-6.058f, 0.015f, -7.119f), new Quaternion(0f, 0f, 0f, 0f)));
        possibleObjectLocations.Add(new PossibleObjectLocation(new Vector3(10.342f, -0.002f, -21.95f), new Quaternion(0f, 0f, 0f, 0f)));
        possibleObjectLocations.Add(new PossibleObjectLocation(new Vector3(10.4081f, -0.00274f, -0.399f), new Quaternion(0f, 180f, 0f, 0f)));
    }

    public override KeyValuePair<GameObject, PossibleObjectLocation> getRandomObjectAndLocation()
    {
        PossibleObjectLocation objectLocation = getLocation();
        GameObject objectToSpawn = getObject();

        return new KeyValuePair<GameObject, PossibleObjectLocation>(objectToSpawn, objectLocation);
    }
}
