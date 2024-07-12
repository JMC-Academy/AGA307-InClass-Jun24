using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public TMP_Text scoreText;
    public TMP_Text enemyCountText;

    public void UpdateScore(int _score)
    {
        scoreText.text = "Score: " + _score;
    }

    public void UpdateEnemyCount(int _count)
    {
        enemyCountText.text = "Enemy Count: " + _count.ToString();
        enemyCountText.color = _count >= 5 ? Color.red : Color.white;
    }

    public void EnemyScaler(float _scale)
    {
        _EM.ScaleEnemies(_scale);
    }
}
