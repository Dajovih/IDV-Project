using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;

    [SerializeField] private Button _scoreButton;

    private CanvasGroup _canvasGroup;

    private void Start()
    {
        GameEvents.OnMainMenuEvent += OnMainMenu;

        _startButton.onClick.AddListener(OnStartButtonClicked);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
        _scoreButton.onClick.AddListener(OnScoreButtonClicked);
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnDestroy()
    {
        GameEvents.OnMainMenuEvent -= OnMainMenu;
    }

    private void OnMainMenu()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    private void HideMainMenu() {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }

    public void OnStartButtonClicked()
    {
        AudioManager.Instance.PlaySound2D("ClickSFX");
        HideMainMenu();
        GameEvents.OnPlayerNameScreenEvent?.Invoke();
    }

    public void OnExitButtonClicked()
    {
        _exitButton.interactable = false;
        AudioManager.Instance.PlaySound2D("ClickSFX");
        GameManager.Instance.ExitGame();
    }

    public void OnScoreButtonClicked()
    {
        AudioManager.Instance.PlaySound2D("ClickSFX");
        AudioManager.Instance.PlayMusic(AudioMusicType.Score);
        HideMainMenu();
        GameEvents.OnRankingScreenEvent?.Invoke();
    }
}