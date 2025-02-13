using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryConditionsManager : MonoBehaviour
{
    public static VictoryConditionsManager Instance { get; private set; }

    [SerializeField] private GameObject victoryButton;
    [SerializeField] private GameObject looseButton;

    [SerializeField] private bool endOfGame;


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void CheckVictoryOrLoseConditions(){
        if (LevelDataReferencer.Instance.GetMovesAvalible() == 0){
            looseButton.SetActive(true);
            endOfGame = true;
        }

        if (LevelDataReferencer.Instance.GetLevelObjective() <= 0){
            victoryButton.SetActive(true);
            endOfGame = true;
        }
    }

    public bool GetEndOfGame()
    {
        return endOfGame;
    }
}
