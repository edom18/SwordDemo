using UnityEngine;
using System.Collections;

public class MoveDagger : MonoBehaviour
{
    [SerializeField]
    float _moveSpeed = 8f;

    public void MoveTo(Vector3 targetPos)
    {
        StartCoroutine(MovoToImpl(targetPos));
    }

    IEnumerator MovoToImpl(Vector3 targetPos)
    {
        while(transform.position != targetPos)
        {
            yield return 0;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * _moveSpeed);
        }
    }
}
