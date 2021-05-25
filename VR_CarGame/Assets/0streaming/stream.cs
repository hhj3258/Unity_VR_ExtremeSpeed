using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class stream : MonoBehaviour
{
    public string streamTargetScene;


    IEnumerator StreamingTargetScene()
    {
        var targetScene = SceneManager.GetSceneByName(streamTargetScene);

        if (!targetScene.isLoaded)
        {
            var op = SceneManager.LoadSceneAsync(streamTargetScene, LoadSceneMode.Additive);

            while (!op.isDone)
            {
                yield return null;
            }
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("chk1");
        if (other.CompareTag("colCar"))
        {
            Debug.Log("chk2");
            var dir = Vector3.Angle(transform.forward, other.transform.position - transform.position);

            if (dir > 90f)
            {
                Debug.Log("chk3");
                StartCoroutine(StreamingTargetScene());
            }
        }
    }

}
