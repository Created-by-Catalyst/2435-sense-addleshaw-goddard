using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_ANALYTICS
using UnityEngine.Analytics;
#endif

/// <summary>
/// state pushed on top of the GameManager when the player dies.
/// </summary>
public class WinState : AState
{

    public TMP_Text goFasterText;
    public TMP_Text goFurtherText;

    public GameObject runInsights;

    public KeyboardManager onScreenKeyboard;

    public CursorHandler cursor;

    public GameObject defaultLoadoutButton;

    public GameObject entryFinishedLoadoutButton;

    public GameOverState gameOverState;

    public TrackManager trackManager;
    public Canvas canvas;

    public AudioClip winTheme;

    public Leaderboard miniLeaderboard;
    public Leaderboard fullLeaderboard;

    public GameObject addButton;

    [SerializeField] TMP_Text playerEntry;
    [SerializeField] TMP_Text playerEntryOnKeyboard;
    public void AddCharacter(string key)
    {
        if (key == "Enter")
        {
            cursor.gameObject.SetActive(false);
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

        playerEntryOnKeyboard.text = playerEntry.text;
    }

    int totalScore = 0;

    public override void Enter(AState from)
    {
        //GameManager.instance.SetSelectedUIElement(defaultLoadoutButton);

        onScreenKeyboard.gameObject.SetActive(true);
        cursor.gameObject.SetActive(true);

        gameOverState.gameObject.SetActive(false);
        gameObject.SetActive(true);

        canvas.gameObject.SetActive(true);

        miniLeaderboard.playerEntry.inputName.text = PlayerData.instance.previousName;


        print("finish time seconds " + trackManager.finishTime.TotalSeconds);
        print("end health " + trackManager.characterController.currentLife);

        //Total score + remaining health - time taken

        totalScore = (int)trackManager.finishTime.TotalSeconds;

        miniLeaderboard.playerEntry.finalScore = totalScore;


        TimeSpan finaltime = TimeSpan.FromSeconds(miniLeaderboard.playerEntry.finalScore);

        miniLeaderboard.playerEntry.finishTimeText.text = string.Format("{0:D2}:{1:D2}", finaltime.Minutes, finaltime.Seconds);

        miniLeaderboard.Populate();

        CreditCoins();

        //if (MusicPlayer.instance.GetStem(0) != winTheme)
        //{
        //    MusicPlayer.instance.SetStem(0, winTheme);
        //    StartCoroutine(MusicPlayer.instance.RestartAllStems());
        //}

        OpenLeaderboard();

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultLoadoutButton);


        playerEntry.text = "";
        playerEntryOnKeyboard.text = playerEntry.text;
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
        fullLeaderboard.playerEntry.finalScore = totalScore;

        TimeSpan finaltime = TimeSpan.FromSeconds(fullLeaderboard.playerEntry.finalScore);

        fullLeaderboard.playerEntry.finishTimeText.text = string.Format("{0:D2}:{1:D2}", finaltime.Minutes, finaltime.Seconds);

        fullLeaderboard.Open();
    }


    public void DelayedGoToLoadout()
    {
        float timeInFurther = trackManager.characterController.timeInLeftLane;
        float timeInFaster = trackManager.characterController.timeInRightLane;

        float total = timeInFaster + timeInFurther;

        float furtherPercent =  Mathf.Round((timeInFurther / total) * 100f);
        float fasterPercent = Mathf.Round((timeInFaster / total) * 100f);

        print($"{timeInFurther} \n {timeInFaster} \n {total} \n {furtherPercent} \n {fasterPercent}");


        goFurtherText.text = $"You chose  <b>{furtherPercent}%</b> GO FURTHER, \r\npowered by <b>GEL-NIMBUS 27</b>";

        goFasterText.text = $"You chose <b>{fasterPercent}%</b> GO FASTER, \r\npowered by <b>NOVABLAST 5</b>";




        runInsights.SetActive(true);
        Invoke("GoToLoadout", 10f);
    }


    public void GoToLoadout()
    {
        runInsights.SetActive(false);
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



        PlayerData.instance.InsertScore(miniLeaderboard.playerEntry.finalScore, miniLeaderboard.playerEntry.inputName.text);

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
