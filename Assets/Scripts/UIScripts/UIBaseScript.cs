using System;
using UnityEngine;
using UnityEngine.UI;

public class UIBaseScript : MonoBehaviour
{
    [SerializeField] protected Button _exitGameButton;
    public virtual void Start()
    {
        AddListener();
    }

    public virtual void AddListener()
    {
        _exitGameButton.onClick.AddListener(OnExitGame);
    }

    public virtual void OnExitGame()
    {
        Application.Quit();
    }
}
