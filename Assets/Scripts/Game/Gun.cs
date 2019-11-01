using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gun : MonoBehaviour
{
    private const float SHOTCD = 0.2f;
    private Transform gunTrans;  //当前枪
    public Transform bulletSpawn; //当前子弹的生成点
    public GameObject bulletPrefab;  //子弹
    private Camera cameraMain;
    private Transform[] gunModels;

    public Action<int> BulletCostChangeDeleget; //子弹价格变化事件
    public Action<GameObject> BulletPrefabChangeDelegate; //子弹预制体变化事件

    private void Start()
    {
        cameraMain = Camera.main;
        gunTrans = transform;
        gunModels = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            gunModels[i] = transform.GetChild(i);            
        }
    }

    void ChangeGunModelByIndex(int index)
    { 
        
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

        if (bulletPrefab != null)
        {
            newBullet = Instantiate(bulletPrefab);
            newBullet.transform.position = bulletSpawn.position;
            newBullet.transform.rotation = bulletSpawn.rotation;       
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

        ///windows平台
        if (Input.GetMouseButton(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
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
        }
        if (Input.GetMouseButtonUp(0))
        {
            isCooling = false;
        }
    }
}
