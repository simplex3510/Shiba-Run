using System.Collections;

using UnityEngine;

public class CameraMoveController : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 endPoint = Vector3.zero;

    public float smoothTime = 0.3f; // 감속 시간 조절

    private void Start()
    {
        transform.position = startPoint;
        StartCoroutine(MoveCamera());
    }

    private IEnumerator MoveCamera()
    {
        Vector3 velocity = Vector3.zero;

        while ((transform.position - endPoint).magnitude > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, endPoint, ref velocity, smoothTime);
            yield return null; // 다음 프레임까지 대기
        }

        transform.position = endPoint; // 정확히 목표 위치 고정
        yield break;
    }
}
