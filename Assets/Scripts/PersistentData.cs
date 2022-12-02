using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentData : MonoBehaviour
{
    public static PersistentData Instance;

    public string PlayerName = string.Empty;
    public int PlayerBestScore = 0;

    private PlayerList PlayerScores;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        LoadScores();
        DontDestroyOnLoad(gameObject);
    }

    [System.Serializable]
    class PlayerScore
    {
        public string PlayerName;
        public int Score;
    }

    [System.Serializable]
    class PlayerList
    {
        public List<PlayerScore> PlayerScores = new List<PlayerScore>();
    }

    public void SaveScores()
    {
        var playerList = PlayerScores;
        string json = JsonUtility.ToJson(playerList);

        File.WriteAllText(Application.persistentDataPath + "/playerScoreList.json", json);
    }

    public void LoadScores()
    {
        string path = Application.persistentDataPath + "/playerScoreList.json";

        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerList playerList = JsonUtility.FromJson<PlayerList>(json);
            PlayerScores = playerList;
        }
        else
        {
            PlayerScores = new PlayerList();
        }
    }

    public void SavePlayerScore()
    {
        var playerExists = PlayerScores.PlayerScores.Exists(p => p.PlayerName == PlayerName);

        Debug.Log("Player " + PlayerName + " exists? " + playerExists);

        if (!playerExists)
        {
            var playerScore = new PlayerScore();
            playerScore.PlayerName = PlayerName;
            playerScore.Score = PlayerBestScore;

            PlayerScores.PlayerScores.Add(playerScore);
        }
        else
        {
            PlayerScore ps = PlayerScores.PlayerScores.First(p => p.PlayerName == PlayerName);
            ps.Score = PlayerBestScore;

            Debug.Log("JSON " + JsonUtility.ToJson(ps));

            ps = PlayerScores.PlayerScores.First(p => p.PlayerName == PlayerName) as PlayerScore;

            Debug.Log("JSON 2: " + JsonUtility.ToJson(ps));
        }
    }

    public void LoadPlayerScore(string playerName)
    {
        bool playerExists = PlayerScores != null ? PlayerScores.PlayerScores.Exists(p => p.PlayerName == playerName) : false;

        PlayerName = playerName;

        if (playerExists)
        {
            PlayerScore ps = PlayerScores.PlayerScores.First(p => p.PlayerName == playerName);
            PlayerBestScore = ps.Score;
        }
        else
        {
            PlayerBestScore = 0;
        }
    }
}
