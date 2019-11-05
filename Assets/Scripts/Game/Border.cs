using UnityEngine;

public class Border : MonoBehaviour
{
    public static Border Instance;

    private void Awake()
    {
        Instance = this;
    }

    private Transform bUp;
    private Transform bDown;
    private Transform bLeft;
    private Transform bRight;

    private void Start()
    {
        bUp = transform.Find("BorderUp");
        bDown = transform.Find("BorderDown");
        bLeft = transform.Find("BorderLeft");
        bRight = transform.Find("BorderRight");
    }

    public bool IsInside(Vector3 position)
    {
        if (position.x > bLeft.position.x
            && position.x < bRight.position.x
            && position.y > bDown.position.y
            && position.y < bUp.position.y)
        {
            return true;
        }
        else
        {
            return false;
        }     
    }
}
