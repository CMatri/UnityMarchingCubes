using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class FlashlightController : MonoBehaviour
{
    void Update()
    {
        if (Camera.current != null)
        {
            transform.position = Camera.current.transform.position;
            transform.forward -= (transform.forward - Camera.current.transform.forward) * 0.2f;
        }
    }

    void OnDrawGizmos()
    {

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            EditorApplication.QueuePlayerLoopUpdate();
            SceneView.RepaintAll();
        }
#endif
    }
}
