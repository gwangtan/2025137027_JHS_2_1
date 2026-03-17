using UnityEngine;

public class planet : MonoBehaviour
{
    [Header("중심")]
    public Transform centerPlanet;

    [Header("거리, 자전, 공전")]
    public float distance;
    public float rotationSpeed; 
    public float revolutionSpeed;   

    float currentRevolutionAngle;
    float currentRotationAngle;

   void Rotation()
    {
        currentRotationAngle += rotationSpeed * Time.deltaTime;
        Vector3 currentEuler = transform.eulerAngles;
        currentEuler.y = currentRotationAngle;
        transform.eulerAngles = currentEuler;
    } 

    void Revolution()
    {
        currentRevolutionAngle += revolutionSpeed * Time.deltaTime;
        float rad = DegToRad(currentRevolutionAngle);
        Vector3 dir = new Vector3(Mathf.Cos(rad), 0.0f, Mathf.Sin(rad));
        transform.position = centerPlanet.position + (dir * distance);
    }
    void Update()
    {
        Revolution();
        Rotation();
    }
    

    float DegToRad(float deg) => deg * (Mathf.PI / 180);

}