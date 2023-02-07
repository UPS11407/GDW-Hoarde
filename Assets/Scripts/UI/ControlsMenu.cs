using UnityEngine;


public class ControlsMenu : MonoBehaviour
{
    public PauseMenu pauseMenu;

    public void OpenMenu()
    {
        gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);

        pauseMenu.OpenMenu();
    }
}
