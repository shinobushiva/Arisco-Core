using UnityEngine;
using System.Collections;

public class WalkerFactory : MonoBehaviour
{
	public virtual AAgent GetAWalker ()
	{
		return null;
	}
}
