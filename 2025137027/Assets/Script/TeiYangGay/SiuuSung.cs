using UnityEngine;

public class SiuuSung : MonoBehaviour
{
    public Vector3 centerPoint = Vector3.zero;
    float radiusX = 300f;
    float radiusZ = 300f;
    float speed = 4.73f;

    float ruuningTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ruuningTime += Time.deltaTime * speed;
        float angle = ruuningTime;

        float x = centerPoint.x + Mathf.Cos(angle) * radiusX;
        float z = centerPoint.z + Mathf.Sin(angle) * radiusZ;

        transform.position = new Vector3 (x, z, centerPoint.z);
    }
}
