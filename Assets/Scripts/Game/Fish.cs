using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
  
    private Border border;
    private Vector3 moveDirection;
    private float moveSpeed;
    private float angleSpeed;
    private FishModel fishData;
    private bool isStraight;


    private void Awake()
    {
        border = Border.Instance;
        isStraight = true;
    }   

    private void Update()
    {
        if (fishData != null)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            if (isStraight == false)
            {
                moveDirection = Quaternion.Euler(0, 0, angleSpeed * Time.deltaTime) * moveDirection;
                transform.right = moveDirection;
            }
        }

      
        
        if (!border.IsInside(transform.position))
        {
            Destroy(gameObject);
        }
    }


    public void Init(FishModel fish, Vector3 dir, float moveSpeed, bool isStraightLine = true, float angleSpeed = 0f)
    {
        this.fishData = fish;
        this.moveDirection = dir;
        this.moveSpeed = moveSpeed;
        this.isStraight = isStraightLine;
        this.angleSpeed = angleSpeed;         
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bullet"))
        {
            //销毁子弹
            Destroy(collision.gameObject);

            int damage = collision.gameObject.GetComponent<Bullet>().Damage;

            //生成渔网,继承子弹80%的伤害
            GameObject web = Instantiate(FishManager.Instance.WebPrefab, gameObject.transform.position, Quaternion.identity);
            web.GetComponent<FishWeb>().Damage = damage * 8 / 10;

            //自身扣血
            TakeDamage(damage);
        }
        else if (collision.CompareTag("web"))
        {
            TakeDamage(collision.gameObject.GetComponent<FishWeb>().Damage);
        }
        
    }

    void TakeDamage(int damage)
    {
        if (fishData != null)
        {
            fishData.HP -= damage;

            if (fishData.HP <= 0)
            {
                //生成金币
                GameObject die = Instantiate(fishData.fishDiePrefab);
                GameObject coinPrefab;
                if (fishData.Gold < 100)
                {
                    coinPrefab = FishManager.Instance.SilverCoinsPrefab;
                }
                else if (fishData.Gold > 500)
                {
                    coinPrefab = FishManager.Instance.BigCoinsPrefab;
                }
                else
                {
                    coinPrefab = FishManager.Instance.GoldCoinsPrefab;
                }

                GameObject coin = Instantiate(coinPrefab, gameObject.transform.position, Quaternion.identity);
                coinPrefab.AddComponent<Coin>().Init(fishData.Gold, FishManager.Instance.CoinCollectionPos);

                //得经验
                GameManager.Instance.AddExp(fishData.Exp);
            }
        }
    }

}
