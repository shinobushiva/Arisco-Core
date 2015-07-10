using UnityEngine;
using System.Collections;

public class LifeGameWorldParameters : WorldParameters {

    [Range(0, 1)]
    public float rate = 0.5f;

    [Range(3, 100)]
    public int num = 20;

    public bool torus;

	// Use this for initialization
	void Start () {
		AriscoGUI.Instance.FloatField ("rate", 0.5f, 0, 1f);
		AriscoGUI.Instance.IntField ("num", 20, 3, 100);
		AriscoGUI.Instance.BoolField("torus", false);
	}

}
