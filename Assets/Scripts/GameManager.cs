using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //Gameplay
    private int _score;
    private int _level;
    private float _time;
    private string _playerName = "Anonymous";
    public int playerHealth;
    public Ranking ranking;
    
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
            DontDestroyOnLoad(gameObject);
        } 
    }


    private void Start()
    {   
        AudioManager.Instance.Init();
        LoadRanking();
        MainMenu();
        GameEvents.OnPointsChangeEvent += OnPointsChange;
        GameEvents.OnPlayerNameChangeEvent += OnPlayerNameChange;
    }
    

    private void OnDestroy()
    {
        GameEvents.OnPointsChangeEvent -= OnPointsChange;
        GameEvents.OnPlayerNameChangeEvent += OnPlayerNameChange;
        SaveRanking();
    }

    public void StartGameplay()
    {      
        int maxScore = PlayerPrefs.GetInt(_playerName, -1);

        if (maxScore == -1) {
            PlayerPrefs.SetInt(_playerName, 0);
        }

        playerHealth = 3;
        _score = 0;
        _level = 2;
        LoadLevel(2);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        Debug.Log("Loading Menu...");        
        AudioManager.Instance.PlayMusic(AudioMusicType.Menu);
        SceneManager.LoadScene("Menu");
    }

    public void OnPlayerNameChange(string playerName)
    {   
        if (playerName != "") {
            _playerName = playerName;
        }
        
        StartGameplay();
    }
    
    public void LoadLevel(int levelNum)
    {
        Debug.Log("Loading Level " + levelNum.ToString() + "...");

        _level = levelNum;
        AudioMusicType music = AudioMusicType.Level_1;

        switch (levelNum) {

            case 2:
                music = AudioMusicType.Level_2;
                break;

            case 3:
                music = AudioMusicType.Level_3;
                break;

            default:
                break;

        }
        
        string levelName = "Level" + levelNum.ToString();
        AudioManager.Instance.PlayMusic(music);
        StartCoroutine(LoadLevelAsyncScene(levelName));
    }

    IEnumerator LoadLevelAsyncScene(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        _time = Time.time;
        yield return new WaitForSeconds(0.1f);
        int maxScore = PlayerPrefs.GetInt(_playerName, 0);
        GameEvents.OnPlayerHealthChangeEvent?.Invoke(playerHealth);
        GameEvents.OnPlayerScoreChangeEvent?.Invoke(_score);
        GameEvents.OnLoadHighScoreEvent?.Invoke(maxScore);
    }

    public void GameOver()
    {
        Debug.Log("Show 'Game Over' Screen");
        int maxScore = SaveScore();
        _time = Time.time - _time;
        GameEvents.OnGameOverEvent?.Invoke(_score, _score > maxScore, _time, _level);
        AudioManager.Instance.PlayMusic(AudioMusicType.Death);
    }

    public void NextLevel()
    {
        Debug.Log("Show 'Victory' Screen");
        int maxScore = SaveScore();
        _time = Time.time - _time;
        GameEvents.OnNextLevelEvent?.Invoke(_score, _score > maxScore, _time, _level);
        AudioManager.Instance.PlayMusic(AudioMusicType.Victory);

        _level += 1;
    }

    private int SaveScore() {

        int maxScore = PlayerPrefs.GetInt(_playerName, 0);
        
        if (_score > maxScore)
        {
            PlayerPrefs.SetInt(_playerName, _score);
        }

        int playerRankingID = 0;
        int playersCount = ranking.players.Count;

        foreach (RankPlayer player in ranking.players)
        {
            if (_score > player.maxScore)
            {   
                string actualPlayerName;
                int actualPlayerMaxScore;
                string previousPlayerName = ranking.players[playerRankingID].name;
                int previousPlayerMaxScore = ranking.players[playerRankingID].maxScore;

                ranking.players[playerRankingID].name = _playerName;
                ranking.players[playerRankingID].maxScore = _score;

                for (int i = playerRankingID+1; i < playersCount; i++) {
                    actualPlayerName = previousPlayerName;
                    actualPlayerMaxScore = previousPlayerMaxScore;
                    previousPlayerName = ranking.players[i].name;
                    previousPlayerMaxScore = ranking.players[i].maxScore;

                    ranking.players[i].name = actualPlayerName;
                    ranking.players[i].maxScore = actualPlayerMaxScore;
                }

                break;
            }

            playerRankingID++;
        }

        return maxScore;
    }

    private void OnPointsChange(int points)
    {   
        if (_score + points > 9999) {
            _score = 9999;
        } else {
            _score += points;
        }
        
        GameEvents.OnPlayerScoreChangeEvent?.Invoke(_score);
    }

    [ContextMenu("Save ranking")]
    public void SaveRanking()
    {
        string rankData = JsonUtility.ToJson(ranking, true);
        Debug.Log("Save ranking: " + rankData);
        PlayerPrefs.SetString("Ranking", rankData);
    }

    [ContextMenu("Load ranking")]
    public void LoadRanking()
    {   
        string rankData = PlayerPrefs.GetString("Ranking", "null");

        if (rankData != "null") {
            ranking = JsonUtility.FromJson<Ranking>(rankData);
        } else {
            ranking = new Ranking {players = new List<RankPlayer>()};
            SaveRanking();
        }

        if (ranking.players.Count != 5) {

            for (int i = 0; i < 5; i++) {
                string playerName = "Anonymous";
                int playerMaxScore = 0;
                RankPlayer player = new(playerName, playerMaxScore);
                ranking.players.Add(player);
            }
        }
    }
 
}

[Serializable]
public class Ranking
{
    public List<RankPlayer> players;
}

[Serializable]
public class RankPlayer
{   
    public RankPlayer(string _name, int _maxScore)
    {
        name = _name;
        maxScore = _maxScore;
    }

    public string name;
    public int maxScore;
}