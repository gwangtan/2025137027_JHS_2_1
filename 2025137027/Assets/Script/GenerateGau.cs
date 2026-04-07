using UnityEngine;
using UnityEngine.UI;

public class Gener : MonoBehaviour
{

    public float Mean;
    public float stdDev;

    public void Test()
    {
        Debug.Log(Generate(Mean, stdDev));
    }

    
    float Generate(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;

        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1))*
            Mathf.Sin(2.0f*Mathf.PI*u2);
        return mean + stdDev * randStdNormal;
    }
}
