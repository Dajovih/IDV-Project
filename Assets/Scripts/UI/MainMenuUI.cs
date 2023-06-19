using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;

    private void Start()
    {
        _startButton.onClick.AddListener(OnStartButtonClicked);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    public void OnStartButtonClicked()
    {
        _startButton.interactable = false;
        GameManager.Instance.StartGame();
    }

    public void OnExitButtonClicked()
    {
        _exitButton.interactable = false;
        GameManager.Instance.ExitGame();
    }
}