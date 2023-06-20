using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDNextLevelScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private TextMeshProUGUI _timeText;
    [SerializeField]
    private TextMeshProUGUI _levelText;
    [SerializeField]
    private GameObject _newMaxScore;
    [SerializeField] 
    private Button _nextButton;
    [SerializeField] 
    private Button _backButton;
    private int _nextlevel;

    private CanvasGroup _canvasGroup;
    
    private void Start()
    {
        GameEvents.OnNextLevelEvent += OnNextLevel;

        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
        
        _nextButton.onClick.AddListener(OnButtonNext);
        _backButton.onClick.AddListener(OnButtonBack);
    }

    private void OnDestroy()
    {
        GameEvents.OnNextLevelEvent -= OnNextLevel;
    }

    private void OnNextLevel(int score, bool isMaxScore, float time, int level)
    {
        SetResults(score, isMaxScore, time, level);

        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;

        _nextlevel = level + 1;
    }

    void SetResults(int score, bool isMaxScore, float time, int level)
    {   
        int _scoreLength = score.ToString().Length;
        string zeros = new String('0', 4-_scoreLength);;
        _scoreText.text = $"SCORE: {zeros}{score}";
        _newMaxScore.SetActive(isMaxScore);
        
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        _timeText.text = $"Time: {timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";

        _levelText.text = $"Level: {level}";
    }
    
    void OnButtonNext()
    {   
        if (_nextlevel > 3) {
            GameManager.Instance.ScoreMenu();
        } else {
            GameManager.Instance.Level(_nextlevel);
        }
    }

    void OnButtonBack()
    {   
        GameManager.Instance.MainMenu();
    }
}