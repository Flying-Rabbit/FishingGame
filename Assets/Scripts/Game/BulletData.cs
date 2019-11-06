using System.Xml;
using UnityEngine;

public class BulletData : MonoBehaviour
{
    public static BulletData Instance;
    private void Awake()
    {
        Instance = this;
        LoadBulletData();       
    }
    private BulletModel[] bulletArray;  
    void LoadBulletData()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(Resources.Load("bullet").ToString());
        XmlNodeList nodeList = xmlDoc.DocumentElement.GetElementsByTagName("bullet");

        bulletArray = new BulletModel[nodeList.Count];

        for (int i = 0; i < nodeList.Count; i++)
        {
            bulletArray[i] = new BulletModel();
            bulletArray[i].ID = int.Parse(nodeList[i].Attributes["id"].Value);         
            bulletArray[i].Level = int.Parse(nodeList[i].Attributes["level"].Value);
            bulletArray[i].Damage = int.Parse(nodeList[i].Attributes["damage"].Value);
            bulletArray[i].Cost = int.Parse(nodeList[i].Attributes["cost"].Value);
            bulletArray[i].PathName = nodeList[i].Attributes["assetname"].Value;
            bulletArray[i].BulletPrefab = Resources.Load<GameObject>("Bullet/" + bulletArray[i].PathName);           
        }
    }

    /// <summary>
    /// 获取子弹数据
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public BulletModel GetBullet(int id)
    {
        if (bulletArray != null)
        {
            for (int i = 0; i < bulletArray.Length; i++)
            {
                if (id == bulletArray[i].Level)
                    return bulletArray[i];
            }
        }
        return null;
    }
}

public class BulletModel
{
    public int ID;
    public int Level;
    public int Damage;
    public int Cost;
    public string PathName;
    public GameObject BulletPrefab;
}
