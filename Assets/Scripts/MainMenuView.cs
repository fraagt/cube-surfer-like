using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _exitButton;

    public Button StartGameButton => _startGameButton;
    public Button ExitButton => _exitButton;
}
