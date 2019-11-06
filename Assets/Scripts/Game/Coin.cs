using System.Collections;
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
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime*10);
            if (Mathf.Approximately(targetPos.x,transform.position.x) || 
                Mathf.Approximately(targetPos.y,transform.position.y))
            {
                GameManager.Instance.AddCoins(Gold);
                if (gameObject.name.Contains("gold"))
                {
                    AudioManager.Instance.PlayAudio(ACName.Gold);
                }
                else if (gameObject.name.Contains("silver"))
                {
                    AudioManager.Instance.PlayAudio(ACName.Silver);
                }
                Destroy(gameObject);
                break;
            }
        }
       
    }
}
