using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataReferencer : MonoBehaviour
{
    public static LevelDataReferencer Instance { get; private set; }

    [Header("Level Data")]
    [SerializeField] private int movesUsed;
    [SerializeField] private int movesAvalible;
    [SerializeField] private int levelObjective;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        LevelDataUI.Instance.UpdateAllTexts();
    }

    public int GetMovesUsed()
    {
        return movesUsed;
    }

    public void SetMovesUsed(int value)
    {
        movesUsed = value;
        LevelDataUI.Instance.UpdateAllTexts();
    }

    public int GetMovesAvalible()
    {
        return movesAvalible;
    }

    public void SetMovesAvalible(int value)
    {
        movesAvalible = value;
        LevelDataUI.Instance.UpdateAllTexts();
    }

    public int GetLevelObjective()
    {
        return levelObjective;
    }

    public void SetLevelObjective(int value)
    {
        levelObjective = value;  
        LevelDataUI.Instance.UpdateAllTexts();
    }
}
