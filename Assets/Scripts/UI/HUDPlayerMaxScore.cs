using TMPro;
using UnityEngine;
using DG.Tweening;
using System;

public class HUDPlayerMaxScore : MonoBehaviour
{
    private TextMeshProUGUI _maxScoreText;
    
    void Start()
    {
        _maxScoreText = GetComponent<TextMeshProUGUI>();
        GameEvents.OnStartGameEvent += SetMaxScore;
    }

    private void OnDestroy()
    {
        GameEvents.OnStartGameEvent -= SetMaxScore;
    }

    
    private void SetMaxScore(int score)
    {   
        int scoreLength = score.ToString().Length;
        string zeros = new String('0', 4 - scoreLength);;
        _maxScoreText.text = $"HIGH SCORE: {zeros}{score}";
        Vector2 anchorPos = _maxScoreText.rectTransform.anchoredPosition;
        _maxScoreText.rectTransform.DOJumpAnchorPos(anchorPos, 25, 1, 0.5f);
    }
    
}
