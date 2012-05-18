private var normalTexture : Texture;
private var hoverTexture : Texture;
private var pressedTexture : Texture;
var messagee : GameObject;
var message = "ButtonPressed";

private var state = 0;
private var myGUITexture : GUITexture;
private var current_object : String;

myGUITexture = GetComponent(GUITexture);
current_object = gameObject.name;
hoverTexture = Resources.Load("GUITextures/"+current_object+"_over");
normalTexture = Resources.Load("GUITextures/"+current_object+"_normal");
pressedTexture = Resources.Load("GUITextures/"+current_object+"_down");

function OnMouseEnter() {
	state++;
	if (state == 1)
		myGUITexture.texture = hoverTexture;
}

function OnMouseDown() {
	state++;
	if (state == 2)
		myGUITexture.texture = pressedTexture;
}

function OnMouseUp() {
	if (state == 2) {
		state--;
		var menuCreator : MenuCreator;
		menuCreator = gameObject.GetComponent("MenuCreator");
		
		// Effettuo una azione a seconda di cosa e' stato cliccato
		
		// MENU 0
		if (current_object == "0.1") {
			menuCreator.exitMenu("menu0");
			menuCreator.enterMenu("menu1");
		} else if (current_object == "0.2") {
			//menuCreator.exitMenu("menu0");
		} else if (current_object == "0.3") {
			//menuCreator.exitMenu("menu0");
		} else if (current_object == "0.4") {
			//menuCreator.exitMenu("menu0");
		} else if (current_object == "0.5") {
			//menuCreator.exitMenu("menu0");
		} else if (current_object == "0.6") {
			menuCreator.exitMenu("menu0");
			yield WaitForSeconds(1);
			Application.Quit();
		}
		
		// MENU 1
		else if (current_object == "1.1") {
			menuCreator.exitMenu("menu1");
			menuCreator.enterMenu("menu2");
		} else if (current_object == "1.2") {
			//menuCreator.exitMenu("menu1");
			//menuCreator.enterMenu("menu2");
		} else if (current_object == "1.3") {
			//menuCreator.exitMenu("menu1");
			//menuCreator.enterMenu("menu2");
		} else if (current_object == "1.4") {
			menuCreator.backMenu("menu1");
			menuCreator.enterMenu("menu0");
		}
		
		// MENU 2
		else if (current_object == "2.1") {
			menuCreator.exitMenu("menu2");
			menuCreator.enterMenu("characters");
			menuCreator.enterMenu("menu3");
		} else if (current_object == "2.2") {
			//menuCreator.exitMenu("menu2");
		} else if (current_object == "2.3") {
			//menuCreator.exitMenu("menu2");
		} else if (current_object == "2.4") {
			//menuCreator.exitMenu("menu2");
		} else if (current_object == "2.5") {
			menuCreator.backMenu("menu2");
			menuCreator.enterMenu("menu1");
		}
		
		// MENU 3
		else if (current_object == "3.4") {
			menuCreator.backMenu("menu3");
			menuCreator.exitMenu("characters");
			menuCreator.enterMenu("menu2");
		}
		
		if (messagee) {
			messagee.SendMessage(message, gameObject);
		}
	} else {
		state --;
		if (state < 0)
			state = 0;
	}
	myGUITexture.texture = normalTexture;
}

function OnMouseExit() {
	if (state > 0)
		state--;
	if (state == 0)
		myGUITexture.texture = normalTexture;
}