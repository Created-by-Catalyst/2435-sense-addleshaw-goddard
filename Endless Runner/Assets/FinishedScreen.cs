using UnityEngine;

public class FinishedScreen : MonoBehaviour
{
    [SerializeField] WinState winState;

    [SerializeField] GameOverState gameoverState;

    [SerializeField] Leaderboard leaderboard;

    public void RunAgain()
    {
        if (gameoverState.gameObject.activeSelf == true)
        {
            gameoverState.RunAgain();
        }
        else if (winState.gameObject.activeSelf == true)
        {
            SaveNewScoreToFirebase();
            winState.RunAgain();
        }
    }

    public void GoToLoadout()
    {
        if (gameoverState.gameObject.activeSelf == true)
        {
            gameoverState.GoToLoadout();
        }
        else if (winState.gameObject.activeSelf == true)
        {
            SaveNewScoreToFirebase();
            winState.GoToLoadout();
        }
    }



    void SaveNewScoreToFirebase()
    {
        FirebaseHandler.Instance.UploadScore(leaderboard.playerEntry.playerName.text, leaderboard.playerEntry.finalScore);
    }

    public void OpenLeaderboard()
    {
        //if (gameoverState.gameObject.activeSelf == true)
        //{
        gameoverState.OpenLeaderboard();
        //}
        //else if (winState.gameObject.activeSelf == true)
        //{
        //    winState.OpenLeaderboard();
        //}
        //else
        //{
        //    gameoverState.OpenLeaderboard();
        //}
    }

    public void CloseLeaderboard()
    {

        gameoverState.CloseLeaderboard();

    }

}
