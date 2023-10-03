using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text playerNameText;
    private Image backgroundImage;
    private Color localPlayerBackgroundColor;
    private Player player;

    private void Awake()
    {
        localPlayerBackgroundColor = Color.yellow;
        localPlayerBackgroundColor.a = 0.6f;

        backgroundImage = GetComponent<Image>();
    }

    public void SetUp(Player player)
    {
        this.player = player;
        playerNameText.text = player.NickName;
        SetPlayerTextIfMaster();
        ChangeImageColorIfLocal();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
        else
        {
            SetPlayerTextIfMaster();
            ChangeImageColorIfLocal();
        }
    }

    private void SetPlayerTextIfMaster()
    {
        if (PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.masterClientId).NickName == player.NickName)
        {
            playerNameText.text = player.NickName + " (Master)";
        }
    }

    private void ChangeImageColorIfLocal()
    {
        if (PhotonNetwork.LocalPlayer.NickName == player.NickName)
        {
            backgroundImage.color = localPlayerBackgroundColor;
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
