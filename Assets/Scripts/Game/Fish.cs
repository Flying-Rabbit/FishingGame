using UnityEngine;

public class Fish : MonoBehaviour
{

    private Border border;
    private Vector3 moveDirection;
    private float moveSpeed;
    private float angleSpeed;
    private FishModel fishData;
    private bool isStraight;
    private bool isDie = false;

    private void Awake()
    {
        border = Border.Instance;
        isStraight = true;
    }

    private void Update()
    {
        if (fishData != null && isDie == false)
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
            AudioManager.Instance.PlayAudio(ACName.Web);

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
                isDie = true;

                //生成金币               
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
                coin.GetComponent<SpriteRenderer>().sortingOrder = GetLayer();
                coin.AddComponent<Coin>().Init(fishData.Gold, FishManager.Instance.CoinCollectionPos);

                //得经验
                GameManager.Instance.AddExp(fishData.Exp);
                if (fishData.Exp >= 300)
                {
                    AudioManager.Instance.PlayAudio(ACName.BigFish);
                }

                //播放死亡动画
                Instantiate(fishData.fishDiePrefab, gameObject.transform.position, gameObject.transform.rotation);

                //销毁自身
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// 给金币设置层，防止重叠闪烁问题
    /// </summary>
    private int layer = 0;
    int GetLayer()
    {
        layer = (layer + 1) % 80;
        return layer + 100;
    }

}
