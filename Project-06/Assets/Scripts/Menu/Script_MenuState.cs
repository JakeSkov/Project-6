using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum MenuStates
{
    MENU_MAIN,
    MENU_CONNECT,
    MENU_SETUP,
    MENU_QUITTING,
    SETUP_STARTING_HOST,
    SETUP_STARTING_SERVER,
    CONNECT_CONNECTING_TO_SERVER
}

public enum MenuCommands
{
    GOTO_CONNECT,
    GOTO_SETUP,
    GOTO_MAIN,
    QUIT_APPLICATION,
    SETUP_HOST,
    SETUP_SERVER,
    CONNECT_CLIENT
}

public class Script_MenuState : MonoBehaviour {

    Dictionary<MenuTransitions, MenuStates> allTransitions; //A  dictionary for all state transitions

    Dictionary<string, MenuCommands> enumParse;

    public MenuStates CurrentState { get; private set; }    //A property to store the current state of the machine
    public MenuStates PreviousState { get; private set; }   //A property to store the previous state of the machine

    public GameObject mainMenu;                             //A reference to the main menu game object for menu navigation
    public GameObject connectMenu;                          //A reference to the connect menu game object for menu navigation
    public GameObject setupMenu;                            //A reference to the setup menu game object for menu navigation
    public NetworkManager manager;                          //A reference to the network manager for managing connections

    // Use this for initialization
    void Start () {

        //Setup the current state to be main
        CurrentState = MenuStates.MENU_MAIN;

        //Setup the previous state to be connect
        PreviousState = MenuStates.MENU_CONNECT;

        //Create the dictionary 
        allTransitions = new Dictionary<MenuTransitions, MenuStates>
        {
            //Defines the state transitions where
            //{ new MenuTransition(actual state of the machine, transition state/command), final state of the machine) }
			{new MenuTransitions(MenuStates.MENU_MAIN, MenuCommands.GOTO_CONNECT), MenuStates.MENU_CONNECT},
			{new MenuTransitions(MenuStates.MENU_MAIN, MenuCommands.GOTO_SETUP), MenuStates.MENU_SETUP},
			{new MenuTransitions(MenuStates.MENU_MAIN, MenuCommands.QUIT_APPLICATION), MenuStates.MENU_QUITTING},
			{new MenuTransitions(MenuStates.MENU_CONNECT, MenuCommands.GOTO_MAIN), MenuStates.MENU_MAIN},
			{new MenuTransitions(MenuStates.MENU_CONNECT, MenuCommands.CONNECT_CLIENT),MenuStates.CONNECT_CONNECTING_TO_SERVER},
			{new MenuTransitions(MenuStates.MENU_SETUP,MenuCommands.GOTO_MAIN),MenuStates.MENU_MAIN},
			{new MenuTransitions(MenuStates.MENU_SETUP,MenuCommands.SETUP_HOST),MenuStates.SETUP_STARTING_HOST},
			{new MenuTransitions(MenuStates.MENU_SETUP,MenuCommands.SETUP_SERVER),MenuStates.SETUP_STARTING_SERVER},
        };

        //Create the dictionary where
        //{string that is passed by the button, command the string represets}
        enumParse = new Dictionary<string, MenuCommands>
        {
			{"goto connect menu", MenuCommands.GOTO_CONNECT},
			{"goto setup menu", MenuCommands.GOTO_SETUP},
			{"goto main menu", MenuCommands.GOTO_MAIN},
			{"quit application", MenuCommands.QUIT_APPLICATION},
			{"setup host", MenuCommands.SETUP_HOST},
			{"setup server", MenuCommands.SETUP_SERVER},
			{"connect to server", MenuCommands.CONNECT_CLIENT}
        };

        Debug.Log("Curr state = " + CurrentState);
    }

