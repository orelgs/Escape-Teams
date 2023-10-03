using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private PlayerTeam playerTeam;
    public PlayerTeam PlayerTeam => playerTeam;
}