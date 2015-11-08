using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class LevelSetup : MonoBehaviour 
{
    //Text Objects
    public Text widthText, lengthText, numObjectsText;

    //The Level Scene
    public string levelScene;

    //User Editable Variables
    private int width, length, numObjects;

    //Map Size Constants
    private const int DEFAULT_WIDTH = 2;
    private const int DEFAULT_LENGTH = 2;

    //Object Constants
    private const int MAX_OBJECTS = 150;
    private const int MIN_OBJECTS = 0;
    private const int DEFAULT_NUM_OBJECTS = 15;

    //Tests if all inputs are valid
    void TestValues()
    {
        #region Invalid Input Tests
        //Tests if the Width is a valid entry
        try
        {
            width = Convert.ToInt32(widthText.text);
        }
        catch (FormatException)
        {
            Debug.Log("Use a number to edit Width\nDefaulting to " + DEFAULT_WIDTH);
            width = DEFAULT_WIDTH;
        }

        //Tests if the Length is a valid entry
        try
        {
            length = Convert.ToInt32(lengthText.text);
        }
        catch (FormatException)
        {
            Debug.Log("Use a number to edit Lenght\nDefaulting to " + DEFAULT_LENGTH);
            length = DEFAULT_LENGTH;
        }

        //Tests if the number of objects is valid
        try
        {
            numObjects = Convert.ToInt32(numObjectsText.text);
        }
        catch (FormatException)
        {
            Debug.Log("Use a number to edit Length\nDefaulting to " + DEFAULT_NUM_OBJECTS);
            numObjects = DEFAULT_NUM_OBJECTS;
        }
        #endregion

        #region Value Tests
        //If the width value is too small then the width will default
        if (width < DEFAULT_WIDTH)
        {
            width = DEFAULT_WIDTH;
            widthText.text = DEFAULT_WIDTH.ToString();
        }

        //If the length value is too small then the length will default
        if (length < DEFAULT_LENGTH)
        {
            length = DEFAULT_LENGTH;
            widthText.text = DEFAULT_LENGTH.ToString();
        }
        if (numObjects < MIN_OBJECTS)
        {
            numObjects = MIN_OBJECTS;
            numObjectsText.text = MIN_OBJECTS.ToString();
        }
        else if(numObjects > MAX_OBJECTS)
        {
            numObjects = MAX_OBJECTS;
            numObjectsText.text = MAX_OBJECTS.ToString();
        }

        #endregion

        //Applies changes to the level creation script
        LevelCreation.groundWidth = width;
        LevelCreation.groundLength = length;
        LevelCreation.numObjects = numObjects;
    }

    public void OnStartClick()
    {
        TestValues();
    }
}
