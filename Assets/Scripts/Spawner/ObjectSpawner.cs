using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ObjectSpawner : MonoBehaviourPunCallbacks
{
    private List<IPossibleLocationsGroup> possibleLocationsGroups = new List<IPossibleLocationsGroup>();
    private bool isRunning = true;
    private Dictionary<string, InteractableObjectServerData> currentlySpawnedObjects = new Dictionary<string, InteractableObjectServerData>();
    private List<PossibleObjectLocation> currentlyUsedLocations = new List<PossibleObjectLocation>();
    private PossibleObjectLocation mostRecentlyDestroyedObjectLocation = null;
    private int currentObjectsToSpawnCount;

    private void Awake()
    {
        possibleLocationsGroups.Add(GetComponent<TableLocationsGroup>());
        possibleLocationsGroups.Add(GetComponent<FloorLocationsGroup>());
        possibleLocationsGroups.Add(GetComponent<WallLocationsGroup>());

        currentObjectsToSpawnCount = PhotonNetwork.CurrentRoom.PlayerCount >= 8 ? PhotonNetwork.CurrentRoom.PlayerCount / 2 + 1 : 5;
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (!isRunning) return;
        if (currentlySpawnedObjects.Count >= currentObjectsToSpawnCount) return;

        KeyValuePair<GameObject, PossibleObjectLocation> objectAndLocation;

        do
        {
            objectAndLocation = getObjectAndLocationToSpawn();
        } while (currentlySpawnedObjects.ContainsKey(objectAndLocation.Key.name + "(Clone)") || currentlyUsedLocations.Contains(objectAndLocation.Value) || (mostRecentlyDestroyedObjectLocation is not null && objectAndLocation.Value == mostRecentlyDestroyedObjectLocation));

        GameObject currentlySpawnedObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", objectAndLocation.Key.name), objectAndLocation.Value.Position, Quaternion.identity);

        RotateSpawnedObject(currentlySpawnedObject, objectAndLocation.Value.Rotation);

        currentlySpawnedObjects.Add(currentlySpawnedObject.name, new InteractableObjectServerData(currentlySpawnedObject, objectAndLocation.Value));
        currentlyUsedLocations.Add(objectAndLocation.Value);
    }

    private KeyValuePair<GameObject, PossibleObjectLocation> getObjectAndLocationToSpawn()
    {
        System.Random random = new System.Random();
        IPossibleLocationsGroup locationsGroup = possibleLocationsGroups[random.Next(possibleLocationsGroups.Count)];

        return locationsGroup.getRandomObjectAndLocation();
    }

    private void RotateSpawnedObject(GameObject gameObjectToRotate, Quaternion rotation)
    {
        float xRotation = rotation.x;
        float yRotation = rotation.y;
        float zRotation = rotation.z;
        Vector3 rot = gameObject.transform.localRotation.eulerAngles;

        rot.Set(xRotation, yRotation, zRotation);
        gameObjectToRotate.transform.localRotation = Quaternion.Euler(rot);
    }

    private void CheckForObjectInteractionFinished(string objectName)
    {
        if (currentlySpawnedObjects[objectName].countOfClientsFinished != PhotonNetwork.CurrentRoom.PlayerCount) return;

        PhotonNetwork.Destroy(currentlySpawnedObjects[objectName].GameObj);
        mostRecentlyDestroyedObjectLocation = currentlySpawnedObjects[objectName].SpawnLocation;
        currentlyUsedLocations.Remove(currentlySpawnedObjects[objectName].SpawnLocation);
        currentlySpawnedObjects.Remove(objectName);
    }

    public void HandleObjectInteractionEnded(Component sender, object data)
    {
        if (data is InteractionStatus interactionStatus)
        {
            if (interactionStatus == InteractionStatus.Team1Finished || interactionStatus == InteractionStatus.Team2Finished)
            {
                SyncInteractionEndedOverPhoton(sender.gameObject.name);
            }
        }
    }

    private void SyncInteractionEndedOverPhoton(string endedInteractionObjectName)
    {
        Hashtable hash = new Hashtable
        {
            {"ObjectInteractionFinished", endedInteractionObjectName }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (!changedProps.ContainsKey("ObjectInteractionFinished")) return;

        currentlySpawnedObjects[(string)changedProps["ObjectInteractionFinished"]].countOfClientsFinished++;

        CheckForObjectInteractionFinished((string)changedProps["ObjectInteractionFinished"]);
    }
}
