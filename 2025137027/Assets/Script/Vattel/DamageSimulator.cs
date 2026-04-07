using TMPro;
using UnityEngine;


public class DamageSimulator : MonoBehaviour
{
    public TextMeshProUGUI statusDisplay;
    public TextMeshProUGUI logDisplay;
    public TextMeshProUGUI resultDisplay;
    public TextMeshProUGUI rangeDisplay;

    private int level = 1;
    private float totalDamage = 0, baseDamage = 20f;
    private int attackCount = 0;
    private int yakAtk = 0;
    private int missAtk = 0;
    private int critCount = 0;
    private float maxDamage = 0;

    private string weaponName;
    private float stdDevMult, critRate, critMult;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetWeapon(0); // 시작 시 단검 장착
    }

    private void ResetData()
    {
        totalDamage = 0;
        attackCount = 0;
        level = 1;
        baseDamage = 20f;

        yakAtk = 0;
        missAtk = 0;
        critCount = 0;
        maxDamage = 0;
    }



    private void UpdateUI()
    {
        statusDisplay.text = string.Format("Level: {0} / 무기: {1}\n기본 데미지: {2} / 치명타: {3}% (x{4})",
            level, weaponName, baseDamage, critRate * 100, critMult);

        rangeDisplay.text = string.Format("예상 일반 데미지 범위 : [{0:F1} ~ {1:F1}]",
            baseDamage - (3 * baseDamage * stdDevMult),
            baseDamage + (3 * baseDamage * stdDevMult));

        float dpa = attackCount > 0 ? totalDamage / attackCount : 0;
        resultDisplay.text = string.Format("누적 데미지: {0:F1}\n공격 횟수: {1}\n평균 DPA: {2:F2}",
            totalDamage, attackCount, dpa);
    }

    private float GetNormalStdDevDamage(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        return mean + stdDev * randStdNormal;
    }

    public void LevelUp()
    {
        totalDamage = 0;
        attackCount = 0;
        level++;
        baseDamage = level * 20f;
        logDisplay.text = string.Format("레벨업! 현재 레벨: {0}", level);
        UpdateUI();
    }

    public void OnAttack()
    {
        float sd = baseDamage * stdDevMult;
        float normalDamage = GetNormalStdDevDamage(baseDamage, sd);

        bool isMiss = normalDamage < (baseDamage - 2f * sd);
        bool isYak = normalDamage > (baseDamage + 2f * sd);

        float finalDamage = 0f;

        if (isMiss)
        {
            missAtk++;
        }
        else
        {
            finalDamage = normalDamage;

            if (isYak)
            {
                yakAtk++;
                finalDamage *= 2f;
            }

            bool isCrit = Random.value < critRate;
            if (isCrit)
            {
                critCount++;
                finalDamage *= critMult;
            }
        }

        // 통계
        attackCount++;
        totalDamage += finalDamage;
        if (finalDamage > maxDamage) maxDamage = finalDamage;

        // 로그
        string log = "";

        if (isMiss)
            log = "<color=gray>[미스]</color>";
        else
        {
            if (isYak) log += "<color=yellow>[약점]</color> ";
            if (finalDamage > normalDamage) log += "<color=red>[치명타]</color> ";
            log += $"데미지: {finalDamage:F1}";
        }

        logDisplay.text = log;
        UpdateUI();
    }


    public void SetWeapon(int id)
    {
        ResetData();
        if (id == 0)
        {
            SetStats("단검", 0.1f, 0.4f, 1.5f);
        }
        else if (id == 1)
        {
            SetStats("장검", 0.2f, 0.3f, 2.0f);
        }
        else
        {
            SetStats("도끼", 0.3f, 0.2f, 3.0f);
        }


        logDisplay.text = string.Format("{0} 장착!", weaponName);
        UpdateUI();

    }

    public void OnAttack1000()
    {
        // 초기화
        yakAtk = 0;
        missAtk = 0;
        critCount = 0;
        maxDamage = 0;

        totalDamage = 0;
        attackCount = 0;

        for (int i = 0; i < 1000; i++)
        {
            float sd = baseDamage * stdDevMult;
            float normalDamage = GetNormalStdDevDamage(baseDamage, sd);

            bool isMiss = normalDamage < (baseDamage - 2f * sd);
            bool isYak = normalDamage > (baseDamage + 2f * sd);

            float finalDamage = 0f;

            if (isMiss)
            {
                missAtk++;
            }
            else
            {
                finalDamage = normalDamage;

                if (isYak)
                {
                    yakAtk++;
                    finalDamage *= 2f;
                }

                bool isCrit = Random.value < critRate;
                if (isCrit)
                {
                    critCount++;
                    finalDamage *= critMult;
                }
            }

            attackCount++;
            totalDamage += finalDamage;
            if (finalDamage > maxDamage) maxDamage = finalDamage;
        }

        float dpa = totalDamage / attackCount;

        logDisplay.text =
            $"[1000회 결과]\n" +
            $"약점 공격: {yakAtk}\n" +
            $"명중 실패: {missAtk}\n" +
            $"크리티컬: {critCount}\n" +
            $"최대 데미지: {maxDamage:F1}\n" +
            $"평균 DPA: {dpa:F2}";

        UpdateUI();
    }

    private void SetStats(string _name, float _stdDev, float _critRate, float _critMult)
    {
        weaponName = _name;
        stdDevMult = _stdDev;
        critRate = _critRate;
        critMult = _critMult;
    }


    void CheckCrit()
    {
        int critCount;
        int failCount;
        int allCritCount;
        float maxCritDam;

    }
}
