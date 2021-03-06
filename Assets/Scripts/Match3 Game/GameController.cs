using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public int maxActionsNumber = 10;
    public UnityEvent<int, int> onUpdateDataEvent;
    public UnityEvent<int> onSimpeEndGame;
    public UnityEvent<int> onRecordEndGame;

    private int currentScore = 0;
    private int currentRemainingMotions = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentScore = 0;
        currentRemainingMotions = maxActionsNumber;
        onUpdateDataEvent.Invoke(currentScore, currentRemainingMotions);
    }

    public void TakeScore(int matchCount)
    {
        currentRemainingMotions--;
        currentScore += matchCount < 3 ? 1 : matchCount - 1;
        onUpdateDataEvent.Invoke(currentScore, currentRemainingMotions);

        if (currentRemainingMotions <= 0)
        {
            int index = GameManager.recordsList.AddNewRecord(currentScore);
            if (index != -1)
            {
                GameManager.newRecordIndex = index;
                SaveManager.SavePP<SaveData.RecordsList>(GameManager.RECORDS_KEY, GameManager.recordsList);
                onRecordEndGame.Invoke(currentScore);
            }
            else
            {
                onSimpeEndGame.Invoke(currentScore);
            }
        }
    }

    public void ContinueGame(int motions)
    {
        GameManager.cashAccount.money -= currentScore;
        GameManager.SaveCashAccount();

        currentRemainingMotions += motions;
    }
}
