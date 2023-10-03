using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text buttonText;

    public RoomInfo roomInfo;

    public void SetUp(RoomInfo roomInfo)
    {
        this.roomInfo = roomInfo;
        buttonText.text = roomInfo.Name;
    }

    public void OnClick()
    {
        PhotonLauncher.Instance.JoinRoom(roomInfo);
    }
}
