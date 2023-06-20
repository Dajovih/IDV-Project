using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //Gameplay
    private int _score = 0;
    private int _level = 1;
    private float _time = 0;
    
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
        MainMenu();

        GameEvents.OnPointsChangeEvent += OnPointsChange;
    }
    

    private void OnDestroy()
    {
        GameEvents.OnPointsChangeEvent -= OnPointsChange;
    }

    public void StartGame()
    {
        HandleLevel(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void MainMenu()
    {
        HandleMenu();
    }

    public void Level(int levelNum)
    {
        HandleLevel(levelNum);
    }

    public void ScoreMenu()
    {
        AudioManager.Instance.PlayMusic(AudioMusicType.Score);
    }

    public void GameOver()
    {
        Debug.Log("Game over");
        _time = Time.time - _time;
        int maxScore = PlayerPrefs.GetInt("MaxScore", 0);
        if (_score > maxScore)
        {
            PlayerPrefs.SetInt("MaxScore", _score);
        }
        
        GameEvents.OnGameOverEvent?.Invoke(_score, _score > maxScore, _time, _level);
        AudioManager.Instance.PlayMusic(AudioMusicType.Death);
    }

    public void NextLevel()
    {
        Debug.Log("Next Level");
        _time = Time.time - _time;
        int maxScore = PlayerPrefs.GetInt("MaxScore", 0);
        
        if (_score > maxScore)
        {
            PlayerPrefs.SetInt("MaxScore", _score);
        }
        
        GameEvents.OnNextLevelEvent?.Invoke(_score, _score > maxScore, _time, _level);
        AudioManager.Instance.PlayMusic(AudioMusicType.Victory);

        _level += 1;
    }

    void HandleMenu()
    {
        Debug.Log("Loading Menu...");

        _score = 0;
        
        AudioManager.Instance.PlayMusic(AudioMusicType.Menu);
        SceneManager.LoadScene("Menu");
    }
    
    void HandleLevel(int levelNum)
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
        SceneManager.LoadScene(levelName);

        _time = Time.time;
        GameEvents.OnStartGameEvent?.Invoke();
    }

    private void OnPointsChange(int points)
    {   
        if (_score + points < 0) {
            _score = 0;
        } else {
            _score += points;
        }
        
        GameEvents.OnPlayerScoreChangeEvent?.Invoke(_score);
    }
/*
    public Rank rank;

    [ContextMenu("Save rank")]
    public void SaveRank()
    {
        string rankData = JsonUtility.ToJson(rank, true);
        Debug.Log(rankData);
        PlayerPrefs.SetString("Rank", rankData);
    }

    [ContextMenu("Load rank")]
    public void LoadRank()
    {
        string rankData = PlayerPrefs.GetString("Rank");
        rank = JsonUtility.FromJson<Rank>(rankData);
    }
 */
}
/* 
[Serializable]
public class Rank
{
    public List<RankUser> users;
}

[Serializable]
public class RankUser
{
    public int score;
    public string name;
}
 */