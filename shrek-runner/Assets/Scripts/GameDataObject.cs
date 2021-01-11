using UnityEngine;

[CreateAssetMenu(fileName = "New_GameDataObject", menuName = "Game Data", order = 51)]
public class GameDataObject : ScriptableObject
{
    [Header("Level objects")]
    public bool isGamePlayed = false;
    public bool isGameWin = false;
    public bool isGameLose = false;

    [Space(10)]
    public Vector3 HoleDistanceCheck = new Vector3(0.4f, 0, 0);

    [Space(10)]
    public float wallMass = 5f;


    [Header("Player parameters"), Space(10)]
    public float PlayerSpeed = 100f;
    public float PlayerSidesSpeed = 0.5f;

    [Space(10)]
    public Vector3 MinSize = new Vector3(1f, 1f, 1f);
    public Vector3 MaxSize = new Vector3(2f, 2f, 2f);

    [Space(10)]
    public float AngleMovingRotation = 12f;
    public float GiantFormRotationSpeed = 0.1f;
    public float NormalFormRotationSpeed = 0.1f;
    public float PreFallingForce = 5f;

    [Space(10)]
    public Vector3 PlaneBorder = new Vector3(12, 0, 300);

    [SerializeField, Space(10)]
    private float perSize = 0.1f;
    public float PerMass = 1f;

    public Vector3 PerSize { get => new Vector3(perSize, perSize, perSize); }

    public void StartGame()
    {
        isGamePlayed = true;
    }

    public void GameOver()
    {
        isGamePlayed = false;
        isGameLose = true;
    }

    public void GameWin()
    {
        isGamePlayed = false;
        isGameWin = true;
    }
}