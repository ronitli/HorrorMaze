using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstriuctionController : MonoBehaviour
{
    public Button mainMenuButton;
    // Start is called before the first frame update
    void Start()
    {
        mainMenuButton.onClick.AddListener(OnMainMenuClick);
    }

    public void OnMainMenuClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy() {
        mainMenuButton.onClick.RemoveAllListeners();
    }
}
