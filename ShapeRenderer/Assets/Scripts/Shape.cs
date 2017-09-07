using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 09/2017

[ExecuteInEditMode]
[DisallowMultipleComponent]
public class Shape : MonoBehaviour {

    protected ShapeRenderer sr;

    private void Awake()
    {
        sr = GetComponent<ShapeRenderer>();
        if (sr == null)
        {
            sr = gameObject.AddComponent<ShapeRenderer>() as ShapeRenderer;
            #if UNITY_EDITOR
            for (int i = 0; i < 4; ++i)
                UnityEditorInternal.ComponentUtility.MoveComponentDown(this);            
            #endif
        }
        sr.shape = this;
    }

    private void Update()
    {
        if (sr == null)
        {
            sr = gameObject.AddComponent<ShapeRenderer>() as ShapeRenderer;
            #if UNITY_EDITOR
            for (int i = 0; i < 4; ++i)
                UnityEditorInternal.ComponentUtility.MoveComponentDown(this);            
            #endif
            sr.shape = this;
        }
    }

    public virtual void Draw()
    {
        Debug.Log("Shape attached to GameObject " + name + " does not implement the Draw() function.");
    }


}
