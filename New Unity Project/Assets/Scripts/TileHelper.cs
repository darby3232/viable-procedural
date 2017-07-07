using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHelper : MonoBehaviour
{

	public enum TileType
	{
		Rock,
		Water,
		Grass}

	;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public Color getGrassColor ()
	{
		return Color.green; 	
	}

	public Color getMountainColor ()
	{
		return Color.gray; 
	}

	public Color getMountainWater ()
	{
		return Color.cyan; 
	}

}
