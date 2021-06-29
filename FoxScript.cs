using UnityEngine;

public class FoxScript : MonoBehaviour
{
    public float speed;
    public Transform point1;
    public Transform point2;
    float value;
    bool pointTo1;
    bool pointTo2;

    void Start()
    {
        transform.position = point2.position;
        pointTo1 = true;
        pointTo2 = false;
        speed += Random.Range(-.25f, .25f);
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(point2.position, point1.position, value);

        if (pointTo1)
        {
            value += speed * Time.deltaTime;
            transform.LookAt(point1.position);
        }
        if (value >= 1)
        {
            pointTo1 = false;
            pointTo2 = true;
        }
        if (value <= 0)
        {
            pointTo2 = false;
            pointTo1 = true;
        }
        if (pointTo2)
        {
            value -= speed * Time.deltaTime;
            transform.LookAt(point2.position);
        }
    }
}
