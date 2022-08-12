using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is separated because I didn't want the entirety of GridManager to run in edit mode
[ExecuteInEditMode]
public class DestroyColliderObjects : MonoBehaviour
{
    public bool destroy = false;

    private void ResetColliders(bool destroyColliders)
    {   
        if (destroyColliders == false)
            return;
        
        foreach (GameObject collider in GameObject.FindGameObjectsWithTag("OldCollider"))
        {
            DestroyImmediate(collider);
        }
        
        destroyColliders = false;
    }

    private void Update()
    {
        ResetColliders(destroy);
    }
}
