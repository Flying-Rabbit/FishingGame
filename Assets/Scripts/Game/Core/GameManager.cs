using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }


    public void AddCoins(int value)
    {
        Debug.Log("Add Coins:" + value);
    }

    public void AddExp(int value)
    {
        Debug.Log("Add Exp:" + value);
    }
}
