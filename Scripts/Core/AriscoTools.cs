using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arisco.Core;
using System.Linq;

public class AriscoTools : MonoBehaviour
{
    public static List<T> GetAllAgents<T>() where T : AComponent
    {
        return AriscoSystem.GetAComponents<T>().Cast<T>().ToList();
    }

    public static AAgent CreateAgent (World world, AAgent agent, Vector2 pos)
    {
        return CreateAgent(world, agent, new Vector3(pos.x, 0, pos.y));
    }

    public static AAgent CreateAgent (World world, AAgent agent, Vector3 pos)
	{
		agent.transform.position = pos;
		AAgent a = Instantiate (agent) as AAgent;
        a.transform.parent = world.transform;
        a.transform.position = pos;
		world.RegisterAgent (a);
		
		return a;
	}

    public static AAgent CreateAgent (World world, AAgent agent)
    {
        return CreateAgent(world, agent, Vector3.zero);
    }

    public static void ResignAgent(World world, AAgent agent){
        world.ResignAgent(agent);
    }

	public static Vector3 ToGrid(Vector3 pos){
		pos.x = Mathf.RoundToInt(pos.x);
		pos.y = Mathf.RoundToInt(pos.y);
		pos.z = Mathf.RoundToInt(pos.z);
		return pos;
	}
	

	public static float Rand(){
		return Random.value;
	}

    /// <summary>
    /// Determines whether the supplied object is a .NET numeric system type
    /// </summary>
    /// <param name="val">The object to test</param>
    /// <returns>true=Is numeric; false=Not numeric</returns>
    public static bool IsNumeric(ref object val)
    {
        if (val == null)
            return false;
        
        // Test for numeric type, returning true if match
        if 
            (
                val is double || val is float || val is int || val is long || val is decimal || 
                val is short || val is uint || val is ushort || val is ulong || val is byte || 
                val is sbyte
                )
                return true;
        
        // Not numeric
        return false;
    }

    /// <summary>
    /// Determines whether the supplied object is a .NET numeric system type
    /// </summary>
    /// <param name="val">The object to test</param>
    /// <returns>true=Is numeric; false=Not numeric</returns>
    public static bool IsInt(ref object val)
    {
        if (val == null)
            return false;
        
        // Test for numeric type, returning true if match
        if 
            (
                val is int || val is long || val is decimal || 
                val is short || val is uint || val is ushort || val is ulong || val is byte || 
                val is sbyte
                )
                return true;
        
        // Not numeric
        return false;
    }



	
}
