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
public class GameManager : MonoBehaviour
{
    public GameState gameState;
    public Difficulty difficulty;

    public int score;
    private int scoreMultiplier = 1;

    void Start()
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

}
