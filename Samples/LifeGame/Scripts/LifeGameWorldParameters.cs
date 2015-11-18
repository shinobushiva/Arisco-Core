using UnityEngine;
using System.Collections;

public class LifeGameWorldParameters : WorldParameters {

    [Range(0, 1)]
    public float rate = 0.5f;

    [Range(3, 100)]
    public int num = 20;

    public bool torus = false;

	// Use this for initialization
	void Start () {

	}

}
