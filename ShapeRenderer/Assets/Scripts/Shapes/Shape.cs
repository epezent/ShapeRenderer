using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShapeRenderer))]
[ExecuteInEditMode]
[DisallowMultipleComponent]
public class Shape : MonoBehaviour {

    [Range(1,10)]
    public int five = 5;
    protected ShapeRenderer sr;

    private void Awake()
    {
        sr = GetComponent<ShapeRenderer>();
        sr.shape = this;
    }

    public virtual void Draw()
    {
        Debug.Log("Shape attached to GameObject " + name + " does not implement the Draw() function.");
    }

}
