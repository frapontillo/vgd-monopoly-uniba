using UnityEngine;
using System.Collections;
using System;

public class RemoveBuildings : MonoBehaviour {
	
	public void updateAllBuildings() {
		// Ciclo su tutte le mini-celle della board principale
		for (int i = 0; i < 40; i++) {
			// Ottengo la cella corrente
			GameObject curCell = GameObject.Find(i.ToString());
			try {
				// Ottengo il set di case incluse nella cella
				GameObject curSet = curCell.transform.Find("GlobalBuildings").gameObject;
				GameObject[] curBuildings = new GameObject[5];
				
				// Nascondo tutte le costruzioni
				for (int houseIndex = 0; houseIndex < 5; houseIndex++) {
					// trovo la componente che ci serve
					curBuildings[houseIndex] = curSet.transform.Find("b" + (houseIndex+1).ToString()).gameObject;
					curBuildings[houseIndex].SetActiveRecursively(false);
				}
			} catch (NullReferenceException e) {
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		updateAllBuildings();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
