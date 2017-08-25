using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*I'm going to
 * 
 * */


public class Tile  : MonoBehaviour
{
	private int xPosition;
	private int zPosition;
	public TileHelper.TileType type;

	//instead of a constructor, use a start method and multiple setters.... =__=
	//DON'T CONFUSE THIS: YOU DON'T USE CONSTRUCTORS IN MONOBEHAVIOR/UNITY
	public Tile (int xPosition, int zPosition, TileHelper.TileType type)
	{
		this.xPosition = xPosition; 
		this.zPosition = zPosition; 
		this.type = type; 

		createTileObject (); 
	}

	public void setXPosition (int xPosition)
	{
		this.xPosition = xPosition; 
	}

	public void setzPosition (int zPosition)
	{
		this.zPosition = zPosition; 
	}

	public void start ()
	{
		//cubeRepresentation = GameObject.CreatePrimitive (PrimitiveType.Cube);
	}


	//TODO
	//must be called finally
	public void createTileObject ()
	{
		/*
		if (type == TileHelper.TileType.Grass) {
			gameObject.GetComponent<Renderer> ().material.color = Color.green;
		} else if (type == TileHelper.TileType.Forest) {
			gameObject.GetComponent<Renderer> ().material.color = Color.gray; 
		} else if (type == TileHelper.TileType.Water) {
			gameObject.GetComponent<Renderer> ().material.color = Color.cyan;
		}*/
			
		gameObject.transform.position = new Vector3 (xPosition * 5f, zPosition * 5f, 0f); //5f is the tileSize

	}


	public GameObject getNorthernTile (GameObject[,] array)
	{
		if (!Procedural2DArray.isOutOfBounds (xPosition, zPosition + 1)) {
			return array [xPosition, zPosition + 1];
		} else {
			return null; 
		}
	}

	public GameObject getNorthEasternTile (GameObject[,] array)
	{
		if (!Procedural2DArray.isOutOfBounds (xPosition + 1, zPosition + 1)) {
			return array [xPosition + 1, zPosition + 1]; 
		} else {
			return null; 
		}
	}

	public GameObject getEasternTile (GameObject[,] array)
	{
		if (!Procedural2DArray.isOutOfBounds (xPosition + 1, zPosition)) {
			return array [xPosition + 1, zPosition]; 
		} else {
			return null; 
		}
	}

	public GameObject getSouthEasternTile (GameObject[,] array)
	{
		if (!Procedural2DArray.isOutOfBounds (xPosition + 1, zPosition - 1)) {			
			return array [xPosition + 1, zPosition - 1]; 
		} else {
			return null; 
		}
	}


	public GameObject getSouthernTile (GameObject[,] array)
	{
		if (!Procedural2DArray.isOutOfBounds (xPosition, zPosition - 1)) {
			return array [xPosition, zPosition - 1]; 
		} else {
			return null; 
		}
	}

	public GameObject getSouthWesternTile (GameObject[,] array)
	{
		if (!Procedural2DArray.isOutOfBounds (xPosition - 1, zPosition - 1)) {			
			return array [xPosition - 1, zPosition - 1]; 
		} else {
			return null; 
		}
	}

	public GameObject getWesternTile (GameObject[,] array)
	{
		if (!Procedural2DArray.isOutOfBounds (xPosition - 1, zPosition)) {
			return array [xPosition - 1, zPosition]; 
		} else {
			return null; 
		}
	}

	public GameObject getNorthWesternTile (GameObject[,] array)
	{
		if (!Procedural2DArray.isOutOfBounds (xPosition - 1, zPosition + 1)) {
			return array [xPosition - 1, zPosition + 1]; 
		} else {
			return null; 
		}
	}

	public void setTileType (TileHelper.TileType type)
	{		
		this.type = type;


		if (type == TileHelper.TileType.Grass) {
			gameObject.GetComponent<Renderer> ().material.color = TileHelper.grassColor;
		} else if (type == TileHelper.TileType.Forest) {
			gameObject.GetComponent<Renderer> ().material.color = TileHelper.forestColor;
		} else if (type == TileHelper.TileType.Water) {
			gameObject.GetComponent<Renderer> ().material.color = TileHelper.waterColor;
		}
	}

	public TileHelper.TileType getTileType ()
	{
		return type; 
	}


	/*private bool isOutOfBounds (int xPos, int yPos)
	{
		if (xPos < 0 || xPos >= Procedural2DArray.xWorldTileMax) {
			return true;
		}

		if (yPos < 0 || yPos >= Procedural2DArray.yWorldTileMax) {
			return true; 
		}

		return false; 
	}*/

}
