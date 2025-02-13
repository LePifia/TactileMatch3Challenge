using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelDataUI : MonoBehaviour
{
    public static LevelDataUI Instance { get; private set; }

    [Header("Moves Used")]
    [Space]
    [SerializeField] TextMeshProUGUI movesUsedUI;

    [Space]
    [Header("Moves Left")]
    [Space]
    [SerializeField] TextMeshProUGUI movesAvalibleUI;
    [Space]

    [Header("Level Objective")]
    [Space]
    [SerializeField] TextMeshProUGUI levelObjectiveUI;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void UpdateAllTexts(){
        movesUsedUI.text = "Moves Used : " + LevelDataReferencer.Instance.GetMovesUsed().ToString();
        movesAvalibleUI.text = "Moves Left : " + LevelDataReferencer.Instance.GetMovesAvalible().ToString();

        if (LevelDataReferencer.Instance.GetLevelObjective() > 0){
            levelObjectiveUI.text = "level Objective : " + LevelDataReferencer.Instance.GetLevelObjective().ToString();
        }
        else{
            levelObjectiveUI.text = "level Objective : 0";
        }
        
    }
}
