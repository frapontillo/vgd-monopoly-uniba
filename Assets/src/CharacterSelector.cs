using UnityEngine;
using System.Collections;

public class CharacterSelector : MonoBehaviour {

    public bool characterSelectorEnabled = false;
    public int personaggio = 0;
    public GameObject cur_camera;

	// Use this for initialization
	void Start () {
	
	}

    public void enableSelector() {
	    characterSelectorEnabled = true;
    }

    public void disableSelector() {
	    characterSelectorEnabled = false;
    }

    void Update() {
	    if (characterSelectorEnabled) {
		    if (Input.GetKeyDown("left")) {
			    startTurning("left");
		    } else if (Input.GetKeyDown("right")) {
			    startTurning("right");
		    }
		
		    if(Input.GetButtonDown("Jump")) {
			    startTheGame();
		    }
	    }
    }

    void startTurning(string theDirection) {

	    // Blocco la possibilita' di ruotare i personaggi
	    characterSelectorEnabled = false;
        Hashtable table1 = new Hashtable();
        table1.Add("x", 0);

	    if (theDirection == "left") {
		    iTween.RotateAdd(transform.gameObject, iTween.Hash("x",0, "y",-60, "z",0, "transition","easeInElastic", "time",0.3, "oncomplete","left"));
	    } else if (theDirection == "right") {
		    iTween.RotateAdd(transform.gameObject, iTween.Hash("x",0, "y",60, "z",0, "transition","easeInElastic", "time",0.3, "oncomplete","right"));
	    }
    }

    void left() {
	    // Decremento il contatore
	    if (personaggio == 0)
		    personaggio = 5;
	    else personaggio--;
	
	    // Riabilito la funzionalita' di movimento
	    characterSelectorEnabled = true;
	
	    print("character #" + personaggio);
    }

    void right() {
	    // Incremento il contatore
	    if (personaggio == 5)
		    personaggio = 0;
	    else personaggio++;
	
	    // Riabilito la funzionalita' di movimento
	    characterSelectorEnabled = true;
	
	    print("character #" + personaggio);
    }

    void startTheGame() {
        // Disabilito la funzionalita' di movimento
        characterSelectorEnabled = false;
        // Salvo il personaggio in una variabile accessibile dall'altro livello
        GameObject.Find("character_selection").GetComponent<CharacterSelection>().personaggio = personaggio;
        Application.LoadLevel("Board");
		cur_camera = GameObject.Find("camera");
    }
}
