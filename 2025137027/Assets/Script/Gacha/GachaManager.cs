using UnityEngine;

public class GachaManager : MonoBehaviour
{
    // 버튼 등에 연결해서 사용할 실제 실행 메서드
    public void ClickGachaButton()
    {
        string result = Simulate();
        Debug.Log("가챠 결과: " + result);
    }

    // 1회 뽑기의 핵심 로직 (확률 계산만 담당)
    string Simulate()
    {
        float r = Random.value; // 0.0 ~ 1.0 사이의 랜덤 값

        if (r < 0.4f) return "C";      // 40% 확률
        if (r < 0.7f) return "B";      // 30% 확률 (0.4 ~ 0.7)
        if (r < 0.9f) return "A";      // 20% 확률 (0.7 ~ 0.9)

        return "S";                    // 10% 확률 (0.9 ~ 1.0)
    }
}