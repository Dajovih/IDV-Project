using System;
using UnityEngine;

public static class GameEvents
{
    public static Action<int> OnStartGameEvent; // current max score

    public static Action onNewPlatformEvent;

    public static Action onRankingScreenEvent;

    public static Action onMainMenuEvent;
    
    public static Action<int, bool, float, int> OnGameOverEvent; //total score, is max score?, time, level

    public static Action<int, bool, float, int> OnNextLevelEvent; //total score, is max score?, time, level

    public static Action<int> OnPlayerScoreChangeEvent; //current score

    public static Action<int> OnPlayerHealthChangeEvent; // current health

    public static Action<int> OnPointsChangeEvent; //points
}