using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingDirector : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 9.0f);
    }

    private void OnDestroy()
    {
        SceneController.instance.BackToMainMenu();
    }

}
