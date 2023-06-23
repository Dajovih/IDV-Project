using TMPro;
using UnityEngine;
using System;

public class HUDPlayerScore : MonoBehaviour
{
    private TextMeshProUGUI _scoreText;
    
    void Start()
    {
        _scoreText = GetComponent<TextMeshProUGUI>();
        GameEvents.OnPlayerScoreChangeEvent += OnPlayerScoreChange;
    }

    private void OnDestroy()
    {
        GameEvents.OnPlayerScoreChangeEvent -= OnPlayerScoreChange;
    }

    
    private void OnPlayerScoreChange(int score)
    {   
        int scoreLength = score.ToString().Length;
        string zeros = new String('0', 4 - scoreLength);;
        _scoreText.text = $"SCORE: {zeros}{score}";
    }
    
}
