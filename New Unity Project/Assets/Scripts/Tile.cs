using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

	private int xPosition;
	private int yPosition;
	private TileHelper.TileType type;

	public Tile (int xPosition, int yPosition, TileHelper.TileType type)
	{
		this.xPosition = xPosition; 
		this.yPosition = yPosition; 
		this.type = type; 
	}


	public Tile getNorthernTile ()
	{
		return Procedural2DArray.worldArray [xPosition + 1, yPosition]; 
	}

	public Tile getNorthEasternTile ()
	{
		return Procedural2DArray.worldArray [xPosition + 1, yPosition + 1]; 
	}

	public Tile getEasternTile ()
	{
		return Procedural2DArray.worldArray [xPosition, yPosition + 1]; 
	}

	public Tile getSouthEasternTile ()
	{
		return Procedural2DArray.worldArray [xPosition - 1, yPosition + 1]; 
	}

	public Tile getSouthernTile ()
	{
		return Procedural2DArray.worldArray [xPosition - 1, yPosition]; 
	}

	public Tile getSouthWesternTile ()
	{
		return Procedural2DArray.worldArray [xPosition - 1, yPosition - 1]; 
	}

	public Tile getWesternTile ()
	{
		return Procedural2DArray.worldArray [xPosition, yPosition - 1]; 
	}

	public Tile getNorthWesternTile ()
	{
		return Procedural2DArray.worldArray [xPosition + 1, yPosition - 1]; 
	}

	public void setTileType (TileHelper.TileType type)
	{
		this.type = type;
	}

	public TileHelper.TileType getTileType ()
	{
		return type; 
	}

}
