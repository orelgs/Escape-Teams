
using UnityEngine;

public class InteractableObjectServerData
{
    public GameObject GameObj { get; }
    public PossibleObjectLocation SpawnLocation { get; }
    public int countOfClientsFinished { get; set; } = 0;

    public InteractableObjectServerData(GameObject gameObj, PossibleObjectLocation spawnLocation)
    {
        GameObj = gameObj;
        SpawnLocation = spawnLocation;
    }
}