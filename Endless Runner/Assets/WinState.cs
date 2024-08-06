using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_ANALYTICS
using UnityEngine.Analytics;
#endif

/// <summary>
/// state pushed on top of the GameManager when the player dies.
/// </summary>
public class WinState : AState
{
    public KeyboardManager onScreenKeyboard;

    public GameObject defaultLoadoutButton;

    public GameObject entryFinishedLoadoutButton;

    public GameOverState gameOverState;

    public TrackManager trackManager;
    public Canvas canvas;

    public AudioClip winTheme;

    public Leaderboard miniLeaderboard;
    public Leaderboard fullLeaderboard;

    public GameObject addButton;

    [SerializeField] InputField playerEntry;
    public void AddCharacter(string key)
    {
        if (key == "Enter")
        {
            onScreenKeyboard.gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(entryFinishedLoadoutButton);
        }
        else if (key == "Back")
        {
            if (playerEntry.text.Length > 0)
            {
                playerEntry.text = playerEntry.text.Substring(0, playerEntry.text.Length - 1);
            }
        }
        else
        {
            playerEntry.text += key;
        }
    }


    public override void Enter(AState from)
    {
        //GameManager.instance.SetSelectedUIElement(defaultLoadoutButton);

        onScreenKeyboard.gameObject.SetActive(true);

        gameOverState.gameObject.SetActive(false);
        gameObject.SetActive(true);

        canvas.gameObject.SetActive(true);

        miniLeaderboard.playerEntry.inputName.text = PlayerData.instance.previousName;

        miniLeaderboard.playerEntry.finalScore = 3000 - ( trackManager.finishTime.Seconds * 3);
        miniLeaderboard.playerEntry.finishTimeText.text = miniLeaderboard.playerEntry.finalScore.ToString();
        miniLeaderboard.Populate();

        CreditCoins();

        if (MusicPlayer.instance.GetStem(0) != winTheme)
        {
            MusicPlayer.instance.SetStem(0, winTheme);
            StartCoroutine(MusicPlayer.instance.RestartAllStems());
        }

        OpenLeaderboard();

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultLoadoutButton);


        playerEntry.text = "";
    }

    public override void Exit(AState to)
    {
        onScreenKeyboard.gameObject.SetActive(false);
        canvas.gameObject.SetActive(false);
        FinishRun();
    }

    public override string GetName()
    {
        return "Win";
    }

    public override void Tick()
    {

    }

    public void OpenLeaderboard()
    {
        fullLeaderboard.forcePlayerDisplay = false;
        fullLeaderboard.displayPlayer = true;
        fullLeaderboard.playerEntry.playerName.text = miniLeaderboard.playerEntry.inputName.text;
        fullLeaderboard.playerEntry.finalScore = 3000 - (trackManager.finishTime.Seconds * 3);
        fullLeaderboard.playerEntry.finishTimeText.text = fullLeaderboard.playerEntry.finalScore.ToString();

        fullLeaderboard.Open();
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
        if (miniLeaderboard.playerEntry.inputName.text == "")
        {
            miniLeaderboard.playerEntry.inputName.text = "";
        }
        else
        {
            PlayerData.instance.previousName = miniLeaderboard.playerEntry.inputName.text;
        }

        PlayerData.instance.InsertScore(3000 - trackManager.finishTime.Seconds, miniLeaderboard.playerEntry.inputName.text);

        CharacterCollider.DeathEvent de = trackManager.characterController.characterCollider.deathData;
        //register data to analytics
#if UNITY_ANALYTICS
        AnalyticsEvent.GameOver(null, new Dictionary<string, object> {
            { "coins", de.coins },
            { "premium", de.premium },
            { "score", de.score },
            { "distance", de.worldDistance },
            { "obstacle",  de.obstacleType },
            { "theme", de.themeUsed },
            { "character", de.character },
        });
#endif

        PlayerData.instance.Save();

        trackManager.End();
    }

    //----------------
}
