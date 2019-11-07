using System;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gun : MonoBehaviour
{
    public static Gun Instance;
    private void Awake()
    {
        Instance = this;
        LoadGunData();
    }


    private GunModel[] gunData;
    void LoadGunData()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(Resources.Load("gun").ToString());
        XmlNodeList nodeList = xmlDoc.DocumentElement.GetElementsByTagName("gun");

        gunData = new GunModel[nodeList.Count];

        for (int i = 0; i < nodeList.Count; i++)
        {
           gunData[i] = new GunModel();
           gunData[i].ID = int.Parse(nodeList[i].Attributes["id"].Value);
           gunData[i].Type = int.Parse(nodeList[i].Attributes["type"].Value);
           gunData[i].BulletID = int.Parse(nodeList[i].Attributes["bulletid"].Value);
           gunData[i].DamageRate = float.Parse(nodeList[i].Attributes["damageRate"].Value);
           gunData[i].BulletData = BulletData.Instance.GetBullet(gunData[i].BulletID);
        }
        
    }    

    private const float SHOTCD = 0.2f;
    private Transform gunTrans;    
    private Camera cameraMain;
    private Transform[] gunModelTrans;
    private Transform[] bulletSpawnTrans;

    private GunModel currentGunData; //当前的枪的数据
    private Transform currenBulletSpawnTrans; //当前枪发射子弹初始Transform
    private int currentGunID;

    bool flag = true;
    private void Start()
    {     
        gunTrans = transform;
        cameraMain = Camera.main;
        gunModelTrans = new Transform[transform.childCount];
        bulletSpawnTrans = new Transform[gunModelTrans.Length];
        for (int i = 0; i < transform.childCount; i++)
        {
            gunModelTrans[i] = transform.GetChild(i);
            bulletSpawnTrans[i] = gunModelTrans[i].Find("BulletPos");
            gunModelTrans[i].gameObject.SetActive(false);
        }

        currentGunID = 0;
        InitGunByIndex(currentGunID);

        if (Application.platform == RuntimePlatform.Android)
            flag = false;
    }

    void InitGunByIndex(int gunID)
    {
        //缓存当前枪的数据
        currentGunData = gunData[gunID];
        int gunType = currentGunData.Type;
        
        //设置当前需要显示的枪模型
        for (int i = 0; i < gunModelTrans.Length; i++)
        {
            gunModelTrans[i].gameObject.SetActive(i == gunType);
        }

        //缓存当前子弹初始Transform
        currenBulletSpawnTrans = bulletSpawnTrans[gunType];
    }

    public void GunLevelUp()
    {
        currentGunID += 1;
        if (currentGunID >= gunData.Length)
        {
            currentGunID = gunData.Length - 1;
        }
        InitGunByIndex(currentGunID);
    }

    public void GunLevelDown()
    {
        currentGunID -= 1;
        if (currentGunID < 0)
        {
            currentGunID = 0;
        }
        InitGunByIndex(currentGunID);
    }

    public int GetBulletCost()
    {
        return currentGunData.BulletData.Cost;
    }

    //朝向点击位置
    float degree;
    
    void GunRoate(Vector3 touchPos)
    {
        touchPos.z = 0;      
        degree = Vector3.Angle(touchPos - gunTrans.position, Vector3.up);      
        degree *= Mathf.Sign(gunTrans.position.x - touchPos.x);
        gunTrans.rotation = Quaternion.Euler(0, 0, degree);        
    }

    //开火    
    private bool isCooling = false;
    private GameObject newBullet;
    void GunFire()
    {
        if (isCooling)
            return;
        isCooling = true;

        if (GameManager.Instance.DecreaseCoins(currentGunData.BulletData.Cost))
        {
            GameObject bulletPrefab = currentGunData.BulletData.BulletPrefab;
            if (bulletPrefab != null)
            {
                newBullet = Instantiate(bulletPrefab);
                newBullet.transform.position = currenBulletSpawnTrans.position;
                newBullet.transform.rotation = currenBulletSpawnTrans.rotation;
                newBullet.GetComponent<Bullet>().Damage = (int)(currentGunData.DamageRate * currentGunData.BulletData.Damage);               
                AudioManager.Instance.PlayAudio(ACName.Fire);
            }
        }       
    }

    float timer = 0f;
    private void Update()
    {
        //if (Input.touchCount > 0)
        //{
        //    if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        //    {
        //        GunRoate(camera.ScreenToWorldPoint(Input.GetTouch(0).position));
        //    }       
        //}

       
        if (Input.GetMouseButton(0))
        { 
            //安卓平台 
            if (flag == false &&　EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return;                            
            }
            //windows平台
            if (flag && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            GunRoate(cameraMain.ScreenToWorldPoint(Input.mousePosition));
            if (!isCooling)
            {
                GunFire();
                timer = SHOTCD;
                isCooling = true;
            }
            else
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    isCooling = false;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isCooling = false;
        }
    }
}

public class GunModel
{
    public int ID;
    public int Type;
    public int BulletID;
    public float DamageRate;
    public BulletModel BulletData;
}