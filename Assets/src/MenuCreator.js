public function enterMenu(strMenu : String) {
	// Cerco e ottengo il menu e la camera
	var menu: GameObject = GameObject.Find(strMenu);
	var cur_camera: GameObject = GameObject.Find("camera");
	var selection_camera: GameObject = GameObject.Find("selection_camera");
	
	if (menu != null) {
		if (strMenu != "characters") {
			// Avvio le animazioni per ogni figlio nel menu
			for (var child: Transform in menu.transform) {
				yield WaitForSeconds (0.05);
				iTween.MoveTo(child.gameObject, {"x":0.5, "transition":"easeInExpo", "time":1});
			}
		} else {
			// Blocco la rotazione della camera
			cur_camera.GetComponent(RotateCamera).stopRotate();
			// Faccio scendere il livello delle pedine
			iTween.MoveTo(menu.transform.gameObject, {"y":400, "transition":"easeInExpo", "time":1});
			// Muovo la camera alle nuove coordinate, ruotandola
			iTween.MoveTo(cur_camera.transform.gameObject, {"x":selection_camera.transform.position.x, "y":selection_camera.transform.position.y, "z":selection_camera.transform.position.z, "transition":"easeInExpo", "time":2});
			iTween.RotateTo(cur_camera.transform.gameObject, {"x":selection_camera.transform.eulerAngles.x, "y":selection_camera.transform.eulerAngles.y, "z":selection_camera.transform.eulerAngles.z, "transition":"easeInExpo", "time":2});
			// Abilito la selezione del character
            print(menu);
			menu.GetComponent(CharacterSelector).enableSelector();
		}
	}
}

public function exitMenu(strMenu : String){
	// Cerco e ottengo il menu
	var menu: GameObject = GameObject.Find(strMenu);
	var cur_camera: GameObject = GameObject.Find("camera");
	var selection_camera: GameObject = GameObject.Find("selection_camera");
	
	if (menu != null) {
		if (strMenu != "characters") {
			// Avvio le animazioni per ogni figlio nel menu
			for (var child: Transform in menu.transform) {
				yield WaitForSeconds (0.05);
				iTween.MoveTo(child.gameObject, {"x":-0.5, "transition":"easeOutExpo", "time":5});
			}
		} else {
			// Disabilito la selezione del character
			menu.GetComponent(CharacterSelector).disableSelector();
			// Faccio risalire il livello delle pedine
			iTween.MoveTo(menu.transform.gameObject, {"y":1000, "transition":"easeOutExpo", "time":1});
			// Riavvio la rotazione della camera
			cur_camera.GetComponent(RotateCamera).startRotate();
			// Muovo la camera alle vecchie coordinate, ruotandola
			iTween.MoveTo(cur_camera.transform.gameObject, {"x":2350, "y":480, "z":-500, "transition":"easeInExpo", "time":2});
			iTween.RotateTo(cur_camera.transform.gameObject, {"x":14, "y":314, "z":0, "transition":"easeInExpo", "time":2});
		}
	}
}

public function backMenu(strMenu : String){
	// Cerco e ottengo il menu
	var menu: GameObject = GameObject.Find(strMenu);
	
	if (menu != null) {
		// Avvio le animazioni per ogni figlio nel menu
		for (var child: Transform in menu.transform) {
				yield WaitForSeconds (0.05);
				iTween.MoveTo(child.gameObject, {"x":1.5, "transition":"easeOutExpo", "time":5});
		}
	}
}

function Start() {
	enterMenu("menu0");
}