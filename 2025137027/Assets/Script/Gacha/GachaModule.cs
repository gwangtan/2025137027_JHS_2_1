using UnityEngine;

public class GachaModule
{
    public float baseLegendRate = 0.05f; // 5%
    public float currentLegendRate;

    public GachaModule() { currentLegendRate = baseLegendRate; }

    public string GetDropGrade()
    {
        float r = Random.value;

        // 전설 당첨 여부 확인
        if (r < currentLegendRate)
        {
            currentLegendRate = baseLegendRate; // 확률 초기화
            return "<color=yellow>전설</color>";
        }
        else
        {
            // 실패 시 확률 1.5% 증가
            currentLegendRate += 0.015f;

            // 나머지 등급 결정 (일반 50%, 고급 30%, 희귀 15%)
            float subRoll = Random.value;
            if (subRoll < 0.50f) return "일반";
            if (subRoll < 0.80f) return "<color=green>고급</color>";
            return "<color=blue>희귀</color>";
        }
    }
}