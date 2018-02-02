using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
*  TILE CHAR KEY
*   w - wall
*   f - forest
*   g - grass
*   t - water
*   d - desert
*
*/

public class GenerateWorld : MonoBehaviour {
    //create a struct to hold the tile
    [System.Serializable]
    public struct TileOutline
    {
        public GameObject tilePrefab;
        public int chanceOfPlacement;

    }

    //special type to tile. 
    [System.Serializable]
    public struct Barnacle
    {
        public GameObject barnacleMajor;
        public GameObject [] barnacleMinor;
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
    public Barnacle forestBarnacle; 
    public GameObject defaultForest;
    //here be the forests array
    private TileOutline[] forestPrefabs; 

    //here be the grasses
    public TileOutline grass1;
    //public TileOutline grass2;
    public Barnacle grassBarnacle; 
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
    public Barnacle waterBarnacle;
    public GameObject defaultWater;
    //here be the water array
    private TileOutline[] waterPrefabs;

    //here be the desert
    public TileOutline desert1;
    public GameObject defaultDesert;
    //here be the desert array
    private TileOutline[] desertPrefabs; 


    public int numOfTilesPerSide;
    public int tileSize; 
    public bool isInitialPopulateBiased;
    public bool checkSelfWhenOrganizing;
    public int previousTileBias;
    public int babyBarnacleChance; 
    public int organizeIterations;
    public int babyBarnacleSpreadX = 4; 
    public int babyBarnacleSpreadZ = 4; 

    private int differentTileTypes;


    struct World
    {
        public char [,] tileTypes;
        public int sideLength;
        public int numWater;
        public int numGrass;
        public int numForest;
        public int numDesert; 

    }

    private char[] tileChars = new char [] { 'f', 'g', 't'/*, 'd'*/ }; 

    World world;

    // Use this for initialization
    void Start()
    {
        //make the GameObject arrays for each land type
        differentTileTypes = tileChars.Length + 1; 
        fillPrefabArrays(); 
        world = new World();
        world.sideLength = numOfTilesPerSide;
        world.tileTypes = new char[world.sideLength, world.sideLength];

        if (isInitialPopulateBiased)
        {
            populateWorldPreviousTileBias();
        }
        else
        {
            populateWorldRandomly();
        }
       
        organizeWorld();        
        waterEasement();
        placeBarnacles();
        createWorld();
        //printWorld(); 
     }

