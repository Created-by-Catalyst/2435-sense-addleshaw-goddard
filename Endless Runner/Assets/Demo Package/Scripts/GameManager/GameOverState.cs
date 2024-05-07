using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_ANALYTICS
using UnityEngine.Analytics;
#endif

/// <summary>
/// state pushed on top of the GameManager when the player dies.
/// </summary>
public class GameOverState : AState
{
    public GameObject defaultLoadoutButton;

    public WinState winState;

    public TrackManager trackManager;
    public Canvas canvas;

    public AudioClip gameOverTheme;
    public Leaderboard fullLeaderboard;

    public GameObject addButton;

    public override void Enter(AState from)
    {

        //GameManager.instance.SetSelectedUIElement(defaultLoadoutButton);

        winState.gameObject.SetActive(false);
        gameObject.SetActive(true);
        canvas.gameObject.SetActive(true);

        OpenLeaderboard();

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultLoadoutButton);
    }

    public override void Exit(AState to)
    {
        canvas.gameObject.SetActive(false);
        FinishRun();
    }

    public override string GetName()
    {
        return "GameOver";
    }

    public override void Tick()
    {

    }

    public void OpenLeaderboard()
    {
        fullLeaderboard.forcePlayerDisplay = false;
        fullLeaderboard.displayPlayer = false;
        //fullLeaderboard.playerEntry.finishTimeText.text = trackManager.finishTimeStr;
        //fullLeaderboard.playerEntry.finishTime = trackManager.finishTime;

        fullLeaderboard.Open();
    }
    public void CloseLeaderboard()
    {
        fullLeaderboard.Close();
    }

    public void GoToLoadout()
    {
        trackManager.isRerun = false;
        manager.SwitchState("Loadout");
    }

    public void RunAgain()
    {
        trackManager.isRerun = false;
        manager.SwitchState("Game");
        print("state run again");
    }

    protected void CreditCoins()
    {
        PlayerData.instance.Save();

#if UNITY_ANALYTICS // Using Analytics Standard Events v0.3.0
        var transactionId = System.Guid.NewGuid().ToString();
        var transactionContext = "gameplay";
        var level = PlayerData.instance.rank.ToString();
        var itemType = "consumable";
        
        if (trackManager.characterController.coins > 0)
        {
            AnalyticsEvent.ItemAcquired(
                AcquisitionType.Soft, // Currency type
                transactionContext,
                trackManager.characterController.coins,
                "fishbone",
                PlayerData.instance.coins,
                itemType,
                level,
                transactionId
            );
        }

        if (trackManager.characterController.premium > 0)
        {
            AnalyticsEvent.ItemAcquired(
                AcquisitionType.Premium, // Currency type
                transactionContext,
                trackManager.characterController.premium,
                "anchovies",
                PlayerData.instance.premium,
                itemType,
                level,
                transactionId
            );
        }
#endif
    }

    protected void FinishRun()
    {
        trackManager.End();
    }

    //----------------
}
