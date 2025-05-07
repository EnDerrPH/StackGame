using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class UIGameHandler : UIBaseScript
{
    [SerializeField] Button _playAgainButton;
    [SerializeField] Button _saveDataButton;
    [SerializeField] Button _confirmButton;
    [SerializeField] Button _mainMenuButton;
    [SerializeField] TMP_Text _highScoreInGame;
    [SerializeField] TMP_Text _highScorePostGame;
    [SerializeField] GameObject _inputName;
    [SerializeField] TMP_InputField _InputField;
    [SerializeField] GameObject _scrollView;
    [SerializeField] TMP_Text _warningText;
    const int _maxCharacterName = 16;
    const int _minimumCharacterName = 3;

    public override void Start()
    {
        base.Start();
        _saveDataButton.interactable = true;
    }

    public override void AddListener()
    {
        base.AddListener();
        _playAgainButton.onClick.AddListener(PlayerAgain);
        _saveDataButton.onClick.AddListener(SaveData);
        _confirmButton.onClick.AddListener(OnConfirm);
        _mainMenuButton.onClick.AddListener(OnMainMenu);
    }

    private void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void SaveData()
    {
        _inputName.SetActive(true);
        _scrollView.SetActive(false);
    }

    public void UpdateHighScore(string score)
    {
        _highScoreInGame.text = score;
        _highScorePostGame.text = "HIGHSCORE: " + score;
    }

    private void PlayerAgain()
    {
        SceneManager.LoadScene("Game");
    }

     void OnConfirm()
    {
        int charCount = _InputField.text.Length;
        if(charCount <= _minimumCharacterName)
        {
            _warningText.gameObject.SetActive(true);
            _warningText.text = "Name is too short. Must be 4 or more characters.";
            return;
        }

        if(charCount > _maxCharacterName)
        {
            _warningText.gameObject.SetActive(true);
            _warningText.text = "Name is too long. Must be less than 16.";
            return;
        }
        PlayerData playerData = GameManager.Instance.GetPlayerData();
        playerData.PlayerName = _InputField.text;
        _inputName.SetActive(false);
        _scrollView.SetActive(true);
        _saveDataButton.interactable = false;
        SaveSystem.Save(playerData);
    }
}
