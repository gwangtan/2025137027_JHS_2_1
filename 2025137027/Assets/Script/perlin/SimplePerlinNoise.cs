using UnityEngine;

public class SimplePerlinTerrain : MonoBehaviour
{
    [Header("Map Settings")]
    public int width = 30;
    public int depth = 30;
    public float scale = 0.1f;
    public float heightMultiplier = 8f;
    public int waterLevel = 3; // 이 높이 이하(<=)인 빈 공간에는 물이 찹니다.

    [Header("Prefabs")]
    public GameObject grassPrefab;
    public GameObject dirtPrefab;
    public GameObject waterPrefab;

    private SimplePerlinNoise simpleNoise;
    private int XOffset = 0;
    private int ZOffset = 0;

    void Start()
    {
        simpleNoise = GetComponent<SimplePerlinNoise>();

        // 매 실행마다 새로운 맵이 나오도록 Offset 랜덤화
        XOffset = Random.Range(-9999, 9999);
        ZOffset = Random.Range(-9999, 9999);

        Generate();
    }

    public void Generate()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                // Noise 함수를 이용해 해당 위치의 높이 계산
                float noise = simpleNoise.Noise((x + XOffset) * scale, (z + ZOffset) * scale);
                int height = Mathf.RoundToInt(noise * heightMultiplier);

                // 계산된 높이를 바탕으로 블록 및 물 생성
                CreateColumn(x, z, height);
            }
        }
    }

    void CreateColumn(int x, int z, int height)
    {
        // 1. 지형 블록 생성 (y가 0부터 height까지)
        for (int y = 0; y <= height; y++)
        {
            Vector3 position = new Vector3(x, y, z);
            GameObject prefabToSpawn;

            if (y == height)
            {
                // 제일 상단 타일은 Grass
                prefabToSpawn = grassPrefab;
            }
            else
            {
                // 기존(아래쪽) 맵은 Dirt
                prefabToSpawn = dirtPrefab;
            }

            Instantiate(prefabToSpawn, position, Quaternion.identity, transform);
        }

        // 2. 물 채우기 로직
        // 지형 높이(height)가 설정한 waterLevel보다 낮다면, 그 차이만큼 위로 물을 채웁니다.
        if (height < waterLevel)
        {
            for (int y = height + 1; y <= waterLevel; y++)
            {
                Vector3 waterPosition = new Vector3(x, y, z);
                Instantiate(waterPrefab, waterPosition, Quaternion.identity, transform);
            }
        }
    }
}