using System;
using UnityEngine;
using UnityEngine.UI;

public class ReturnToMenuButton : MonoBehaviour
{
    private Button _returnButton;
    
    private void Start()
    {   
        _returnButton = GetComponent<Button>();
        _returnButton.onClick.AddListener(OnReturnButtonClicked);
    }

    void OnReturnButtonClicked()
    {   
        AudioManager.Instance.PlaySound2D("ClickSFX");
        GameManager.Instance.MainMenu();
    }
}