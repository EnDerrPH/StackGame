using UnityEngine;
using TMPro;

public class LeaderBoardDataHandler : MonoBehaviour
{
   [SerializeField] TMP_Text _playerName;
   [SerializeField] TMP_Text _highScore;

   public void SetData(PlayerSaveData saveData)
   {
      _playerName.text = saveData.playerName;
      _highScore.text = saveData.highScore.ToString();
   }
}
