using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*High Level concept*
 * 
 * -Create a 2D array of X by X comprised of differenet objects
 * -The composition of the array will be partially random, having adjacent variables more dependent on each other
 * 			-concept: a random variable for each of the possible terrains and take the highest as the terrain
 * -each terrain type will be connected and will have random variables assigned on creation to determined the varibles of the terrain
 * 
 * 
 * TILE SYSTEM THAT WILL USE A MATHMATICAL 2D REPRESENTATION
 * 
 * 
 * 
 * TODO 7/2
 * - finish the tile type enum problem  |X|
 * - finish the tile type determiner  | |
 * - finish the tile creator in the tile constructor  | |
 * 
 * */

public class Procedural2DArray : MonoBehaviour
{
	public float tileSize = 5;
	public int xWorldTileMax = 50;
	public int yWorldTileMax = 50;

	//two data structures that I thought about using, going with the array at first for what I think will be efficiency... eventually maybe want to make the size of the area dependent
	public List<List<Tile>> worldTiles = new List<List<Tile>> ();
	public Tile[,] worldArray;
	//on start can create a method to determine the (random) size, but for now just using static 100s


	// Use this for initializationf
	void Start ()
	{
		worldArray = new Tile[xWorldTileMax, yWorldTileMax];
		createWorld ();
		
	}

	//possibly make this a coroutine to show the process and be able to wait some milliseconds before creating each block?
	void createWorld ()
	{
		//1. form tiles one by one is the first option, but I'd rather use recursion as I think this might be the standard way to do it 
		/*
		for (int i = 0; i < worldArray.Length; i++) {
			for (int j = 0; j < worldArray [i].Length; j++) {

				//create a tile at this position
				createTile (i, j); 

			}
		}*/


		//2. create the tiles recursively
		//original position should be the center of the world!
		createTilesRecursively (xWorldTileMax / 2, yWorldTileMax / 2);



	}
	/*Ideas_1
	 * set up creating a list so that we create the surrounding tiles
	 * 
	 * Ideas_2
	 * just do this recursively, and as we go through the same tile again, reevaluate the tile. 
	 * 
	 * 
	 * Final_Pass
	 * make a final pass through all the tiles to see if there is anything completely abnormal
	 * */


	//spreads out from the original position
	private void createTilesRecursively (int xPos, int yPos)
	{
		//BASE CASE that the position is out of bounds
		if (isOutOfBounds (xPos, yPos)) {
			return; 
		}
		//create the tile - in the tile 
		Tile tile = new Tile (xPos, yPos, TileHelper.TileType.Grass); 

		//DETERMINE THE TILE TYPE now that we have access to the methods of the tile class
		determineInitialTileType (tile);

		//add the tile to the world array so I can know the space types of each block in the future and create the world
		worldArray [xPos] [yPos] = tile; 

		//first create the northern tile
		createTilesRecursively (xPos + 1, yPos); 

		//second create the north-eastern tile
		createTilesRecursively (xPos + 1, yPos + 1); 

		//third create the eastern tile
		createTilesRecursively (xPos, yPos + 1);

		//fourth create the south-eastern tile
		createTilesRecursively (xPos - 1, yPos + 1);

		//fifth create the southern tile
		createTilesRecursively (xPos - 1, yPos); 

		//sixth create the south-western tile
		createTilesRecursively (xPos - 1, yPos - 1);

		//seventh create the western tile
		createTilesRecursively (xPos - 1, yPos);
		 
		//eighth creat the north-western tile
		createTilesRecursively (xPos - 1, yPos + 1); 

	}


	private TileHelper.TileType determineInitialTileType (Tile tile)
	{
		//so check each of the surrounding types to try and determine the tile type
		List<TileHelper.TileType> surroundingTypes = new List<TileHelper.TileType> (); 
		//add each surrounding type to a list
		surroundingTypes.Add (tile.getNorthernTile.getTileType ());
		surroundingTypes.Add (tile.getNorthEasternTile.getTileType ());
		surroundingTypes.Add (tile.getEasternTile.getTileType ());
		surroundingTypes.Add (tile.getSouthEasternTile.getTileType ());
		surroundingTypes.Add (tile.getSouthernTile.getTileType ());
		surroundingTypes.Add (tile.getSouthWesternTile.getTileType ());
		surroundingTypes.Add (tile.getWesternTile.getTileType ());
		surroundingTypes.Add (tile.getNorthWesternTile.getTileType ());

		foreach (TileHelper.TileType type in surroundingTypes) {
			
		}
	}

	private bool isOutOfBounds (int xPos, int yPos)
	{
		if (xPos < 0 || xPos >= xWorldTileMax) {
			return true;
		}

		if (yPos < 0 || yPos >= yWorldTileMax) {
			return true; 
		}

		return false; 
	}

}
