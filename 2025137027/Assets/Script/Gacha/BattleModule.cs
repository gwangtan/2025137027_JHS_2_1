using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [Header("Settings")]
    public float playerDamage = 30f;
    public float enemyMaxHP = 300f;
    private float currentEnemyHP;

    [Header("UI References")]
    public Text battleLog;
    public Text hpText;
    public Image enemyImage;
    public Sprite[] enemySprites;

    // 다른 스크립트(모듈) 참조
    private CriticalModule critModule;
    private GachaModule gachaModule;

    void Awake()
    {
        // 모듈 초기화 (치명타 30% 설정)
        critModule = new CriticalModule(0.3f);
        gachaModule = new GachaModule();
    }

    void Start() { SpawnNewEnemy(); }

    public void OnAttackButton()
    {
        if (currentEnemyHP <= 0) return;

        // 치명타 모듈 사용
        bool isCrit = critModule.RollCrit();
        float damage = isCrit ? playerDamage * 2 : playerDamage;

        currentEnemyHP -= damage;
        UpdateUI(isCrit, damage);

        if (currentEnemyHP <= 0)
        {
            // 가챠 모듈 사용
            string item = gachaModule.GetDropGrade();
            battleLog.text += $"\n<color=red>처치!</color> 획득: {item} (다음 전설 확률: {gachaModule.currentLegendRate * 100:F1}%)";
            SpawnNewEnemy();
        }
    }

    void SpawnNewEnemy()
    {
        currentEnemyHP = enemyMaxHP;
        if (enemySprites.Length > 0)
            enemyImage.sprite = enemySprites[Random.Range(0, enemySprites.Length)];
        UpdateHPUI();
    }

    void UpdateUI(bool isCrit, float dmg)
    {
        battleLog.text = $"{(isCrit ? "<b>치명타!</b> " : "")}{dmg} 데미지!";
        UpdateHPUI();
    }

    void UpdateHPUI() => hpText.text = $"HP: {currentEnemyHP} / {enemyMaxHP}";
}