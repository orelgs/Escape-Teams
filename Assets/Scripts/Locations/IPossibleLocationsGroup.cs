using System.Collections.Generic;
using UnityEngine;

public interface IPossibleLocationsGroup
{
    [SerializeField] List<GameObject> RelevantGameObjects{ get; }
    PossibleObjectLocation LastPickedLocation { get; }
    GameObject LastPickedObject { get; }
    List<PossibleObjectLocation> PossibleObjectLocations { get; }

    KeyValuePair<GameObject, PossibleObjectLocation> getRandomObjectAndLocation();
    PossibleObjectLocation getLocation();
    GameObject getObject();
}
