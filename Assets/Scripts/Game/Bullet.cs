using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 5f;
    private Border border;
    public int Damage { get; set; }

    private void Start()
    {
        border = Border.Instance;
    }

    private void Update()
    {
        transform.position += transform.up * Time.deltaTime * speed;
        if (!border.IsInside(transform.position))
        {
            Destroy(gameObject);
        }           
    }

    

}
