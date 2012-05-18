using UnityEngine;
using System.Collections;

public class LocalBuildingCompany : MonoBehaviour {
	GameManager gMgr = null;
	Property curProperty = null;
	public GameObject[] curBuildings = new GameObject[5];
	
	public bool debug = false;
	public float fallingTime = 4F;
	public int buildings = 0;
		
	// Use this for initialization
	void Start () {
		// Nascondo tutte le costruzioni
		for (int houseIndex = 0; houseIndex < 5; houseIndex++) {
			// trovo la componente che ci serve
			curBuildings[houseIndex] = GameObject.Find("b" + (houseIndex+1).ToString());
			curBuildings[houseIndex].SetActiveRecursively(false);
		}
		// Cerco di ottenere il GameManager
		if (!debug)
			gMgr = GameObject.Find("GameManager").GetComponent<GameManager>();
		// Cerco di ottenere la proprieta' attuale sulla quale ci troviamo
		if (gMgr != null) {
			curProperty = gMgr.curPlayer.getCurrentCell().getProperty();
			// se la proprieta' esiste
			if (curProperty != null) {
				// ottengo il numero di edifici
				buildings = curProperty.getNumberOfBuildings();
				// abbasso gli edifici
				lowerNoAnimation(buildings);
			}
		} else {
			// qui entro solo se sono in debug
			// abbasso 3 case per testing
			lower(1);
		}
	}
	
	public void lowerNoAnimation(int numberOfBuildings) {
		// Se c'è un albergo
		if (numberOfBuildings == 5) {
			// abbasso solo l'albergo
			lowerHotelNoAnimation();
		} else {
			// Per ogni casa che so essere costruita su questa proprieta'
			for (int i = 0; i < numberOfBuildings; i++) {
				// abbasso una casa
				lowerOneHouseNoAnimation(i);
			}
		}
	}
	
	public void lower(int numberOfBuildings) {
		// Se compro direttamente l'albergo
		if (numberOfBuildings == 5) {
			// abbasso solo l'albergo
			lowerHotel();
		}
		// se avevo buildings case e voglio l'albergo
		else if (buildings + numberOfBuildings == 5) {
			// prima distruggo le case
			destroyBuildings(buildings);
			// poi creo l'albergo
			lowerHotel();
		} else {
			// Per ogni casa che so essere costruita su questa proprieta'
			for (int i = buildings; i < buildings + numberOfBuildings; i++) {
				// abbasso una casa
				lowerOneHouse(i);
			}
		}
		buildings+=numberOfBuildings;
	}
	
	public void higher(int numberOfBuildings) {
		// Se ho l'albergo
		if (buildings == 5) {
			// alzo prima l'albergo
			higherHotel();
			// abbasso le case rimanenti
			for (int i = 0; i < 5-numberOfBuildings; i++) {
				lowerOneHouse(i);
			}
		} else {
			// Per ogni casa che so essere costruita su questa proprieta'
			for (int i = 0; i < numberOfBuildings; i++) {
				// abbasso una casa
				higherOneHouse(buildings - i - 1);
			}
		}
		
		buildings-=numberOfBuildings;
	}
	
	public void destroyBuildings(int numberOfBuildings) {
		for (int i = numberOfBuildings - 1; i >= 0; i--) {
			higherOneHouse(i);
		}
	}
	
	public void lowerOneHouseNoAnimation(int houseIndex) {
		// faccio riapparire la casa
		curBuildings[houseIndex].SetActiveRecursively(true);
		// modifico l'altezza dell'oggetto (y = 20)
		float x = curBuildings[houseIndex].transform.position.x;
		float z = curBuildings[houseIndex].transform.position.z;
		curBuildings[houseIndex].transform.position = new Vector3(x, 20, z);
	}
	
	public void lowerHotelNoAnimation() {
		// abilito il rendering della mesh
		curBuildings[4].SetActiveRecursively(true);
		// modifico l'altezza dell'oggetto (y = 20)
		float x = curBuildings[4].transform.position.x;
		float z = curBuildings[4].transform.position.z;
		curBuildings[4].transform.position = new Vector3(x, 20, z);
	}
	
	public void lowerOneHouse(int houseIndex) {
		// faccio riapparire la casa
		curBuildings[houseIndex].SetActiveRecursively(true);
		// ottengo la posizione finale
		float x = curBuildings[houseIndex].transform.position.x;
		float z = curBuildings[houseIndex].transform.position.z;
		Vector3 newPos = new Vector3(x, 20, z);
		// avvio il movimento
		iTween.MoveTo(curBuildings[houseIndex], iTween.Hash("position", newPos, "time", fallingTime, "easeType", iTween.EaseType.easeOutBounce));
	}
	
	public void higherOneHouse(int houseIndex) {
		// ottengo la posizione finale
		float x = curBuildings[houseIndex].transform.position.x;
		float z = curBuildings[houseIndex].transform.position.z;
		Vector3 newPos = new Vector3(x, 100, z);
		// avvio il movimento
		iTween.MoveTo(curBuildings[houseIndex], iTween.Hash("position", newPos, "time", fallingTime, "easeType", iTween.EaseType.easeInBounce, "oncompletetarget", gameObject, "oncomplete", "hideAfter", "oncompleteparams", curBuildings[houseIndex]));
		// faccio sparire la casa automaticamente dopo la salita
		
	}
	
	public void lowerHotel() {
		// faccio riapparire l'hotel
		curBuildings[4].SetActiveRecursively(true);
		// ottengo la posizione finale
		float x = curBuildings[4].transform.position.x;
		float z = curBuildings[4].transform.position.z;
		Vector3 newPos = new Vector3(x, 20, z);
		// avvio il movimento
		iTween.MoveTo(curBuildings[4], iTween.Hash("position", newPos, "time", fallingTime, "easeType", iTween.EaseType.easeOutBounce));
	}
	
	public void higherHotel() {
		// ottengo la posizione finale
		float x = curBuildings[4].transform.position.x;
		float z = curBuildings[4].transform.position.z;
		Vector3 newPos = new Vector3(x, 100, z);
		// avvio il movimento
		iTween.MoveTo(curBuildings[4], iTween.Hash("position", newPos, "time", fallingTime, "easeType", iTween.EaseType.easeInBounce, "oncompletetarget", gameObject, "oncomplete", "hideAfter", "oncompleteparams", curBuildings[4]));
		// faccio sparire l'hotel
	}
	
	private void hideAfter(GameObject curBuilding) {
		curBuilding.SetActiveRecursively(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
