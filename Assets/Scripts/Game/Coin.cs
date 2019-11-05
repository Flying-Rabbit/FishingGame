using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{  
    public int Gold { get; private set; }

    public void Init(int gold, Vector3 targetPos)
    {       
        Gold = gold;
        StartCoroutine(UpdatePos(targetPos));
    }

    IEnumerator UpdatePos(Vector3 targetPos)
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            yield return null;
            Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime);
            if (Mathf.Approximately(targetPos.x, transform.position.x) || Mathf.Approximately(targetPos.y, transform.position.y))
            {
                GameManager.Instance.AddCoins(Gold);               
                break;
            }
        }
       
    }
}
