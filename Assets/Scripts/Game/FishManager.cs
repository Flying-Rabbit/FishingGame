using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public static FishManager Instance;
    private void Awake()
    {
        Instance = this;
        SetFishSpawnParam();
        LoadFishData();
        BigCoinsPrefab = Resources.Load<GameObject>("Coins/bigCoin");
        GoldCoinsPrefab = Resources.Load<GameObject>("Coins/gold");
        SilverCoinsPrefab = Resources.Load<GameObject>("Coins/silver");
    }

    private void Start()
    {
        isFishGenerationg = true;
        StartCoroutine(StartGenerate());
    }

    #region 鱼生成
    /// <summary>
    /// 启动
    /// </summary>
    /// <returns></returns>
    IEnumerator StartGenerate()
    {
        yield return null;
        while (true)
        {
            if (!isFishGenerationg)
            {
                break;
            }

            yield return new WaitForSeconds(fishwaveGenWaitTime);
            StartCoroutine(GenerateFishes());
        }
    }


    /// <summary>
    /// 设置鱼的生成点，和初始游动方向
    /// </summary>
    private Vector3[] fishSpawnPos;
    private Vector3[] fishSpawnDirection;
    void SetFishSpawnParam()
    {
        int count = 16;
        fishSpawnPos = new Vector3[count];
        fishSpawnDirection = new Vector3[count];
        Camera ca = Camera.main;

        Vector3 MID = ca.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 50));
        Vector3 LT = ca.ScreenToWorldPoint(new Vector3(0, Screen.height, 50));
        Vector3 RT = ca.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 50));
        Vector3 RD = ca.ScreenToWorldPoint(new Vector3(Screen.width, 0, 50));
        Vector3 LD = ca.ScreenToWorldPoint(new Vector3(0, 0, 50));

        //4个角
        fishSpawnPos[0] = LT;
        fishSpawnPos[1] = RT;
        fishSpawnPos[2] = RD;
        fishSpawnPos[3] = LD;

        fishSpawnDirection[0] = (MID - LT).normalized;
        fishSpawnDirection[1] = (MID - RT).normalized;
        fishSpawnDirection[2] = (MID - RD).normalized;
        fishSpawnDirection[3] = (MID - LD).normalized;

        Vector3 up = LT - LD;
        Vector3 right = RD - LD;
        //左右各3个
        for (int i = 0; i < 3; i++)
        {
            fishSpawnPos[4 + i] = LD + up * 0.25f * (i + 1);
            fishSpawnDirection[4 + i] = Vector3.right;

            fishSpawnPos[7 + i] = RD + up * 0.25f * (i + 1);
            fishSpawnDirection[7 + i] = Vector3.left;
        }

        //上4个
        for (int i = 0; i < 4; i++)
        {
            fishSpawnPos[10 + i] = LT + right * 0.2f * (i + 1);
            fishSpawnDirection[10 + i] = Vector3.down;
        }

        //下2个
        fishSpawnPos[14] = LD + right * 0.3f;
        fishSpawnDirection[14] = Vector3.up;
        fishSpawnPos[15] = RD - right * 0.3f;
        fishSpawnDirection[15] = Vector3.up;

        //金币收集板的位置
        CoinCollectionPos = LD + (right / 6f);
        CoinCollectionPos = new Vector3(CoinCollectionPos.x, CoinCollectionPos.y, 0);
        //for (int i = 0; i < 16; i++)
        //{
        //    Debug.DrawLine(fishSpawnPos[i], fishSpawnPos[i] + fishSpawnDirection[i] * 5, Color.red, 100f);
        //}
    }

    /// <summary>
    /// 读取鱼的配置：HP，Count，Speed.....
    /// </summary>
    private FishModel[] fishArray;
    void LoadFishData()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(Resources.Load("fish").ToString());
        XmlNodeList nodeList = xmlDoc.DocumentElement.GetElementsByTagName("fish");

        fishArray = new FishModel[nodeList.Count];

        for (int i = 0; i < nodeList.Count; i++)
        {
            fishArray[i] = new FishModel();
            fishArray[i].Id = int.Parse(nodeList[i].Attributes["Id"].Value);
            fishArray[i].Name = nodeList[i].Attributes["Name"].Value;
            fishArray[i].HP = int.Parse(nodeList[i].Attributes["HP"].Value);
            fishArray[i].Exp = int.Parse(nodeList[i].Attributes["Exp"].Value);
            fishArray[i].Gold = int.Parse(nodeList[i].Attributes["Gold"].Value);
            fishArray[i].MaxNum = int.Parse(nodeList[i].Attributes["MaxNum"].Value);
            fishArray[i].MaxSpeed = int.Parse(nodeList[i].Attributes["MaxSpeed"].Value);
            fishArray[i].fishPrefab = Resources.Load<GameObject>("Fish/" + fishArray[i].Name);
            fishArray[i].fishDiePrefab = Resources.Load<GameObject>("Fish/die/" + fishArray[i].Name + "_die");
        }
    }

    /// <summary>
    /// 鱼生成算法
    /// </summary>
    private float fishGenWaitTime = 0.7f;//每条鱼的出生间隔
    private float fishwaveGenWaitTime = 0.5f;//每波鱼的出生间隔
    IEnumerator GenerateFishes()
    {
        yield return null;

        int posIndex = Random.Range(0, 16);
        int fishIndex = Random.Range(0, fishArray.Length);

        //出生点和初始方向
        Vector3 fishPos = fishSpawnPos[posIndex];
        Vector3 fishDirection = fishSpawnDirection[posIndex];

        //初始速度和数量
        int fishMaxSpeed = fishArray[fishIndex].MaxSpeed;
        int fishMaxNum = fishArray[fishIndex].MaxNum;
        float moveSpeed = Random.Range(fishMaxSpeed / 2, fishMaxSpeed);
        int count = Random.Range(fishMaxNum / 2, fishMaxNum);
        int moveType = Random.Range(0, 2);

        if (moveType == 0)
        {
            //0----走直线
            float angleOffset = Random.Range(-25f, 25f);
            Vector3 newDir = Quaternion.Euler(0, 0, angleOffset) * fishDirection;
            for (int i = 0; i < count; i++)
            {
                GameObject fish = Instantiate(fishArray[fishIndex].fishPrefab);
                fish.transform.position = fishPos; //位置
                fish.transform.right = newDir;//朝向
                fish.AddComponent<Fish>().Init(fishArray[fishIndex], newDir, moveSpeed);
                fish.GetComponent<SpriteRenderer>().sortingOrder = GetLayer();
                yield return new WaitForSeconds(fishGenWaitTime);
            }

        }
        else
        {
            //1----走弧线
            float angleSpeed = Mathf.Sign(Random.Range(-1f, 1f)) * Random.Range(9f, 15f);
            for (int i = 0; i < count; i++)
            {
                GameObject fish = Instantiate(fishArray[fishIndex].fishPrefab);
                fish.transform.position = fishPos; //位置
                fish.transform.right = fishDirection;//朝向
                fish.AddComponent<Fish>().Init(fishArray[fishIndex], fishDirection, moveSpeed, false, angleSpeed);
                fish.GetComponent<SpriteRenderer>().sortingOrder = GetLayer();
                yield return new WaitForSeconds(fishGenWaitTime);
            }
        }
    }


    /// <summary>
    /// 给鱼设置层，放置重叠闪烁问题
    /// </summary>
    private int layer = 0;
    int GetLayer()
    {
        layer = (layer + 1) % 80;
        return layer;
    }
    #endregion

    /// <summary>
    /// 控制鱼生成
    /// </summary>
    private bool isFishGenerationg;
    public bool IsFishGenerating
    {
        get { return isFishGenerationg; }
        set
        {
            if (isFishGenerationg != value)
            {
                isFishGenerationg = value;
                if (isFishGenerationg)
                {
                    StartCoroutine(StartGenerate());
                }
            }

        }
    }


    /// <summary>
    /// 渔网，金币资源
    /// </summary>
    private GameObject webPrefab;
    public GameObject WebPrefab 
    {
        get 
        {
            if (webPrefab == null)
            {
                webPrefab = Resources.Load<GameObject>("fishWeb");
            }
            return webPrefab;
        }
    }
    public GameObject BigCoinsPrefab { get; private set; }
    public GameObject GoldCoinsPrefab { get; private set; }
    public GameObject SilverCoinsPrefab { get; private set; }
    public Vector3 CoinCollectionPos { get; private set; }


}

public class FishModel
{
    public int Id;
    public string Name;
    public int HP;
    public int Exp;
    public int Gold;
    public int MaxNum;
    public int MaxSpeed;
    public GameObject fishPrefab;
    public GameObject fishDiePrefab;
}
