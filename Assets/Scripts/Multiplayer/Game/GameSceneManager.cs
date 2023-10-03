using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ObjectSpawner"), Vector3.zero, Quaternion.identity);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "TaskManager"), Vector3.zero, Quaternion.identity);
    }

    public void OnAllClientsFinished(Component sender, object data)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        PhotonNetwork.LoadLevel((int)SceneIndex.ResultsScene);
    }
}
