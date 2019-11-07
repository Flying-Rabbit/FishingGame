using System;
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
  
    public GameData GameData { get; private set; }
    private string[] titles = { "新手", "入门", "钢铁", "青铜", "白银", "黄金", "白金", "钻石", "大师", "宗师" };
    public event Action UIDataChanged;//UI面板数据更新事件
    public event Action<int> LevelUpEvent;//升级事件
    public event Action<int> UIBulletCostChanged;//子弹价格变动事件

    public string GetTitle()
    {
        int index = GameData.Level / 10;
        return titles[index];
    }

    public void AddCoins(int value)
    {
        if (value <= 0)
        {
            Debug.Log("数据有误，获取金币应该为正数");
        }

        GameData.Gold += value;
        if (GameData.Gold > 999999999)
        {
            GameData.Gold = 999999999;
        }   
        UIDataChanged?.Invoke();
    }

    public bool DecreaseCoins(int value)
    {
        if (value <= 0)
        {
            Debug.Log("数据有误，子弹消耗的金币应该为正数");
        }

        if (GameData.Gold - value < 0)
        {
            Debug.Log("金钱不足");
            return false;
        }

        GameData.Gold -= value;
        UIDataChanged?.Invoke();
        return true;
    }

    public void AddExp(int value)
    {
        GameData.Exp += value;
        int levelUpExp = GameData.Level * GameData.Level * 100;
        if (GameData.Exp >= levelUpExp)
        {
            GameData.Level += 1;
            GameData.Exp -= levelUpExp;
            LevelUpEvent?.Invoke(GameData.Level);
            AudioManager.Instance.PlayAudio(ACName.LevelUp);
        }
        UIDataChanged?.Invoke();
    }

    public void GunLevelChanged(int value)
    {
        switch (value)
        {
            case 1:
                Gun.Instance.GunLevelUp();
                break;
            case -1:
                Gun.Instance.GunLevelDown();
                break;
            default:
                break;
        }
        UIBulletCostChanged?.Invoke(Gun.Instance.GetBulletCost());
        AudioManager.Instance.PlayAudio(ACName.ChangeGun);
    }

    public void SetAudio(bool isOn)
    {
        GameData.AudioIsOn = isOn;
    }

    private void Start()
    {
        LoadGame();
        UIBulletCostChanged?.Invoke(Gun.Instance.GetBulletCost());
        UIDataChanged?.Invoke();
    }
    public void SaveGame()
    {
        PlayerPrefs.SetInt("LEVEL", GameData.Level);
        PlayerPrefs.SetInt("EXP", GameData.Exp);
        PlayerPrefs.SetInt("Gold", GameData.Gold);
        PlayerPrefs.SetInt("AUDIO", GameData.AudioIsOn ? 1 : 0);
    }

    public void LoadGame()
    {
        GameData = new GameData();
        GameData.Level = PlayerPrefs.GetInt("LEVEL", 0);
        GameData.Exp = PlayerPrefs.GetInt("EXP", 0);
        GameData.Gold = PlayerPrefs.GetInt("Gold", 10000);
        GameData.AudioIsOn = PlayerPrefs.GetInt("AUDIO", 1) == 1; //0：静音   1：打开声音        
        GetComponent<UIManager>().Init();       
    }
}

public class GameData
{
    public int Level;
    public int Exp;
    public int Gold;

    public bool AudioIsOn;
    public int GunLevel;
    public int BigTimerReward;
    public int LittleTimerReward;

    public GameData()
    {
        this.AudioIsOn = true;
        this.GunLevel = 0;
        this.BigTimerReward = 10000;
        this.LittleTimerReward = 100;
    }
}

//升级经验 = LV * LV * 100