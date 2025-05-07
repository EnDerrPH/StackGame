using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerData _playerData;
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: Keep across scenes
    }

    public PlayerData GetPlayerData()
    {
        return _playerData;
    }
}
