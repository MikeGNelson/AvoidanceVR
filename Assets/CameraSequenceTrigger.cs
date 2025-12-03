using UnityEngine;
using UnityEngine.Playables;

public class CameraSequenceTrigger : MonoBehaviour
{
    public PlayableDirector director;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed, playing timeline");
            director.Play();
        }
    }
}
