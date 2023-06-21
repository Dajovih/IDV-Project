using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingScreen : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _playersScore;

    [SerializeField] 
    private Button _backButton;


    private CanvasGroup _canvasGroup;
    
    private void Start()
    {
        GameEvents.onRankingScreenEvent += OnRankingScreen;

        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
        
        _backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnDestroy()
    {
        GameEvents.onRankingScreenEvent -= OnRankingScreen;
    }

    private void OnRankingScreen()
    {
        SetRanking();
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    private void HideRanking() {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }
 
    void SetRanking()
    {   
        int playerRankingID = 0;
        foreach (GameObject playersScore in _playersScore)
        {
            TextMeshProUGUI nameText = playersScore.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI maxScoreText = playersScore.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();

            string playerName = GameManager.Instance.ranking.players[playerRankingID].name;
            int playerMaxScore = GameManager.Instance.ranking.players[playerRankingID].maxScore;

            int scoreLength = playerMaxScore.ToString().Length;
            string zeros = new String('0', 4-scoreLength);;
            maxScoreText.text = $"{zeros}{playerMaxScore}";
            nameText.text = $"{playerName}";

            playerRankingID++;
        }
    }

    void OnBackButtonClicked()
    {   
        AudioManager.Instance.PlaySound2D("ClickSFX");
        AudioManager.Instance.PlayMusic(AudioMusicType.Menu);
        HideRanking();
        GameEvents.onMainMenuEvent?.Invoke();
    }
}