using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Unity.VisualScripting;
using Firebase;
using UnityEngine.SocialPlatforms.Impl;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Threading.Tasks;

public class FirebaseHandler : MonoBehaviour
{
    FirebaseFirestore db;
    public static FirebaseHandler Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => { 
            //FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            FirebaseApp app = FirebaseApp.DefaultInstance;
            db = FirebaseFirestore.DefaultInstance;
        });
        print(FirebaseApp.CheckDependenciesAsync().Result);
    }


    [ContextMenu("TESTSEND")]
    public void TestSend()
    {
        UploadScore("billy", 50);

        //SendUserDataToDatabase(new UserData
        //{
        //    name = "bill",
        //    score = 5
        //});
    }


    public void UploadScore(string username, int score)
    {
        if (db == null)
        {
            Debug.LogError("Firebase not initialized");
            return;
        }

        // Create a new dictionary to represent the data
        Dictionary<string, object> userScore = new Dictionary<string, object>
        {
            { "username", username },
            { "score", score }
        };

        //userScore.Add("billy", 50);

        // Add a new document with a generated ID
        db.Collection("leaderboard").AddAsync(userScore).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Score uploaded successfully!");
            }
            else
            {
                Debug.LogError("Failed to upload score: " + task.Exception);
            }
        });
    }


    public List<Entry> backupLeaderboardEntries;

    [ContextMenu("TESTREAD")]
    public async Task<List<Entry>> ReadScores()
    {
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            //FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            FirebaseApp app = FirebaseApp.DefaultInstance;
            db = FirebaseFirestore.DefaultInstance;
        });


        Debug.Log("start");
        if (db == null)
        {
            Debug.LogError("Firebase not initialized");
            return backupLeaderboardEntries;
        }

        // Reference to the "scores" collection
        CollectionReference scoresRef = db.Collection("leaderboard");

        List<Entry> leaderboardEntries = new List<Entry>();

        // Get all documents in the "scores" collection
        await scoresRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {

                QuerySnapshot snapshot = task.Result;
                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    Debug.Log("read");
                    // Retrieve data from the document
                    Dictionary<string, object> leaderboardData = document.ToDictionary();
                    string username = leaderboardData["username"].ToString();
                    int score = int.Parse(leaderboardData["score"].ToString());
                    //Debug.Log($"Username: {username}, Score: {score}");

                    leaderboardEntries.Add(new Entry(username, score));
                }
            }
            else
            {
                Debug.LogError("Failed to read scores: " + task.Exception);
            }
        });

        backupLeaderboardEntries = leaderboardEntries;

        return leaderboardEntries;
    }


    

}
public class Entry
{
    public Entry(string newName, int newScore)
    {
        name = newName;
        score = newScore;
    }

    public string name;
    public int score;
}