using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenerateWorld : MonoBehaviour {
    //get all the prefabs sent in here cause we ballin' out

    //here be the forests
    public GameObject forest1;
    public GameObject forest2;
    public GameObject forest3;
    public GameObject forest4;
    public GameObject forest5;
    public GameObject forest6;
    public GameObject forest7;
    public GameObject forest8;
    //here be the forests array
    private GameObject[] forestPrefabs; 

    //here be the grasses
    public GameObject grass1;
    //here be the grasses array
    private GameObject[] grassPrefabs;

    //here be the water
    public GameObject water1;
    public GameObject water2;
    public GameObject water3;
    public GameObject water4;
    public GameObject water5;
    public GameObject water6;
    public GameObject water7;
    public GameObject water8;
    public GameObject water9;
    public GameObject water10;
    public GameObject water11;
    //here be the water array
    private GameObject[] waterPrefabs;


    public int numOfTilesPerSide;
    public int tileSize; 
    public int differentTileTypes = 3; 

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
        populateWorldRandomly();    
        createWorld();
        printWorld(); 
    }

    /*fills the arrays with the Prefabs passed in
    I use the arrays for easy random operations*/
    void fillPrefabArrays()
    {
        forestPrefabs = new GameObject[] { forest1, forest2, forest3, forest4, forest5, forest6, forest7, forest8 };
        grassPrefabs = new GameObject[] { grass1 };
        waterPrefabs = new GameObject[] { water1, water2, water3, water4, water5, water6, water7, water8, water9, water10, water11 };
    }   


    void populateWorldRandomly()
    {
        //iterate through all the tiles
        for(int i = 0; i < world.sideLength; i++)
        {
            for(int j = 0; j < world.sideLength; j++)
            {
                //assign the random tile type
                int tileType = Random.Range(0, differentTileTypes);
                world.tileTypes[i, j] = tileType;

            }
        }
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
                GameObject tileToInstantiate = grassPrefabs[0];//WARNING MAKING ALL GRASS AT START!
                switch (tileType)
                {
                    case 0:
                        world.numForest++;
                        //get some forest prefab
                        int numForestTiles = forestPrefabs.Length;
                        int fTilePosInArray = Random.Range(0, numForestTiles - 1);
                        tileToInstantiate = forestPrefabs[fTilePosInArray]; 
                        break;
                    case 1: 
                        world.numGrass++;
                        int numGrassTiles = grassPrefabs.Length;
                        int gTilePosInArray = Random.Range(0, numGrassTiles - 1);
                        tileToInstantiate = grassPrefabs[gTilePosInArray];
                        break;  
                    case 2:
                        world.numWater++;
                        int numWaterTiles = waterPrefabs.Length;
                        int wTilePosInArray = Random.Range(0, numWaterTiles - 1);
                        tileToInstantiate = waterPrefabs[wTilePosInArray];
                        break;

                }

                //Set up transform of the prefab to be made
                Transform tileTransform = tileToInstantiate.transform; 
                tileTransform.position = new Vector3 (i * tileSize, 0, j * tileSize); 
                tileTransform.localScale = new Vector3(tileSize, tileSize, tileSize);

                //Create the random Quaternion to rotate around
                int[] possibleYRotationAngles = new int[] { 0, 90, 180, 270 };
                int randomAngle = possibleYRotationAngles[Random.Range(0, 3)];
                Quaternion tileQuaternion = Quaternion.identity;
                tileQuaternion.eulerAngles = new Vector3(0, randomAngle, 0);

                //Instantiate tile chosen in switch statement
                Instantiate(tileToInstantiate, tileTransform.position, tileQuaternion);


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
