using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;

    [SerializeField] private Button _scoreButton;

    private void Start()
    {
        _startButton.onClick.AddListener(OnStartButtonClicked);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
        _scoreButton.onClick.AddListener(OnScoreButtonClicked);
    }

    public void OnStartButtonClicked()
    {
        _startButton.interactable = false;
        AudioManager.Instance.PlaySound2D("ClickSFX");
        GameManager.Instance.StartGame();
    }

    public void OnExitButtonClicked()
    {
        _exitButton.interactable = false;
        AudioManager.Instance.PlaySound2D("ClickSFX");
        GameManager.Instance.ExitGame();
    }

    public void OnScoreButtonClicked()
    {
        _scoreButton.interactable = false;
        AudioManager.Instance.PlaySound2D("ClickSFX");
        GameManager.Instance.ScoreMenu();
    }
}