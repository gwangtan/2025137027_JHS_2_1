using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 만약 텍스트가 TextMeshPro라면 using TMPro; 로 변경하세요.

public class TurnGameManager : MonoBehaviour
{
    public static TurnGameManager Instance;

    public enum Turn { Player1, Player2 }
    [Header("Game State")]
    public Turn currentTurn = Turn.Player1;
    public bool isBallMoving = false;
    public float stopThreshold = 0.4f;

    [Header("Players and Targets")]
    public Rigidbody player1Ball;
    public Rigidbody player2Ball;
    public List<Rigidbody> targetBalls;

    [Header("Scores")]
    public int p1Score = 0;
    public int p2Score = 0;
    public int winScore = 5;

    [Header("UI References")]
    public Text turnText;      // 현재 누구 턴인지 표시 (예: "1P 턴")
    public Text p1ScoreText;   // 1P 점수판 UI 연결 ◀ 분리됨!
    public Text p2ScoreText;   // 2P 점수판 UI 연결 ◀ 분리됨!
    public Text winText;       // 우승자 안내 텍스트

    private HashSet<GameObject> hitTargetsThisTurn = new HashSet<GameObject>();
    private bool hitOpponentThisTurn = false;
    private bool gameEnded = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
        if (winText != null) winText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (gameEnded) return;
        CheckBallsMovement();
    }

    void CheckBallsMovement()
    {
        bool anyBallMoving = false;

        // 플레이어 공 속도 체크
        if ((player1Ball != null && player1Ball.linearVelocity.magnitude > stopThreshold) ||
            (player2Ball != null && player2Ball.linearVelocity.magnitude > stopThreshold))
        {
            anyBallMoving = true;
        }

        // 타겟 공 속도 체크
        foreach (var target in targetBalls)
        {
            if (target != null && target.linearVelocity.magnitude > stopThreshold)
            {
                anyBallMoving = true;
                break;
            }
        }

        if (isBallMoving && !anyBallMoving)
        {
            isBallMoving = false;
            EvaluateTurnResult();
        }
        else if (!isBallMoving && anyBallMoving)
        {
            isBallMoving = true;
        }
    }

    public void OnBallCollision(GameObject roller, GameObject hitObject)
    {
        if (currentTurn == Turn.Player1 && roller != player1Ball.gameObject) return;
        if (currentTurn == Turn.Player2 && roller != player2Ball.gameObject) return;

        GameObject opponentBall = (currentTurn == Turn.Player1) ? player2Ball.gameObject : player1Ball.gameObject;
        if (hitObject == opponentBall)
        {
            hitOpponentThisTurn = true;
        }

        foreach (var target in targetBalls)
        {
            if (hitObject == target.gameObject)
            {
                hitTargetsThisTurn.Add(hitObject);
            }
        }
    }

    void EvaluateTurnResult()
    {
        if (hitOpponentThisTurn)
        {
            if (currentTurn == Turn.Player1) p1Score = Mathf.Max(0, p1Score - 1);
            else p2Score = Mathf.Max(0, p2Score - 1);
        }
        else if (hitTargetsThisTurn.Count == targetBalls.Count)
        {
            if (currentTurn == Turn.Player1) p1Score++;
            else p2Score++;
        }

        hitTargetsThisTurn.Clear();
        hitOpponentThisTurn = false;

        UpdateUI();

        if (p1Score >= winScore || p2Score >= winScore)
        {
            EndGame();
            return;
        }

        currentTurn = (currentTurn == Turn.Player1) ? Turn.Player2 : Turn.Player1;
        UpdateUI();
    }

    // UI 업데이트 로직 (두 명의 스코어가 항상 화면에 갱신됨)
    void UpdateUI()
    {
        if (turnText != null)
            turnText.text = $"TURN: {(currentTurn == Turn.Player1 ? "1P" : "2P")}";

        // 1P 스코어와 2P 스코어를 각각 독립된 텍스트 UI에 띄웁니다.
        if (p1ScoreText != null)
            p1ScoreText.text = $"1P Score: {p1Score}";

        if (p2ScoreText != null)
            p2ScoreText.text = $"2P Score: {p2Score}";
    }

    void EndGame()
    {
        gameEnded = true;
        if (winText != null)
        {
            winText.gameObject.SetActive(true);
            string winner = p1Score >= winScore ? "Player 1" : "Player 2";
            winText.text = $"{winner} WINS!";
        }
    }
}