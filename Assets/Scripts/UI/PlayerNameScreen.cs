using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameScreen : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _playerInput;

    [SerializeField]
    private Button _backButton;

    [SerializeField]
    private Button _nextButton;

    private CanvasGroup _canvasGroup;
    
    private void Start()
    {
        GameEvents.OnPlayerNameScreenEvent += OnPlayerNameScreen;

        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
        
        _backButton.onClick.AddListener(OnBackButtonClicked);
        _nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    private void OnDestroy()
    {
        GameEvents.OnPlayerNameScreenEvent -= OnPlayerNameScreen;
    }

    private void OnPlayerNameScreen()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    private void HidePlayerNameScreen() {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }

    void OnBackButtonClicked()
    {   
        AudioManager.Instance.PlaySound2D("ClickSFX");
        HidePlayerNameScreen();
        GameEvents.OnMainMenuEvent?.Invoke();
    }

    public void OnNextButtonClicked()
    {
        _nextButton.interactable = false;
        AudioManager.Instance.PlaySound2D("ClickSFX");
        GameEvents.OnPlayerNameChangeEvent?.Invoke(_playerInput.text.ToUpper());
    }
}