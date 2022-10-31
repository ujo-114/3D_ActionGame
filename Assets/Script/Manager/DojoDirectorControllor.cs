using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DojoDirectorControllor : MonoBehaviour
{
    public PlayableDirector pd;


    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            pd.Play();
            SceneController.instance.SetFlashPoint(this.transform.position);
            Destroy(this.gameObject, 2.2f);
        }
    }
}
