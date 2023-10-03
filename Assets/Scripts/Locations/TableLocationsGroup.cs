using System.Collections.Generic;
using UnityEngine;


public class TableLocationsGroup : PossibleLocationsGroup
{
    public TableLocationsGroup()
    {
        possibleObjectLocations.Add(new PossibleObjectLocation(new Vector3(8.2591f, 1.1265f, -6.2528f), new Quaternion(-63.31f, 179.373f, 0.883f, 0f)));
    }

    public override KeyValuePair<GameObject, PossibleObjectLocation> getRandomObjectAndLocation()
    {
        PossibleObjectLocation objectLocation = getLocation();
        GameObject objectToSpawn = getObject();

        return new KeyValuePair<GameObject, PossibleObjectLocation>(objectToSpawn, objectLocation);
    }
}
