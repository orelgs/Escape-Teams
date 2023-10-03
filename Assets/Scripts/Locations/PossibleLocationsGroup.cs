using System.Collections.Generic;
using UnityEngine;

public abstract class PossibleLocationsGroup : MonoBehaviour, IPossibleLocationsGroup
{
    [SerializeField] protected List<GameObject> relevantGameObjects;
    List<GameObject> IPossibleLocationsGroup.RelevantGameObjects => relevantGameObjects;

    protected PossibleObjectLocation lastPickedLocation = null;
    PossibleObjectLocation IPossibleLocationsGroup.LastPickedLocation => lastPickedLocation;

    protected GameObject lastPickedObject = null;
    GameObject IPossibleLocationsGroup.LastPickedObject => lastPickedObject;

    protected List<PossibleObjectLocation> possibleObjectLocations = new List<PossibleObjectLocation>();
    public List<PossibleObjectLocation> PossibleObjectLocations => possibleObjectLocations;

    abstract public KeyValuePair<GameObject, PossibleObjectLocation> getRandomObjectAndLocation();

    public PossibleObjectLocation getLocation()
    {
        System.Random random = new System.Random();
        PossibleObjectLocation locationToSpawn = possibleObjectLocations[random.Next(possibleObjectLocations.Count)];

        // if (lastPickedLocation is not null)
        // {
        //     while (locationToSpawn == lastPickedLocation)
        //     {
        //         locationToSpawn = possibleObjectLocations[random.Next(possibleObjectLocations.Count)];
        //     }
        // }

        // lastPickedLocation = locationToSpawn;

        return locationToSpawn;
    }

    public GameObject getObject()
    {
        System.Random random = new System.Random();
        GameObject objectToSpawn = relevantGameObjects[random.Next(relevantGameObjects.Count)];

        // if (lastPickedObject is not null)
        // {
        //     while (objectToSpawn == lastPickedObject)
        //     {
        //         objectToSpawn = relevantGameObjects[random.Next(relevantGameObjects.Count)];
        //     }
        // }

        // lastPickedObject = objectToSpawn;

        return objectToSpawn;
    }
}