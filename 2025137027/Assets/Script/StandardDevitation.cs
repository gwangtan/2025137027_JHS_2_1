using System.Linq;
using UnityEngine;

public class StandardDevitation1 : MonoBehaviour
{
    public int sampleCount = 1000;
    public int minValue = 0;
    public int maxValue = 10000;

    public void Test()
    {
        StandardDeviation();
    }
    

    void StandardDeviation()
    {
        int n = sampleCount; // »ùÇÃ ¼ö
        float[] samples = new float[n];
        for (int i = 0; i < n; i++)
        {
            samples[i] = Random.Range(minValue, maxValue);
        }

        float mean = samples.Average();
        float sumOfSquares = samples.Sum(x => Mathf.Pow(x - mean, 2));
        float stdDev = Mathf.Sqrt(sumOfSquares / n);

        Debug.Log($"Æò±Õ: {mean}, Ç¥ÁØÆíÂ÷: {stdDev}");
    }
}
