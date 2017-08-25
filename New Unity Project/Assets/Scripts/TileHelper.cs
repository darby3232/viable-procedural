using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHelper : MonoBehaviour
{

	public static Color forestColor = new Color (0f, .30f, 0f);
	//new Color (.6f, 0f, .6f);
	public static Color grassColor = Color.green;
	//new Color (1f, 0f, 1f);
	public static Color waterColor = Color.cyan;
	//new Color (0f, 1f, 1f);


	public enum TileType
	{
		Forest,
		Water,
		Grass}
	;



}
