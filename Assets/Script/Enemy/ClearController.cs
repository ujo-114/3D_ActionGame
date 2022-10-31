using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearController : MonoBehaviour
{
    public StateManager sm;
    private void OnDisable()
    {
        if (GameMnager.instance.am.sm.isDie == false && sm.isDie == true)
        {
            SceneController.instance.GoToEnding();
        }
    }
}
