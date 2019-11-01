using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 5f; 
    public int Demage { get; set; }

    private void Update()
    {
        transform.position += transform.up * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("border"))
        {
            Destroy(gameObject);
        }
    }
}
