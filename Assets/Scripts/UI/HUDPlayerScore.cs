using TMPro;
using UnityEngine;
using DG.Tweening;
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
        int _scoreLength = score.ToString().Length;
        string zeros = new String('0', 4-_scoreLength);;
        _scoreText.text = $"SCORE: {zeros}{score}";
        Vector2 anchorPos = _scoreText.rectTransform.anchoredPosition;
        _scoreText.rectTransform.DOJumpAnchorPos(anchorPos, 25, 1, 0.5f);
    }
    
}
