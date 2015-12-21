using UnityEngine;
using System.Collections;

public class AAnimatorBehavior : ABehavior {

	private Animator animator;

	private Locomotion locomotion;

	protected Animator Avatar {
		get {
			if (animator == null) {
				animator = GetComponentInChildren<Animator> ();
			}
			return animator;
		}
	}

	protected Locomotion AvatarLocomotion {
		get {
			if (animator == null || locomotion == null) {
				locomotion = new Locomotion (Avatar);
			}
			return locomotion;
		}
	}
}
