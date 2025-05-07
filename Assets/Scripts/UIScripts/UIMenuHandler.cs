using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Linq;

public class UIMenuHandler : UIBaseScript
{
    [SerializeField] Button _startGameButton;
    [SerializeField] Button _leaderBordButton;
    [SerializeField] Button _backButton;
    [SerializeField] LeaderBoardDataHandler _leaderBoardPrefab;
    [SerializeField] Transform _leaderboardContent;
    [SerializeField] GameObject _leaderboardsTab;
    [SerializeField] GameObject _mainMenuTab;
    [SerializeField] List<PlayerSaveData> _saveDataList;

    public override void Start()
    {
        base.Start();
        SetData();
    }

    public override void AddListener()
    {
        base.AddListener();
        _startGameButton.onClick.AddListener(OnStartGame);
        _leaderBordButton.onClick.AddListener(OnLeaderBords);
        _backButton.onClick.AddListener(OnBack);
    }

    void OnStartGame()
    {
        SceneManager.LoadScene("Game");
    }

    void OnLeaderBords()
    {
        SetLeaderBoards();
    }

    private void OnBack()
    {
        _leaderboardsTab.SetActive(false);
        _mainMenuTab.SetActive(true);
    }

    void SetLeaderBoards()
    {
        _mainMenuTab.SetActive(false);
        _leaderboardsTab.SetActive(true);
        //deactivate all child
        if(_leaderboardContent.childCount != 0)
        {
            foreach(Transform child in _leaderboardContent)
            {
                child.gameObject.SetActive(false);
            }
        }
        //set and activate if equal amount
        if(_leaderboardContent.childCount >= _saveDataList.Count)
        {
            for(int i = 0 ; i < _saveDataList.Count ; i++)
            {
                _leaderboardContent.GetChild(i).GetComponent<LeaderBoardDataHandler>().SetData(_saveDataList[i]);
                _leaderboardContent.GetChild(i).gameObject.SetActive(true);
            }
            return;
        }
        //set data for the prefab that is already instantiated, instantiate and set data if not enough prefab;
        for(int i = 0; i < _saveDataList.Count; i++)
        {
            if( i <= _leaderboardContent.childCount)
            {
                LeaderBoardDataHandler leaderBoardData = Instantiate(_leaderBoardPrefab, _leaderboardContent);
                leaderBoardData.SetData(_saveDataList[i]);
                leaderBoardData.gameObject.SetActive(true);
                continue;
            }

            _leaderboardContent.GetChild(i).GetComponent<LeaderBoardDataHandler>().SetData(_saveDataList[i]);
            _leaderboardContent.GetChild(i).gameObject.SetActive(true);
        }
    }

    void SetData()
    {
        if(_saveDataList != null || _saveDataList.Count != 0)
        {
            _saveDataList.Clear();
        }
        PlayerSaveDataList _dataList = SaveSystem.LoadAll();
        _saveDataList = _dataList.players.OrderByDescending(data => data.highScore).ToList();
    }


}
