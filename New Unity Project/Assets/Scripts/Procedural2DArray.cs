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
 * TODO 7/11
 * - implement the tile creation process in the tile constuctor
 * 
 * */

public class Procedural2DArray : MonoBehaviour
{
	public float tileSize = 4;
	public int enteredXTileMax = 5;
	public int enteredZTileMax = 5;

	public int lakeHeightIterations = 3;
	public int smoothTilesIterations = 2;


	public static int xWorldTileMax = 20;
	public static int zWorldTileMax = 20;

	private int debugTimesRun = 0;
	public System.TimeSpan totalCreationTime;
	public System.TimeSpan totalTime;


	//two data structures that I thought about using, going with the array at first for what I think will be efficiency... eventually maybe want to make the size of the area dependent
	//public List<List<GameObject>> worldTiles = new List<List<GameObject>> ();
	public static GameObject[,] worldArray;
	//on start can create a method to determine the (random) size, but for now just using static 100s


	// Use this for initialization
	void Start ()
	{
		xWorldTileMax = enteredXTileMax; 
		zWorldTileMax = enteredZTileMax; 
		

		worldArray = new GameObject[xWorldTileMax, zWorldTileMax];
		createWorld ();
		
	}

	//possibly make this a coroutine to show the process and be able to wait some milliseconds before creating each block?
	void createWorld ()
	{

		//2. create the tiles recursively
		//original position should be the center of the world!

		Debug.Log ("Got Here");

		//createTilesRecursively (xWorldTileMax / 2, zWorldTileMax / 2);
		createTilesIteratively (); 
		smoothTiles (smoothTilesIterations); 
		setRandomTileHeight (); 
		setSpecialLakeHeight (lakeHeightIterations); 



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

	private void setSpecialLakeHeight (int iterations)
	{
		for (int i = 0; i < iterations; i++) {
			//iterate through the world array, checking if the tile is water, if so, make all surrounding tiles the same
			for (int x = 0; x < xWorldTileMax; x++) {
				for (int z = 0; z < zWorldTileMax; z++) {

					GameObject currentTile = worldArray [x, z];

					if (currentTile.GetComponent<Tile> ().getTileType () == TileHelper.TileType.Water) {
						Debug.Log ("Made it in the if"); 
						//so check each of the surrounding types to try and determine the tile type
						Vector3 currentTileScale = currentTile.transform.localScale;
					

						if (zWorldTileMax > z + 1 /*&& worldArray [x, z + 1] != null*/) {
							worldArray [x, z + 1].transform.localScale = currentTileScale; 
						}
						if (zWorldTileMax > z + 1 && xWorldTileMax > x + 1) {
							worldArray [x + 1, z + 1].transform.localScale = currentTileScale; 
						}
						if (xWorldTileMax > x + 1) {
							worldArray [x + 1, z].transform.localScale = currentTileScale; 
						}
						if (0 < z - 1 && xWorldTileMax > x + 1) {
							worldArray [x + 1, z - 1].transform.localScale = currentTileScale; 
						}
						if (0 < z - 1 /*&& worldArray [x, z + 1] != null*/) {
							worldArray [x, z - 1].transform.localScale = currentTileScale; 
						}
						if (0 < z - 1 && 0 < x - 1) {
							worldArray [x - 1, z - 1].transform.localScale = currentTileScale; 
						}
						if (0 < x - 1) {
							worldArray [x - 1, z].transform.localScale = currentTileScale; 
						}
						if (zWorldTileMax > z + 1 && 0 < x - 1) {
							worldArray [x - 1, z + 1].transform.localScale = currentTileScale; 
						}

					}
					Debug.Log ("---"); 
				}
			}
		}

	}

	private void setRandomTileHeight ()
	{

		//iterate through the world array, adding the tiles as I go
		for (int x = 0; x < xWorldTileMax; x++) {
			for (int z = 0; z < zWorldTileMax; z++) {

				//so check each of the surrounding types to try and determine the tile type
				//GameObject[] surroundTileObjects = new GameObject[8]; 
				//okay get the tile
				GameObject currentTile = worldArray [x, z];

				float randomHeight = Random.Range (1.0f, 10.0f);

				Vector3 localScaleObj = new Vector3 (tileSize, randomHeight, tileSize); 

				currentTile.transform.localScale = localScaleObj; 
			}
		}
	}

	private void smoothTiles (int iterations)
	{
		//number of smoothing iterations
		for (int i = 0; i < iterations; i++) {
			
			//iterate through the world array, adding the tiles as I go
			for (int x = 0; x < xWorldTileMax; x++) {
				for (int z = 0; z < zWorldTileMax; z++) {	

					//so check each of the surrounding types to try and determine the tile type
					GameObject[] surroundTileObjects = new GameObject[8]; 
					//okay get the tile
					GameObject currentTile = worldArray [x, z];
					Tile tile = currentTile.GetComponent<Tile> (); 
					//add each surrounding type to a list
					surroundTileObjects [0] = tile.getNorthernTile (worldArray); 
					surroundTileObjects [1] = tile.getNorthEasternTile (worldArray); 
					surroundTileObjects [2] = tile.getEasternTile (worldArray); 
					surroundTileObjects [3] = tile.getSouthEasternTile (worldArray); 
					surroundTileObjects [4] = tile.getSouthernTile (worldArray); 
					surroundTileObjects [5] = tile.getSouthWesternTile (worldArray); 
					surroundTileObjects [6] = tile.getWesternTile (worldArray); 
					surroundTileObjects [7] = tile.getNorthWesternTile (worldArray); 


					TileHelper.TileType[] surroundingTypes = new TileHelper.TileType[8]; 

					if (zWorldTileMax > z + 1 /*&& worldArray [x, z + 1] != null*/) {
						surroundingTypes [0] = worldArray [x, z + 1].GetComponent<Tile> ().getTileType (); 
					}
					if (zWorldTileMax > z + 1 && xWorldTileMax > x + 1) {
						surroundingTypes [1] = worldArray [x + 1, z + 1].GetComponent<Tile> ().getTileType ();
					}
					if (xWorldTileMax > x + 1) {
						surroundingTypes [2] = worldArray [x + 1, z].GetComponent<Tile> ().getTileType ();
					}
					if (0 < z - 1 && xWorldTileMax > x + 1) {
						surroundingTypes [3] = worldArray [x + 1, z - 1].GetComponent<Tile> ().getTileType ();
					}
					if (0 < z - 1 /*&& worldArray [x, z + 1] != null*/) {
						surroundingTypes [4] = worldArray [x, z - 1].GetComponent<Tile> ().getTileType (); 
					}
					if (0 < z - 1 && 0 < x - 1) {
						surroundingTypes [5] = worldArray [x - 1, z - 1].GetComponent<Tile> ().getTileType (); 
					}
					if (0 < x - 1) {
						surroundingTypes [6] = worldArray [x - 1, z].GetComponent<Tile> ().getTileType ();
					}
					if (zWorldTileMax > z + 1 && 0 < x - 1) {
						surroundingTypes [7] = worldArray [x - 1, z + 1].GetComponent<Tile> ().getTileType ();
					}

					//count each type of tile types around
					float grassCount = 0; 
					float forestCount = 0; 
					float waterCount = 0; 

					for (int j = 0; j < 8; j++) {
						if (surroundingTypes [j] == TileHelper.TileType.Grass) {
							grassCount++;
						} else if (surroundingTypes [j] == TileHelper.TileType.Forest) {
							forestCount++;
						} else {
							waterCount++; 
						}
					}

					//Now determine the type to return, the one with the highest value
					if (grassCount > 6) {
						tile.setTileType (TileHelper.TileType.Grass); 
					} else if (forestCount > 6) {
						tile.setTileType (TileHelper.TileType.Forest); 
					} else if (waterCount > 6) {
						tile.setTileType (TileHelper.TileType.Water); 
					}
				}
			}
		
		}
	}



	private void createTilesIteratively ()
	{
		//iterate through the world array, adding the tiles as I go
		for (int x = 0; x < xWorldTileMax / 2; x++) {
			for (int z = 0; z < zWorldTileMax / 2; z++) {

				GameObject newTile = GameObject.CreatePrimitive (PrimitiveType.Cube); 
				newTile.AddComponent<Tile> (); 
				newTile.transform.localScale = new Vector3 (tileSize, 1, tileSize); 
				newTile.transform.position = new Vector3 (tileSize * x, 0, tileSize * z); 

				//create the tile - in the tile 
				//Tile tile = new Tile (xPos, zPos, TileHelper.TileType.Grass); 

				//MUST CALL THIS AFTER WE CREATE THE TILE TO GET THE TILE Type
				TileHelper.TileType initialType = determineInitialTileType (newTile.GetComponent<Tile> ());
				newTile.GetComponent<Tile> ().setTileType (initialType);

				//add the tile to the world array so I can know the space types of each block in the future and create the world
				worldArray [x, z] = newTile; 
			}
		}

		for (int x = 0; x < xWorldTileMax / 2; x++) {
			for (int z = zWorldTileMax / 2; z < zWorldTileMax; z++) {

				GameObject newTile = GameObject.CreatePrimitive (PrimitiveType.Cube); 
				newTile.AddComponent<Tile> (); 
				newTile.transform.localScale = new Vector3 (tileSize, 1, tileSize); 
				newTile.transform.position = new Vector3 (tileSize * x, 0, tileSize * z); 

				//MUST CALL THIS AFTER WE CREATE THE TILE TO GET THE TILE Type
				TileHelper.TileType initialType = determineInitialTileType (newTile.GetComponent<Tile> ());
				newTile.GetComponent<Tile> ().setTileType (initialType);

				//add the tile to the world array so I can know the space types of each block in the future and create the world
				worldArray [x, z] = newTile; 
			}
		}

		for (int x = xWorldTileMax / 2; x < xWorldTileMax; x++) {
			for (int z = 0; z < zWorldTileMax / 2; z++) {

				GameObject newTile = GameObject.CreatePrimitive (PrimitiveType.Cube); 
				newTile.AddComponent<Tile> (); 
				newTile.transform.localScale = new Vector3 (tileSize, 1, tileSize); 
				newTile.transform.position = new Vector3 (tileSize * x, 0, tileSize * z); 

				//create the tile - in the tile 
				//Tile tile = new Tile (xPos, zPos, TileHelper.TileType.Grass); 

				//MUST CALL THIS AFTER WE CREATE THE TILE TO GET THE TILE Type
				TileHelper.TileType initialType = determineInitialTileType (newTile.GetComponent<Tile> ());
				newTile.GetComponent<Tile> ().setTileType (initialType);

				//add the tile to the world array so I can know the space types of each block in the future and create the world
				worldArray [x, z] = newTile; 
			}
		}

		for (int x = xWorldTileMax / 2; x < xWorldTileMax; x++) {
			for (int z = zWorldTileMax / 2; z < zWorldTileMax; z++) {

				GameObject newTile = GameObject.CreatePrimitive (PrimitiveType.Cube); 
				newTile.AddComponent<Tile> (); 
				newTile.transform.localScale = new Vector3 (tileSize, 1, tileSize); 
				newTile.transform.position = new Vector3 (tileSize * x, 0, tileSize * z); 

				//MUST CALL THIS AFTER WE CREATE THE TILE TO GET THE TILE Type
				TileHelper.TileType initialType = determineInitialTileType (newTile.GetComponent<Tile> ());
				newTile.GetComponent<Tile> ().setTileType (initialType);

				//add the tile to the world array so I can know the space types of each block in the future and create the world
				worldArray [x, z] = newTile; 
			}
		}


	}

	//spreads out from the original position
	private void createTilesRecursively (int xPos, int zPos)
	{
		System.DateTime startTime = System.DateTime.Now;

		Debug.Log ("Times Through: " + debugTimesRun);	
		int currIteration = debugTimesRun;
		debugTimesRun++;

		GameObject newTile = GameObject.CreatePrimitive (PrimitiveType.Cube); 
		newTile.AddComponent<Tile> (); 
		newTile.transform.localScale = new Vector3 (tileSize, 1, tileSize); 
		newTile.transform.position = new Vector3 (tileSize * xPos, 0, tileSize * zPos); 

		//create the tile - in the tile 
		//Tile tile = new Tile (xPos, zPos, TileHelper.TileType.Grass); 

		//MUST CALL THIS AFTER WE CREATE THE TILE TO GET THE TILE Type
		TileHelper.TileType initialType = determineInitialTileType (newTile.GetComponent<Tile> ());
		newTile.GetComponent<Tile> ().setTileType (initialType);

		//add the tile to the world array so I can know the space types of each block in the future and create the world
		worldArray [xPos, zPos] = newTile; 

		System.DateTime endOfCreation = System.DateTime.Now;

		Debug.Log ("Creation Period: " + (endOfCreation - startTime)); 
		totalCreationTime += (endOfCreation - startTime); 

		//first create the northern tile
		if (slotAvailable (xPos, zPos + 1)) {
			createTilesRecursively (xPos, zPos + 1);
		}

		//second create the north-eastern tile
		//createTilesRecursively (xPos + 1, zPos + 1); 

		//third create the eastern tile
		if (slotAvailable (xPos + 1, zPos)) {			
			createTilesRecursively (xPos + 1, zPos);
		}

		//fourth create the south-eastern tile
		//createTilesRecursively (xPos + 1, zPos + 1);

		//fifth create the southern tile
		if (slotAvailable (xPos, zPos - 1)) {			
			createTilesRecursively (xPos, zPos - 1); 
		}

		//sixth create the south-western tile
		//createTilesRecursively (xPos - 1, zPos - 1);

		//seventh create the western tile
		if (slotAvailable (xPos - 1, zPos)) {			
			createTilesRecursively (xPos - 1, zPos);
		}
		//eighth creat the north-western tile
		//createTilesRecursively (xPos - 1, zPos + 1); 

		System.DateTime endTime = System.DateTime.Now;
		 
		Debug.Log ("Overall Time:" + (endTime - startTime) + " for run " + currIteration); 
		totalTime += (endTime - startTime); 

	}

	private bool slotAvailable (int xPos, int zPos)
	{
		//BASE CASE that the position is out of bounds
		if (isOutOfBounds (xPos, zPos) || tileAlreadyThere (xPos, zPos)) {
			return false; 
		}
		return true; 
	}


	private TileHelper.TileType determineInitialTileType (Tile tile)
	{

		//so check each of the surrounding types to try and determine the tile type
		GameObject[] surroundTileObjects = new GameObject[8]; 
		//add each surrounding type to a list
		surroundTileObjects [0] = tile.getNorthernTile (worldArray); 
		surroundTileObjects [1] = tile.getNorthEasternTile (worldArray); 
		surroundTileObjects [2] = tile.getEasternTile (worldArray); 
		surroundTileObjects [3] = tile.getSouthEasternTile (worldArray); 
		surroundTileObjects [4] = tile.getSouthernTile (worldArray); 
		surroundTileObjects [5] = tile.getSouthWesternTile (worldArray); 
		surroundTileObjects [6] = tile.getWesternTile (worldArray); 
		surroundTileObjects [7] = tile.getNorthWesternTile (worldArray); 

		//count each type of tile types around
		float grassCount = 0; 
		float forestCount = 0; 
		float waterCount = 0; 
		foreach (GameObject tileObject in surroundTileObjects) {
			float randomModifier = Random.Range (0.3f, 0.7f);

			if (tileObject != null) {
				TileHelper.TileType type = tileObject.GetComponent<Tile> ().getTileType (); 

				//if there is a type
				if (type == TileHelper.TileType.Grass) {
					grassCount += randomModifier;//.5f;//randomModifier; 
				} else if (type == TileHelper.TileType.Forest) {
					forestCount += randomModifier;//.5f;//randomModifier;
				} else if (type == TileHelper.TileType.Water) {
					waterCount += randomModifier;//.5f;//randomModifier;
				}		
			} else {
				//else type is probably null and we must add to a random type
				int randomTileType = Random.Range (1, 4);
				if (randomTileType == 1) {
					grassCount += randomModifier; 
				} else if (randomTileType == 2) {
					forestCount += randomModifier; 
				} else if (randomTileType == 3) {
					waterCount += randomModifier; 
				}
			}
		}

		//Now determine the type to return, the one with the highest value
		if (grassCount > forestCount && grassCount > waterCount) {
			return TileHelper.TileType.Grass; 
		} else if (forestCount > waterCount && forestCount > grassCount) {
			return TileHelper.TileType.Forest; 
		} else if (waterCount > forestCount && waterCount > grassCount) {
			return TileHelper.TileType.Water; 
		}

		//default to grass
		return TileHelper.TileType.Grass; 

	}

	public static bool isOutOfBounds (int xPos, int zPos)
	{
		if (xPos < 0 || xPos >= xWorldTileMax) {
			return true;
		}

		if (zPos < 0 || zPos >= zWorldTileMax) {
			return true; 
		}

		return false; 
	}

	public static bool tileAlreadyThere (int xPos, int zPos)
	{
		return worldArray [xPos, zPos] != null; 
	}

	public float getTileSize ()
	{
		return tileSize; 
	}

}
