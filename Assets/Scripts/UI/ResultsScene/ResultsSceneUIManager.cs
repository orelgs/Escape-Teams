using UnityEngine;

public class ResultsSceneUIManager : MonoBehaviour
{
    [SerializeField] private GameObject team1Win;
    [SerializeField] private GameObject team1WinAnnouncement;
    [SerializeField] private GameObject team2Win;
    [SerializeField] private GameObject team2WinAnnouncement;
    [SerializeField] private GameEvent onLeaveGameButtonClicked;

    public void OnLeaveGameButtonClicked()
    {
        onLeaveGameButtonClicked.Raise(this, null);
    }

    public void OnTeamWinAnnounced(Component sender, object data)
    {
        if (data is PlayerTeam winningTeam)
        {
            if (winningTeam == PlayerTeam.Team1)
            {
                team1Win.SetActive(true);
                team1WinAnnouncement.SetActive(true);
            }
            else if (winningTeam == PlayerTeam.Team2)
            {
                team2Win.SetActive(true);
                team2WinAnnouncement.SetActive(true);
            }
        }
    }
}
