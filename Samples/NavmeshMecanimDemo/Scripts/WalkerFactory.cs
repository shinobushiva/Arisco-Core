using UnityEngine;
using System.Collections;

public class WalkerFactory : MonoBehaviour
{
	public virtual AAgent GetAWalker ()
	{
		return null;
	}

	public virtual AAgent GetAWalker (int n)
	{
		return null;
	}

	public virtual bool HasAWalker(int n){
		return false;
	}
}