    /*fills the arrays with the Prefabs passed in
    I use the arrays for easy random operations*/
    void fillPrefabArrays()
    {
        forestPrefabs = new TileOutline[] { forest1, forest2, forest3, forest4, forest5, forest6, forest7 };
        grassPrefabs = new TileOutline[] { grass1};
        waterPrefabs = new TileOutline[] { water1, water2, water3, water4, water5, water6, water7, water8, water9};
        desertPrefabs = new TileOutline[] { desert1 };
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
                    world.tileTypes[i, j] = 'w';//this is the wall tile
                }
                else
                {
                    //assign the random tile type
                    int tileType = Random.Range(0, tileChars.Length);

                    Debug.Log(tileType); 

                    char tileChar = tileChars[tileType];

                    Debug.Log(tileChar); 

                    world.tileTypes[i, j] = tileChar;
                }
            }
        }
    }

    void populateWorldPreviousTileBias()
    {
        char previousTile = 'w'; 
        //iterate through all the tiles
        for (int i = 0; i < world.sideLength; i++)
        {
            for (int j = 0; j < world.sideLength; j++)
            {
                if(i == 0 || j == 0 || i == (world.sideLength - 1) || j == (world.sideLength - 1))
                {
                    world.tileTypes[i, j] = 'w';//this is the wall tile
                }
                else
                {
                    int randomNum = Random.Range(0, 100);
                    //LAST TILE CREATED BIAS
                    if (previousTile != 'w' & previousTileBias >= randomNum)
                    {
                        world.tileTypes[i, j] = previousTile;
                    }
                    else
                    {
                        //assign the random tile type
                        int tileType = Random.Range(0, tileChars.Length);
                        char tileChar = tileChars[tileType];
                        world.tileTypes[i, j] = tileChar;   
                        previousTile = tileChar;
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
        for (int currOrganizeIteration = 0; currOrganizeIteration < this.organizeIterations; currOrganizeIteration++) {
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
    }

    void waterEasement()
    {
        //using second loop to to easement
        for (int i = 1; i < world.sideLength - 1; i++)
        {
            for (int j = 1; j < world.sideLength - 1; j++)
            {
                //Water check! replace non-water tiles around with desert tiles
                if (world.tileTypes[i, j] == 't')
                {
                    interterrainReplacement(i, j, 't', 'd');
                }
            }
        }
    }

    void placeBarnacles()
    {
        //calculate how many different barnacle instances should be created considering the number of tiles, and random values
        int numBarnacles = Mathf.FloorToInt(Mathf.Pow(this.numOfTilesPerSide, 2) / (1000 + (Random.value * 10000))); ;// + 1; 

       for(int i = 0; i < numBarnacles; i++)
        {
            //get locations of the barnalces to create randomly, away from edges
            int randX = Random.Range(5, this.numOfTilesPerSide - 4);
            int randZ = Random.Range(5, this.numOfTilesPerSide - 4);

            char prevTileType = world.tileTypes[randX, randZ]; 

            if(prevTileType == 'd')
            {
                continue; 
            }

            //Bomb the area with the tileType
            for(int x = -babyBarnacleSpreadX + 1; x < babyBarnacleSpreadX; x++)
            {
                for(int z = -babyBarnacleSpreadZ + 1; z < babyBarnacleSpreadZ; z++)
                {
                    world.tileTypes[randX + x, randZ + z] = prevTileType; 
                    
                }
            }

            for (int x = -babyBarnacleSpreadX + 1; x < babyBarnacleSpreadX; x++)
            {
                for (int z = -babyBarnacleSpreadZ + 1; z < babyBarnacleSpreadZ; z++)
                {
                    //place chance for the baby barnacles
                    if ((x > 1 || x < -1) || (z > 1 || z < -1))
                    {
                        int barnaclePlacementChance = Random.Range(0, 100);
                        if (barnaclePlacementChance > babyBarnacleChance)
                        {
                            world.tileTypes[randX + x, randZ + z] = 'b';
                        }
                    }
                }
            }


            world.tileTypes[randX, randZ] = 'B'; 
        }

    }


    void interterrainReplacement(int xCoord, int zCoord, char currentTerrain, char terrainToSurround)
    {
        for (int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                if(i != 0 || j != 0)
                {
                    char tileChar = world.tileTypes[xCoord + i, zCoord + j]; 
                    if (tileChar != currentTerrain && tileChar != terrainToSurround && tileChar != 'w')
                    {
                        world.tileTypes[xCoord + i, zCoord + j] = terrainToSurround;
                    }
                }
            }
        }
    }

   char getTypeToReplaceMostAlike(int xCoord, int zCoord)
   {
        int[] types = new int[differentTileTypes];

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                //if not the block being assesed
                if (checkSelfWhenOrganizing || (i != 0 || j != 0))
                {
                    //get the type of the given tile, then add to that list
                    char tileChar = world.tileTypes[xCoord + i, zCoord + j];



                    if (tileChar == tileChars[0])
                    {
                        types[0]++;
                    }
                    else if (tileChar == tileChars[1])
                    {
                        types[1]++;
                    }
                    else if (tileChar == tileChars[2])
                    {
                        types[2]++;
                    }
                    /*else if (tileChar == tileChars[3])
                    {
                        types[3]++;
                    }*/
                }
               
            }
        }
        //initially stays the same
        int numOfType = 0; 
        char typeToReturn = world.tileTypes[xCoord, zCoord];
        for (int i = 0; i < types.Length; i++)
        {
            if(types[i] > numOfType)
            {
                numOfType = types[i]; 
                typeToReturn = tileChars[i];
            } 
        }

        //Debug.Log(typeToReturn);


        return typeToReturn; 
   }


    char getTypeToReplaceSurroundingChance(int xCoord, int zCoord)
    {
        char[] types = new char[9];
        int currIndex = 0;

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                //get the type of the given tile, then add to that list[TRYING TO BE FANCY WITH THE ++]
                if (checkSelfWhenOrganizing || (i != 0 || j != 0))
                {
                    types[currIndex++] = world.tileTypes[xCoord + i, zCoord + j];
                }
            }
        }
        int randomIndex = 'w';

        do
        {
            randomIndex = Random.Range(0, 9);
        } while (types[randomIndex] == 'w' || types[randomIndex] == 0);

         
        return types[randomIndex];
    }

    void createWorld()
    {
        for (int i = 0; i < world.sideLength; i++)
        {
            for (int j = 0; j < world.sideLength; j++)
            {
                //get the tile type here
                char tileType = world.tileTypes[i, j];
                //create the tile to create
                switch (tileType)
                {
                    case 'w':
                        //get wall object
                        placeBlock(i, j, wallTile.tilePrefab, 'w'); 
                        break;
                    case 'f':
                        world.numForest++;
                        //get some forest prefab
                        int numForestTiles = forestPrefabs.Length;
                        int fTilePosInArray = Random.Range(0, numForestTiles);
                        TileOutline tileToInstantiateF = forestPrefabs[fTilePosInArray];                      
                        if (tileToInstantiateF.chanceOfPlacement < Random.Range(0, 101))
                        {
                            tileToInstantiateF.tilePrefab = defaultForest;
                        }
                        placeBlock(i, j, tileToInstantiateF.tilePrefab, tileType);
                        break;
                    case 'g': 
                        world.numGrass++;
                        int numGrassTiles = grassPrefabs.Length;
                        int gTilePosInArray = Random.Range(0, numGrassTiles);
                        TileOutline tileToInstantiateG = grassPrefabs[gTilePosInArray];
                        if (tileToInstantiateG.chanceOfPlacement < Random.Range(0, 101))
                        {
                            tileToInstantiateG.tilePrefab = grass1.tilePrefab;
                        }
                        placeBlock(i, j, tileToInstantiateG.tilePrefab, tileType);
                        break;  
                    case 't':
                        world.numWater++;
                        int numWaterTiles = waterPrefabs.Length;
                        int wTilePosInArray = Random.Range(0, numWaterTiles);
                        TileOutline tileToInstantiateT = waterPrefabs[wTilePosInArray];
                        if (tileToInstantiateT.chanceOfPlacement < Random.Range(0, 101))
                        {
                            tileToInstantiateT.tilePrefab = defaultWater;
                        }
                        placeBlock(i, j, tileToInstantiateT.tilePrefab, tileType);
                        break; 
                   case 'd':
                        world.numDesert++;
                        int numDesertTiles = desertPrefabs.Length;
                        int dTilePosInArray = Random.Range(0, numDesertTiles);
                        TileOutline tileToInstantiateD = desertPrefabs[dTilePosInArray];
                        if (tileToInstantiateD.chanceOfPlacement < Random.Range(0, 101))
                        {
                            tileToInstantiateD.tilePrefab = defaultDesert;
                        }
                        placeBlock(i, j, tileToInstantiateD.tilePrefab, tileType);

                        break;
                    case 'B':
                        //check nearest tile type and that will be the type
                        char barnacleType = world.tileTypes[i, j - 1];
                        //get the correct barnacle type to use
                        Barnacle currBarnacle; 
                        if(barnacleType == 'f')
                        {
                            currBarnacle = forestBarnacle;
                            Debug.Log("Forest Barnacle!!");

                        }
                        else if(barnacleType == 'g')
                        {
                            currBarnacle = grassBarnacle;
                            Debug.Log("Grass Barnacle!!");

                        }
                        else
                        {
                            currBarnacle = waterBarnacle;
                            Debug.Log("Water Barnacle!!");

                        }
                        //now figure out where to place the baby barnacles by checking the surrounding area for b's
                        for (int x = -this.babyBarnacleSpreadX + 1; x < this.babyBarnacleSpreadX; x++)
                        {
                            for(int z = -this.babyBarnacleSpreadZ + 1; z < this.babyBarnacleSpreadZ; z++)
                            {
                                if(world.tileTypes[i + x, j + z] == 'b')
                                {
                                    GameObject chosenBarnacleMinor = currBarnacle.barnacleMinor[Random.Range(0, currBarnacle.barnacleMinor.Length)];
                                    placeBlock(i + x, j + z, chosenBarnacleMinor, 'b'); 
                                }
                            }
                        }
                        placeBlock(i, j, currBarnacle.barnacleMajor, 'B'); 
                        break; 

                }      
                
            }
        }
    }

    void placeBlock(int i, int j, GameObject tile, char tileType)
    {
        //Set up transform of the prefab to be made
        Transform tileTransform = tile.transform;
        tileTransform.position = new Vector3(i * tileSize, 0, j * tileSize);
        Quaternion tileQuaternion = Quaternion.identity;
        tileQuaternion.eulerAngles = new Vector3(-90, 0, 0);


        if (tileType != 'w')
        {
            //tileTransform.localScale = new Vector3(tileSize, tileSize, tileSize);

            //Create the random Quaternion to rotate around

            int[] possibleYRotationAngles = new int[] { 0, 90, 180, 270 };
            int randomAngle = possibleYRotationAngles[Random.Range(0, 3)];
            tileQuaternion = Quaternion.identity;
            tileQuaternion.eulerAngles = new Vector3(0, randomAngle, 0);
        }

        //Instantiate tile chosen in switch statement
        Instantiate(tile, tileTransform.position, tileQuaternion);
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
