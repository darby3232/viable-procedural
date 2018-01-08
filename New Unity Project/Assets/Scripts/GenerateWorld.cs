using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenerateWorld : MonoBehaviour {
    //create a struct to hold the tile
    [System.Serializable]
    public struct TileOutline
    {
        public GameObject tilePrefab;
        public int weightOutOfHundred; 

    }
    public TileOutline wallTile;

    //get all the prefabs sent in here cause we ballin' out
    //here be the forests
    public TileOutline forest1;
    public TileOutline forest2;
    public TileOutline forest3;
    public TileOutline forest4;
    public TileOutline forest5;
    public TileOutline forest6;
    public TileOutline forest7;
    public GameObject defaultForest;
    //here be the forests array
    private TileOutline[] forestPrefabs; 

    //here be the grasses
    public TileOutline grass1;
    //here be the grasses array
    private TileOutline[] grassPrefabs;

    //here be the water
    public TileOutline water1;
    public TileOutline water2;
    public TileOutline water3;
    public TileOutline water4;
    public TileOutline water5;
    public TileOutline water6;
    public TileOutline water7;
    public TileOutline water8;
    public TileOutline water9;
    public TileOutline water10;
    public GameObject defaultWater;
    //here be the water array
    private TileOutline[] waterPrefabs;


    public int numOfTilesPerSide;
    public int tileSize; 
    public int differentTileTypes = 3;
    public bool isInitialPopulateBiased; 
    public int previousTileBias; 

    struct World
    {
        public int[,] tileTypes;
        public int sideLength;
        public int numWater;
        public int numGrass;
        public int numForest;

    }

    World world;

    // Use this for initialization
    void Start()
    {
        //make the GameObject arrays for each land type
        fillPrefabArrays(); 
        world = new World();
        world.sideLength = numOfTilesPerSide;
        world.tileTypes = new int[world.sideLength, world.sideLength];

        if (isInitialPopulateBiased)
        {
            populateWorldPreviousTileBias();
        }
        else
        {
            populateWorldRandomly();
        }
        //organize world
       // printWorld();

        organizeWorld();    
        createWorld();
        //USELESS FOR NOW :( can't see most of it.
     }

    /*fills the arrays with the Prefabs passed in
    I use the arrays for easy random operations*/
    void fillPrefabArrays()
    {
        forestPrefabs = new TileOutline[] { forest1, forest2, forest3, forest4, forest5, forest6, forest7 };
        grassPrefabs = new TileOutline[] { grass1 };
        waterPrefabs = new TileOutline[] { water1, water2, water3, water4, water5, water6, water7, water8, water9, water10 };
    }   

    

    void populateWorldRandomly()
    {
        //iterate through all the tiles
        for(int i = 0; i < world.sideLength; i++)
        {
            for(int j = 0; j < world.sideLength; j++)
            {
                if (i == 0 || j == 0 || i == (world.sideLength - 1) || j == (world.sideLength - 1))
                {
                    world.tileTypes[i, j] = -1;//this is the wall tile
                }
                else
                {
                    //assign the random tile type
                    int tileType = Random.Range(0, differentTileTypes);
                    world.tileTypes[i, j] = tileType;
                }
            }
        }
    }

    void populateWorldPreviousTileBias()
    {
        int previousTile = -1; 
        //iterate through all the tiles
        for (int i = 0; i < world.sideLength; i++)
        {
            for (int j = 0; j < world.sideLength; j++)
            {
                if(i == 0 || j == 0 || i == (world.sideLength - 1) || j == (world.sideLength - 1))
                {
                    world.tileTypes[i, j] = -1;//this is the wall tile
                }
                else
                {
                    int randomNum = Random.Range(0, 100);
                    //LAST TILE CREATED BIAS
                    if (previousTile != -1 && previousTileBias >= randomNum)
                    {
                        world.tileTypes[i, j] = previousTile;
                    }
                    else
                    {
                        //assign the random tile type
                        int tileType = Random.Range(0, differentTileTypes);
                        world.tileTypes[i, j] = tileType;
                        previousTile = tileType;
                    }
                }    
            }
        }
    }
    /*
    * 1.Look through surrounding blocks, counting number of each type. 
    * 2.Take highest occcuring neighbor, and change at rate of (# of highest surrounding type / SBs). SBs being the total surrounding blocks. 
    */
    void organizeWorld()
    {
        for (int i = 1; i < world.sideLength - 1; i++)
        {
            for (int j = 1; j < world.sideLength - 1; j++)
            {
                //1st replacement
                world.tileTypes[i, j] = getTypeToReplaceSurroundingChance(i, j);
                //2nd replacement (More important replacement)
                world.tileTypes[i, j] = getTypeToReplaceMostAlike(i, j);
            }
        }
    }

   int getTypeToReplaceMostAlike(int xCoord, int zCoord)
   {
        int[] types = new int[differentTileTypes]; 

        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                //if not the block being assesed
                //if(i != 0 && j != 0) {
                    //get the type of the given tile, then add to that list
                    int tileNum = world.tileTypes[xCoord + i, zCoord + j];

                                     
                    
                    if (tileNum > -1)
                    {                  
                        types[tileNum]++;
                    }
               // }
            }
        }
        //initially stays the same
        int numOfType = types[world.tileTypes[xCoord, zCoord]];
        int typeToReturn = world.tileTypes[xCoord, zCoord];
        for (int i = 0; i < types.Length; i++)
        {
            if(types[i] > numOfType)
            {
                numOfType = types[i]; 
                typeToReturn = i;
            } 
        }

        //Debug.Log(typeToReturn);


        return typeToReturn; 
   }


    int getTypeToReplaceSurroundingChance(int xCoord, int zCoord)
    {
        int[] types = new int[9];
        int currIndex = 0;

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                //get the type of the given tile, then add to that list[TRYING TO BE FANCY WITH THE ++]
                types[currIndex++] = world.tileTypes[xCoord + i, zCoord + j];
            }
        }
        int randomIndex = -1;

        do
        {
            randomIndex = Random.Range(0, 9);
        } while (types[randomIndex] == -1);

         
        return types[randomIndex];
    }

    void createWorld()
    {
        for (int i = 0; i < world.sideLength; i++)
        {
            for (int j = 0; j < world.sideLength; j++)
            {
                //get the tile type here
                int tileType = world.tileTypes[i, j];
                //create the tile to creat
                TileOutline tileToInstantiate = grassPrefabs[0];//WARNING MAKING ALL GRASS AT START!
                switch (tileType)
                {
                    case -1:
                        //get wall object
                        tileToInstantiate = wallTile; 
                        break;
                    case 0:
                        world.numForest++;
                        //get some forest prefab
                        int numForestTiles = forestPrefabs.Length;
                        int fTilePosInArray = Random.Range(0, numForestTiles);
                        tileToInstantiate = forestPrefabs[fTilePosInArray];                      
                        if (tileToInstantiate.weightOutOfHundred < Random.Range(0, 101))
                        {
                            tileToInstantiate.tilePrefab = defaultForest;
                        }
                        break;
                    case 1: 
                        world.numGrass++;
                        int numGrassTiles = grassPrefabs.Length;
                        int gTilePosInArray = Random.Range(0, numGrassTiles);
                        tileToInstantiate = grassPrefabs[gTilePosInArray];
                        if (tileToInstantiate.weightOutOfHundred < Random.Range(0, 101))
                        {
                            tileToInstantiate.tilePrefab = grass1.tilePrefab;
                        }
                        break;  
                    case 2:
                        world.numWater++;
                        int numWaterTiles = waterPrefabs.Length;
                        int wTilePosInArray = Random.Range(0, numWaterTiles);
                        tileToInstantiate = waterPrefabs[wTilePosInArray];
                        if (tileToInstantiate.weightOutOfHundred < Random.Range(0, 101))
                        {
                            tileToInstantiate.tilePrefab = defaultWater;
                        }
                        break;

                }

                //Set up transform of the prefab to be made
                Transform tileTransform = tileToInstantiate.tilePrefab.transform;
                tileTransform.position = new Vector3(i * tileSize, 0, j * tileSize);
                Quaternion tileQuaternion = Quaternion.identity;
                tileQuaternion.eulerAngles = new Vector3(-90, 0, 0);


                if (tileType != -1)
                {
                    tileTransform.localScale = new Vector3(tileSize, tileSize, tileSize);

                    //Create the random Quaternion to rotate around
               
                    int[] possibleYRotationAngles = new int[] { 0, 90, 180, 270 };
                    int randomAngle = possibleYRotationAngles[Random.Range(0, 3)];
                    tileQuaternion = Quaternion.identity;
                    tileQuaternion.eulerAngles = new Vector3(0, randomAngle, 0);
                }

                //Instantiate tile chosen in switch statement
                Instantiate(tileToInstantiate.tilePrefab, tileTransform.position, tileQuaternion);


            }
        }
    }


    //print the world out
    void printWorld()
    {
        Debug.Log("got into printWorld()");
        Debug.Log("world.sideLength: " + world.sideLength);
        string output = ""; 
        for (int i = 0; i < world.sideLength; i++)
        {
            //print out a horizontal dividing line
            for (int j = 0; j < world.sideLength * 3; j++)
            {
                output += "_";
            }
            //next line
            output += "\n";
                output += "|"; 
            for (int j = 0; j < world.sideLength; j++)
            {
                output += " " + world.tileTypes[i, j] + " |"; 
            }
            //next line
            output += "\n";
        }
        //print out a horizontal dividing line
        for (int j = 0; j < world.sideLength * 3; j++)
        {
            output += "_";
        }
        output += "\n";
        output += "numForest: " + world.numForest + "\n";
        output += "numGrass: " + world.numGrass + "\n";
        output += "numWater: " + world.numWater + "\n";


        Debug.Log(output); 
    }
}
