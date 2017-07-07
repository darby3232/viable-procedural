using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePosition : MonoBehaviour
{

	private int yPosition;
	private int xPosition;
	//zPosition?? possibly not good in this type of thing as this may not be good for an array

	public TilePosition (int xPosition, int yPosition)
	{
		this.yPosition = yPosition; 
		this.xPosition = xPosition; 
	}

	public int getXPosition ()
	{
		return xPosition; 
	}

	public int getYPosition ()
	{
		return yPosition; 
	}
}
