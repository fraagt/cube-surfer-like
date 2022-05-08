using UnityEngine;
using UnityEngine.UI;

public class GameScreenView : MonoBehaviour
{
    [SerializeField] private Text _scoreText;

    public void SetScore(object score)
    {
        _scoreText.text = score.ToString();
    }
}
