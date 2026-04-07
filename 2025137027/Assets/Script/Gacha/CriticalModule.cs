using UnityEngine;

public class CriticalModule
{
    public int totalHits = 0;
    public int critHits = 0;
    private float targetRate;

    public CriticalModule(float rate) { targetRate = rate; }

    public bool RollCrit()
    {
        totalHits++;
        // 이전까지의 실제 확률 계산 (나누기 0 방지)
        float currentRate = (totalHits > 1) ? (float)critHits / (totalHits - 1) : 0f;

        // 1. 강제 발생 (너무 안 떴을 때)
        if (currentRate < targetRate && (float)(critHits + 1) / totalHits <= targetRate)
        {
            critHits++;
            return true;
        }
        // 2. 강제 차단 (너무 많이 떴을 때)
        if (currentRate > targetRate) return false;

        // 3. 일반 확률
        if (Random.value < targetRate)
        {
            critHits++;
            return true;
        }
        return false;
    }
}