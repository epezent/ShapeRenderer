using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateShape : MonoBehaviour {

    ShapeRenderer sr;
    PolygonShape polygonShape;
    public float size;
    public float radius;

	// Use this for initialization
	void Start () {
        sr = GetComponent<ShapeRenderer>();
        polygonShape = GetComponent<PolygonShape>();
	}
	
	// Update is called once per frame
	void Update () {
        size = 200 + 50 * Mathf.Sin(2 * Mathf.PI * 0.25f * Time.time);
        radius = 25 + 24 * Mathf.Sin(2 * Mathf.PI * 0.25f * Time.time);
        polygonShape.parameterValue = size;
        polygonShape.cornerRadius = radius;
        
	}
}
