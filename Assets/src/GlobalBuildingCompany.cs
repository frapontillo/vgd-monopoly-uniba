using UnityEngine;
using System.Collections;
using System;

public class GlobalBuildingCompany : MonoBehaviour {
	
	public void updateAllBuildings() {
		// Ottengo il numero di costruzioni sulla cella
		GameManager gMgr = GameObject.Find("GameManager").GetComponent<GameManager>();
		// Ciclo su tutte le mini-celle della board principale
		for (int i = 0; i < 40; i++) {
			// Ottengo la cella corrente
			GameObject curCell = GameObject.FindGameObjectWithTag(i.ToString());
			try {
				// Ottengo il set di case incluse nella cella
				GameObject curSet = curCell.transform.Find("GlobalBuildings").gameObject;
				GameObject[] curBuildings = new GameObject[5];
				
				// Nascondo prima tutte le costruzioni
				for (int houseIndex = 0; houseIndex < 5; houseIndex++) {
					// trovo la componente che ci serve
					curBuildings[houseIndex] = curSet.transform.Find("b" + (houseIndex+1).ToString()).gameObject;
					curBuildings[houseIndex].SetActiveRecursively(false);
				}
				
				int mBuildings = 0;
				try {
					mBuildings = gMgr.Cells[i].getProperty().getNumberOfBuildings();
				} catch (ArgumentOutOfRangeException e) {
					mBuildings = 0;
				}
				
				if (mBuildings > 0) {
					// Aggiorno i valori della cella
					setBuildings(mBuildings, curBuildings);
				}
			} catch (NullReferenceException e) {
			}
		}
	}
	
	private void setBuildings(int nBuildings, GameObject[] mBuildings) {
		// Se devo visualizzare solo l'hotel
		if (nBuildings == 5) {
			mBuildings[4].SetActiveRecursively(true);
		} else {
			// Altrimenti visualizzo solo le case che sono costruite
			for (int houseIndex = 0; houseIndex < nBuildings; houseIndex++) {
				// trovo la componente che ci serve
				mBuildings[houseIndex].SetActiveRecursively(true);
			}
		}
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
}
