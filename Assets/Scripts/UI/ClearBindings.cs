using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearBindings : MonoBehaviour
{
    public List<RebindButton> buttons;
    public PlayerControlsManager playerControlsManager;

    public void ClearBinds()
    {
        playerControlsManager.ClearBindings();

        foreach(var button in buttons)
        {
            button.DoButtonText();
        }
    }
}
