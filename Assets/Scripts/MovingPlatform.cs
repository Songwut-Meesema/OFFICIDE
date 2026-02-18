using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform waypoint; 
    public float speed = 2f;   
    public float waitTime = 2f; 

    private Vector3 startPos;  
    private bool isMovingUp = true; 

    void Start()
    {
        startPos = transform.position; 
        StartCoroutine(MovePlatform());
    }

    IEnumerator MovePlatform()
    {
        while (true)
        {
            if (isMovingUp)
            {
                while (Vector3.Distance(transform.position, waypoint.position) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, waypoint.position, speed * Time.deltaTime);
                    yield return null;
                }

                yield return new WaitForSeconds(waitTime);

                isMovingUp = false;
            }
            else
            {
                while (Vector3.Distance(transform.position, startPos) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
                    yield return null;
                }

                yield return new WaitForSeconds(waitTime);

                isMovingUp = true;
            }
        }
    }
}
