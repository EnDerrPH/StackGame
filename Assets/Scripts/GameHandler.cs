using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameHandler : MonoBehaviour
{
    [SerializeField] BlockHandler _previousBlock;
    [SerializeField] BlockHandler _blockPrefab;
    [SerializeField] GameObject _platform;
    [SerializeField] MainCameraHandler _mainCameraHandler;
    [SerializeField] GameObject _UIPostGame;
    [SerializeField] GameObject _UIInGame;
    [SerializeField] UIGameHandler _UIGameHandler;
    [SerializeField] List<Material> _materialList = new List<Material>();
    [SerializeField] GameObject _fog;
    const float _fogSpeed = 1.5f;
    int _highScore;
    float _yOffset = .3f;
    BlockHandler _currentBlock;
    bool _isHorziontal;
    bool _canTap;
    [SerializeField] bool _isGameOver;
    bool _isFog;

    void Start()
    {
        _blockPrefab.transform.localScale = new Vector3(1f,.1f,1f);
        InitializeObjects();
        SetCanTap();
    }

    void Update()
    {
        MoveFog();
        OnScreenTap();
        GameOverByFog(_previousBlock);
    }

    private int MaterialIndex()
    {
        return Random.Range(0, _materialList.Count);
    }

    private void OnScreenTap()
    {
        if (_isGameOver)
        {
            return;
        }
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name != "Game" || _UIPostGame.gameObject.activeSelf)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!_canTap || _isGameOver)
            {
                return;
            }
            _isFog = true;
            Rigidbody rb = _currentBlock.GetComponent<Rigidbody>();
            _currentBlock.SetIsMoving(false);
            rb.useGravity = true;
            StartCoroutine(DelayedSpawn());
            _mainCameraHandler.MoveUp();
            _canTap = false;
        }
    }

    private void SetCanTap()
    {
        _canTap = true;
    }

    private void InitializeObjects()
    {
        GameObject platformObj =  Instantiate(_platform, _platform.transform.position , Quaternion.identity);
        float newYPosition = _platform.transform.localScale.y/ 2;
        Vector3 spawnPosition = new Vector3(0, newYPosition ,0);
        BlockHandler firstBlock = Instantiate(_blockPrefab, spawnPosition, Quaternion.identity);
        firstBlock.SetRenderer();
        firstBlock.SetBlockMaterial(_materialList[MaterialIndex()]);
        firstBlock.FreezePosition();
        firstBlock.enabled = false;
        _previousBlock = firstBlock;
        SpawnBlock();
        _mainCameraHandler.MoveUp();
    }

    private void SpawnBlock()
    {
        if (_isGameOver)
        {
            return;
        }
        SetSpawnPosition();
        BlockHandler newBlock = Instantiate(_blockPrefab, SetSpawnPosition(), Quaternion.identity);
        newBlock.SetRenderer();
        newBlock.SetBlockMaterial(_materialList[MaterialIndex()]);
        newBlock.SetBlockMovement(_isHorziontal);
        newBlock.OnGameOverEvent.AddListener(GameOver);
        newBlock.OnStackedEvent.AddListener(SetCanTap);
        _isHorziontal = !_isHorziontal;
        _currentBlock = newBlock;
    }

    void SliceBlock()
    {
        float hangoverX = _currentBlock.transform.position.x - _previousBlock.transform.position.x;
        float hangoverZ = _currentBlock.transform.position.z - _previousBlock.transform.position.z;

        float directionX = hangoverX > 0 ? 1f : -1f;
        float directionZ = hangoverZ > 0 ? 1f : -1f;

        float maxSizeX = _previousBlock.transform.localScale.x;
        float maxSizeZ = _previousBlock.transform.localScale.z;

        float overlapX = maxSizeX - Mathf.Abs(hangoverX);
        float overlapZ = maxSizeZ - Mathf.Abs(hangoverZ);

        // Game over if no overlap on either axis
        if (overlapX <= 0f || overlapZ <= 0f)
        {
            GameOver();
            return;
        }

        // Resize current block
        Vector3 newScale = _currentBlock.transform.localScale;
        newScale.x = overlapX;
        newScale.z = overlapZ;
        _highScore = _highScore + ((int)Mathf.Round((newScale.x + newScale.z) * 100));
        _UIGameHandler.UpdateHighScore(_highScore.ToString());
        
        _currentBlock.transform.localScale = newScale;
        _blockPrefab.transform.localScale = newScale; // if needed for next spawn

        // Reposition block to align with the previous one
        Vector3 newPosition = _currentBlock.transform.position;
        newPosition.x -= hangoverX / 2f;
        newPosition.z -= hangoverZ / 2f;
        _currentBlock.transform.position = newPosition;

        // Create falling piece(s)
        float fallingSizeX = Mathf.Abs(hangoverX);
        float fallingSizeZ = Mathf.Abs(hangoverZ);

        // Falling piece on X axis
        if (fallingSizeX > 0f)
        {
            float fallingX = newPosition.x + (overlapX / 2f + fallingSizeX / 2f) * directionX;

            GameObject fallingBlockX = GameObject.CreatePrimitive(PrimitiveType.Cube);
            fallingBlockX.transform.localScale = new Vector3(fallingSizeX, newScale.y, newScale.z);
            fallingBlockX.transform.position = new Vector3(fallingX, _currentBlock.transform.position.y, _currentBlock.transform.position.z);
            fallingBlockX.GetComponent<Renderer>().material = _currentBlock.GetComponent<Renderer>().material;
            fallingBlockX.AddComponent<Rigidbody>();
            Destroy(fallingBlockX, 1f);
        }

        // Falling piece on Z axis
        if (fallingSizeZ > 0f)
        {
            float fallingZ = newPosition.z + (overlapZ / 2f + fallingSizeZ / 2f) * directionZ;

            GameObject fallingBlockZ = GameObject.CreatePrimitive(PrimitiveType.Cube);
            fallingBlockZ.transform.localScale = new Vector3(newScale.x, newScale.y, fallingSizeZ);
            fallingBlockZ.transform.position = new Vector3(_currentBlock.transform.position.x, _currentBlock.transform.position.y, fallingZ);
            fallingBlockZ.GetComponent<Renderer>().material = _currentBlock.GetComponent<Renderer>().material;
            fallingBlockZ.AddComponent<Rigidbody>();
            Destroy(fallingBlockZ, 1f);
        }
    }

    private Vector3 SetSpawnPosition()
    {
        float newYPosition = _previousBlock.gameObject.transform.position.y +_yOffset;
        Vector3 spawnPosition = new Vector3(0, newYPosition ,0);
        return spawnPosition;
    }

    private void MoveFog()
    {
        if (!_isFog || _isGameOver)
        {
            return;
        }
        float targetFogY = _fog.transform.position.y + 0.03f;
        float newFogY = Mathf.Lerp(_fog.transform.position.y, targetFogY, Time.deltaTime * _fogSpeed);
        _fog.transform.position = new Vector3(_fog.transform.position.x, newFogY, _fog.transform.position.z);
    }

    private void GameOver()
    {
        _UIInGame.gameObject.SetActive(false);
        _UIPostGame.gameObject.SetActive(true);
        _UIGameHandler.UpdateHighScore(_highScore.ToString());
        _UIGameHandler.ActivateDarkBG();
        GameManager.Instance.GetPlayerData().HighScore = _highScore;
        _isGameOver = true;
    }

    private void GameOverByFog(BlockHandler blockHandler)
    {
        if (_fog == null || _isGameOver)
        {
            return;
        }
        if (_fog.transform.position.y > blockHandler.gameObject.transform.position.y + .1f)
        {
            GameOver(); 
        }
    }

    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(.5f); // Wait for 1 second

        SliceBlock();
        _previousBlock = _currentBlock; 
        SpawnBlock();
    }
}
