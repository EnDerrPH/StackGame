using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] string _playerName;
    [SerializeField] int _highScore;

    public string PlayerName {get => _playerName ; set => _playerName = value;}
    public int HighScore {get => _highScore ; set => _highScore = value;}
}
