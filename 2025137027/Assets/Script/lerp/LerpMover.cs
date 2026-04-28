using UnityEngine;

public class LerpMover : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform startPos;
    public Transform endPos;

    [SerializeField] private float duration = 2f;
    [SerializeField] private float t = 0f;

    // Update is called once per frame
    void Update()
    {
        if (t<1f)
        {
            t += Time.deltaTime / duration;

            Vector3 a= startPos.position;
            Vector3 b= endPos.position;
            Vector3 p = (1f - t) * a + t * b;

            transform.position = p;
        }

        t += Time.deltaTime / duration;
        t = Mathf.PingPong(Time.time    / duration, 1f);
        transform.position = Vector3.Lerp(startPos.position, endPos.position, t);


    }
}
