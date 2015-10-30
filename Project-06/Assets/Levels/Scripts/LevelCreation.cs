using UnityEngine;
using System.Collections;

/// <summary>
/// @Author Jake Skov
/// @Desc Generates the level based on user defined parameters
/// </summary>
public class LevelCreation : MonoBehaviour 
{
    //Map Modifiers
    public static int groundWidth;
    public static int groundLength;
    public static int numObjects;

    //Object Variables
    public GameObject[] environmentObjects;
    public GameObject groundObject;
    public GameObject enviroObject;

    //Spawn Variables
    private int spawnXMin, spawnXMax;
    private int spawnYMin, spawnYMax;
    private int spawnHeight = 1;

	// Use this for initialization
	void Start () 
    {
        //Initializes the evironmentObjects array
        environmentObjects = new GameObject[numObjects];

        //Populates the Array
        for (int i = 0; i < environmentObjects.Length; i++)
        {
            environmentObjects[i] = enviroObject;
        }

        //Spawns the Objects
        SpawnObjects();
	}

    //Spawns Objects Randomly Around the Level
    void SpawnObjects()
    {
        //Temp Variables
        float tempSpawnPosX, tempSpawnPosY;

        //Calulates the min and max values for x and y
        CalculateSpawnArea();

        //Creates the Ground (This Is Very Important)
        //Instantiate(groundObject, Vector3.zero, Quaternion.identity);
        groundObject.transform.lossyScale.Set(groundWidth, 1f, 1f);
        groundObject.transform.localScale.Set(groundLength, 1f, 1f);

        Debug.Log(groundObject.transform.lossyScale.x + " " + groundObject.transform.lossyScale.y +
            " " + groundObject.transform.lossyScale.z);

        //Spawn the Environment Objects
        for (int i = 0; i < environmentObjects.Length; i++)
        {
            tempSpawnPosX = Random.Range(spawnXMin, spawnXMax + 1);
            tempSpawnPosY = Random.Range(spawnYMin, spawnYMax + 1);

            Instantiate(environmentObjects[i], new Vector3(tempSpawnPosX, (float)spawnHeight, tempSpawnPosY),
                Quaternion.identity);
        }
    }

    //BASIC MATHS!
    void CalculateSpawnArea()
    {
        //Set X values
        spawnXMin = 5 * (-1 * (groundWidth / 2));
        spawnXMax = 5 * (groundWidth / 2);

        //Set Y values
        spawnYMin = 5 * (-1 * groundWidth / 2);
        spawnYMax = 5 * (groundWidth / 2);

        Debug.Log(spawnXMin + " " + spawnXMax + "\n" + spawnYMin + " " + spawnYMax);
    }
}