    /// <summary>
    /// Gets the next state if a given command/transition is run
    /// </summary>
    /// <param name="command">The command (or transition) to run</param>
    /// <returns></returns>
    MenuStates GetNext(MenuCommands command)
    {
        //Construct the new transition based on the machines current state, and the supplied transition/command
		MenuTransitions newTransition = new MenuTransitions (CurrentState, command);
        //Location to store the new state for the machine to go into
		MenuStates newState;
        //Make sure that the transition is valid, using the dictionary lookup
		if (!allTransitions.TryGetValue(newTransition, out newState))
			throw new UnityException("Invalid transition " + CurrentState + " -> " + command);
        //If at this point we have not broken anything, return the new state
		return newState;
    }

    /// <summary>
    /// Moves the state machine into the next state given a specific command/transition and triggers the transition
    /// </summary>
    /// <param name="command">The command/transition to run the machine through</param>
    public void MoveNextAndTransition(string command)
    {
        //Record the previous state for transition purposes
		PreviousState = CurrentState;
        //Location for the new command
		MenuCommands newCommand;
        //Try to get the value
		if(!enumParse.TryGetValue(command, out newCommand))
			throw new UnityException("Invalid Command " + command);
        //Setup the next state 
		CurrentState = GetNext(newCommand);
        //Transition to the next state
		Transition ();
    }

    /// <summary>
    /// Will run any neccessary code for the transition from one state to the next
    /// </summary>
    void Transition()
    {
		switch (PreviousState) {
			//if my previous state is MAIN
			case MenuStates.MENU_MAIN:
				//if my new state is CONNECT
				if (CurrentState == MenuStates.MENU_CONNECT) {
					Debug.Log ("Transition from Main to Connect");	
					mainMenu.SetActive(false);
					connectMenu.SetActive(true);
				}
				//if my new state is SETUP
				else if (CurrentState == MenuStates.MENU_SETUP)
				{
					Debug.Log("Transition from Main to Start");
					mainMenu.SetActive(false);
					setupMenu.SetActive(true);
				}
				//if my new state is QUIT
				else if (CurrentState == MenuStates.MENU_QUITTING)
				{
					Debug.Log("Transition from Main to Quit");
					Application.Quit ();
				}
				break;
			//if my previous state is CONNECT
			case MenuStates.MENU_CONNECT:
				//if my new state is MAIN
				if (CurrentState == MenuStates.MENU_MAIN) 
				{
					Debug.Log ("Transition from Connect to Main");
					connectMenu.SetActive(false);
					mainMenu.SetActive(true);
				} 
				else if (CurrentState == MenuStates.CONNECT_CONNECTING_TO_SERVER) 
				{
					Debug.Log ("Transition from Connect to Connecting to Server");
					manager.StartClient ();
				}
				break;
			//if my previous state is SETUP
			case MenuStates.MENU_SETUP:
				//if my new state is MAIN
				if (CurrentState == MenuStates.MENU_MAIN) {
					Debug.Log ("Transition from Start to Main");
					setupMenu.SetActive (false);
					mainMenu.SetActive (true);
				} 
				else if (CurrentState == MenuStates.SETUP_STARTING_SERVER) 
				{
					Debug.Log ("Transitioning from Start to Server Starting");
					manager.StartServer();
				} 
				else if (CurrentState == MenuStates.SETUP_STARTING_HOST) 
				{
					Debug.Log ("Transition from Start to Host Starting");
					manager.StartHost();
				}
				break;
			default:
				break;
		}

    }

    /// <summary>
    /// Updates the IP on the manager
    /// </summary>
    /// <param name="message">The IP to update to</param>
    public void UpdateIP(Object message)
    {
		Text textObj = ((GameObject)message).GetComponent<Text> ();
		if (textObj.text == "") 
		{
			manager.networkAddress = "localhost";
		} 
		else 
		{
			manager.networkAddress = textObj.text;
		}
		Debug.Log ("Updating address to... " + textObj.text);
    }

    public void UpdatePort(Object message)
    {
		Text textObj = ((GameObject)message).GetComponent<Text> ();
		int newPort;

		if (int.TryParse (textObj.text, out newPort)) 
		{
			manager.networkPort = newPort;
		} 
		else 
		{
			manager.networkPort = 7777;
		}
		Debug.Log ("Updating port to... " + newPort.ToString ());
    }

    public void UpdateName(Object message)
    {

    }
}
