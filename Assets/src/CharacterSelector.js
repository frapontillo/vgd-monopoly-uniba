var characterSelectorEnabled: boolean = false;
public var personaggio: int = 0;
var cur_camera: GameObject;

function Awake () {
}

function Start() {
	
}

public function enableSelector() {
	characterSelectorEnabled = true;
}

public function disableSelector() {
	characterSelectorEnabled = false;
}

function Update() {
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

function startTurning(theDirection:String) {

	// Blocco la possibilita' di ruotare i personaggi
	characterSelectorEnabled = false;

	if (theDirection == "left") {
		iTween.RotateAdd(transform.gameObject, {"x":0, "y":-60, "z":0, "transition":"easeInElastic", "time":0.3, "oncomplete":"left"});
	} else if (theDirection == "right") {
		iTween.RotateAdd(transform.gameObject, {"x":0, "y":60, "z":0, "transition":"easeInElastic", "time":0.3, "oncomplete":"right"});
	}
}

function left() {
	// Decremento il contatore
	if (personaggio == 0)
		personaggio = 5;
	else personaggio--;
	
	// Riabilito la funzionalita' di movimento
	characterSelectorEnabled = true;
	
	print("character #" + personaggio);
}

function right() {
	// Incremento il contatore
	if (personaggio == 5)
		personaggio = 0;
	else personaggio++;
	
	// Riabilito la funzionalita' di movimento
	characterSelectorEnabled = true;
	
	print("character #" + personaggio);
}

function startTheGame() {
		print("STARTED!");
        Application.LoadLevel("Board");
		cur_camera = GameObject.Find("camera");
        GameObject.Find("character_selection").GetComponent("CharacterSelection").personaggio = this.personaggio;
}