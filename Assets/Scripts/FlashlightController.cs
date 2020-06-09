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
            transform.forward -= (transform.forward - Camera.current.transform.forward) * 0.1f;
        }
    }

    void OnDrawGizmos()
    {

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            UnityEditor.SceneView.RepaintAll();
        }
#endif
    }
}
