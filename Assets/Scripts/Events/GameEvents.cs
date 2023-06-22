using System;
using UnityEngine;

public static class GameEvents
{
    public static Action<int> OnLoadHighScoreEvent; // current max score

    public static Action OnNewPlatformEvent;

    public static Action OnLevelWin;

    public static Action<float> OnEnemyAttack;

    public static Action OnMainMenuEvent;

    public static Action OnRankingScreenEvent;

    public static Action OnPlayerNameScreenEvent;
    
    public static Action<int, bool, float, int> OnGameOverEvent; //total score, is max score?, time, level

    public static Action<int, bool, float, int> OnNextLevelEvent; //total score, is max score?, time, level

    public static Action<int> OnPlayerScoreChangeEvent; //current score

    public static Action<int> OnPlayerHealthChangeEvent; // current health

    public static Action<string> OnPlayerNameChangeEvent; // player name

    public static Action<int> OnPointsChangeEvent; //points
}