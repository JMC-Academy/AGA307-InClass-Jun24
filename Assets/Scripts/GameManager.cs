using UnityEngine;

public enum GameState
{
    Ready,
    Playing,
    Paused,
    GameOver,
}
public enum Difficulty
{
    Easy, Medium, Hard
}
public class GameManager : Singleton<GameManager>
{
    public GameState gameState;
    public Difficulty difficulty;
    public PauseController pauseController;

    public int score;
    private int scoreMultiplier = 1;

    public void Start()
    {
        switch(difficulty)
        {
            case Difficulty.Easy:
                scoreMultiplier = 1;
                break;
            case Difficulty.Medium:
                scoreMultiplier = 2; 
                break;
            case Difficulty.Hard:
                scoreMultiplier = 3;
                break;
        }
    }

    public void AddScore(int _score)
    {
        score += _score * scoreMultiplier;
        _UI.UpdateScore(score);
        _SAVE.SetScore(score);
    }

    #region Events
    private void OnEnemyHit(GameObject _go)
    {
        AddScore(10);
    }

    private void OnEnemyDie(GameObject _go)
    {
        AddScore(100);
    }

    private void OnEnable()
    {
        GameEvents.OnEnemyHit += OnEnemyHit;
        GameEvents.OnEnemyDie += OnEnemyDie;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyHit -= OnEnemyHit;
        GameEvents.OnEnemyDie -= OnEnemyDie;
    }
    #endregion
}
