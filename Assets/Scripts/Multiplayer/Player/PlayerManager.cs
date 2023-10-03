using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Photon.Pun;
//using Unity.PlasticSCM.Editor.WebApi;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
//using UnityEditor.VersionControl;
using UnityEngine;



public class PlayerManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    private bool isInstantiate = false;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!PV.IsMine) { return; }
        if (isInstantiate) { return; }

        string localPlayer = PhotonNetwork.LocalPlayer.NickName;

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(localPlayer))
        {
            CreateController();
            isInstantiate = true;
        }
    }


    private void CreateController()
    {
        string localPlayer = PhotonNetwork.LocalPlayer.NickName;
        float x, y, z;

        string[] playerInfoArr = (string[])PhotonNetwork.CurrentRoom.CustomProperties[localPlayer];
        string playerTeam = playerInfoArr[0] == "team1" ? "Player1" : "Player2";

        x = float.Parse(playerInfoArr[1]);
        y = float.Parse(playerInfoArr[2]);
        z = float.Parse(playerInfoArr[3]);

        Vector3 playerPos = new Vector3(x, y, z);

        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", playerTeam), playerPos, Quaternion.identity);
    }
}
