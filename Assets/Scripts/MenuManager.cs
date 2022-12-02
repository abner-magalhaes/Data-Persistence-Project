using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    public TMP_InputField InputNameField;
    public TextMeshProUGUI ScoreText;

    private void Start()
    {
        InputNameField.text = PersistentData.Instance.PlayerName;
        GetPlayerScore();
    }

    public void StartNewGame()
    {
        if(InputNameField.text != "")
            SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {

        PersistentData.Instance.SaveScores();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif

    }

    public void GetPlayerScore()
    {
        string playerName = InputNameField.text;
        PersistentData.Instance.LoadPlayerScore(playerName);

        Debug.Log("Player name: " + playerName + " length: " + playerName.Length + " Is Empty: " + string.IsNullOrEmpty(playerName));

        if (playerName.Length != 0)
            ScoreText.text = "Best Score of " + PersistentData.Instance.PlayerName + ": " + PersistentData.Instance.PlayerBestScore.ToString();
        else
            ScoreText.text = "Best Score";
    }
}
