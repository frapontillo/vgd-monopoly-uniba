using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {
	
	private bool render = true;
	private bool loading_finished = false;
	private GameObject cellPrefab = null;
	private Vector3 cameraPosition = new Vector3(0,0,0);
	private Vector3 cameraRotation = new Vector3(0,0,0);
	
	private GameObject terrain = null;
	private GameObject Titles = null;
	private GameObject Divisori = null;
	private GameObject Main_Title = null;
	private GameObject MiniCells = null;
	private GameObject[] Tokens = null;
	private GameObject camera = null;
	private GameObject buildings = null;
	
//	void Awake() {
//        DontDestroyOnLoad(gameObject);
//    }
	
	// Use this for initialization
	void Start () {
		terrain = GameObject.Find("Terrain");
		Titles = GameObject.Find("Titles");
		Divisori = GameObject.Find("Divisori");
		Main_Title = GameObject.Find("Main_Title");
		MiniCells = GameObject.Find("MiniCells");
		camera = GameObject.Find("camera");
	}
	
	private void disappear() {
		if (render) {
			// Fa scomparire gli oggetti
			terrain.SetActiveRecursively(false);
			Main_Title.SetActiveRecursively(false);
			Titles.SetActiveRecursively(false);
			Divisori.SetActiveRecursively(false);
			MiniCells.SetActiveRecursively(false);
			Tokens = GameObject.FindGameObjectsWithTag("Token");
			Tokens[0].SetActiveRecursively(false);
			Tokens[1].SetActiveRecursively(false);
			camera.GetComponent<AudioListener>().enabled = false;
		}
		
		render = false;
	}
	
	private void appear() {
		if (!render) {
			// Fa apparire gli oggetti
			terrain.SetActiveRecursively(true);
			Main_Title.SetActiveRecursively(true);
			Titles.SetActiveRecursively(true);
			Divisori.SetActiveRecursively(true);
			MiniCells.SetActiveRecursively(true);
			Tokens[0].SetActiveRecursively(true);
			Tokens[1].SetActiveRecursively(true);
			camera.GetComponent<AudioListener>().enabled = true;
		}
		
		render = true;
	}
	
	public void loadLevel(int LevelID) {
		// TODO: gestione del caricamento del livello
		// 
		// faccio scomparire la board
		disappear();
		loading_finished = false;
		// carico e posiziono il prefab
		string path = getPathByCell(LevelID);
		StartCoroutine(LoadObj(path));
	}
	
	IEnumerator LoadObj(string path)
	{
	    GameObject.Find("GUIManager").GetComponent<GUIManager>().setLabel("Caricamento cella in corso... Attendi.");
		// render another frame to make sure the label is drawn.
		yield return null;
	
	    Object prefabObj = Resources.Load(path);
	    cellPrefab = (GameObject)Instantiate(prefabObj);
	
	    loading_finished = true;
	}
	
	void Update() {
		if (loading_finished) {
		    // Avviso della fine del caricamento
			GameObject.Find("GUIManager").GetComponent<GUIManager>().setLabel("Esplora l'ambiente, ma non dimenticarti di giocare!");
			loading_finished = false;
			// Metto le case
			// TODO: ruotare oggetto in base a lato della board (0, 90, 180, 270)
			buildings = (GameObject)Instantiate(Resources.Load("LocalBuildings"),new Vector3(0,100,0), new Quaternion(0,0,0,0));
			buildings.tag = "LocalBuildingCompany";
		}
	}
	
	public void returnToBoard() {
		// distruggo il prefab
		Object.Destroy(cellPrefab);
		// distruggo gli edifici
		Object.Destroy(buildings);
		Resources.UnloadUnusedAssets();
		// faccio riapparire la board
		appear();
		// il turno ora e' finito
	}
	
	private string getPathByCell(int LevelID) {
		string path = "Cells/";
		
		switch (LevelID) {
		case (0):
			path += "00_Via/00_Via";
			break;
		case (1):
			path += "01_Libia/01_Libia";
			break;
		case (2):
			path += "02_Probabilita/02_Probabilita";
			break;
		case (3):
			path += "03_Nigeria/03_Nigeria";
			break;
		case (4):
			path += "04_Dogana/04_Dogana";
			break;
		case (5):
			path += "05_Aeroporto_Internazionale_Cairo/05_Aeroporto_Internazionale_Cairo";
			break;
		case (6):
			path += "06_Madagascar/06_Madagascar";
			break;
		case (7):
			path += "07_Imprevisti/07_Imprevisti";
			break;
		case (8):
			path += "08_Egypt/08_Egypt";
			break;
		case (9):
			path += "09_Sudafrica/09_Sudafrica";
			break;
		case (10):
			path += "10_Transito_Prigione/10_Transito_Prigione";
			break;
		case (11):
			path += "11_Italy/11_Italy";
			break;
		case (12):
			path += "12_Societa_Apple/12_Societa_Apple";
			break;
		case (13):
			path += "13_Spain/13_Spain";
			break;
		case (14):
			path += "14_France/14_France";
			break;
		case (15):
			path += "15_Aeroporto_Internazionale_Amsterdam/15_Aeroporto_Internazionale_Amsterdam";
			break;
		case (16):
			path += "16_Germany/16_Germany";
			break;
		case (17):
			path += "17_Probabilita/17_Probabilita";
			break;
		case (18):
			path += "18_UK/18_UK";
			break;
		case (19):
			path += "19_Iceland/19_Iceland";
			break;
		case (20):
			path += "20_Parcheggio_Gratuito/20_Parcheggio_Gratuito";
			break;
		case (21):
			path += "21_Brazil/21_Brazil";
			break;
		case (22):
			path += "22_Imprevisti/22_Imprevisti";
			break;
		case (23):
			path += "23_Colombia/23_Colombia";
			break;
		case (24):
			path += "24_Mexico/24_Mexico";
			break;
		case (25):
			path += "25_Aeroporto_Internazionale_Washington/25_Aeroporto_Internazionale_Washington";
			break;
		case (26):
			path += "26_USA_Washington/26_USA_Washington";
			break;
		case (27):
			path += "27_USA_NewYork/27_USA_NewYork";
			break;
		case (28):
			path += "28_Societa_Google/28_Societa_Google";
			break;
		case (29):
			path += "29_Canada/29_Canada";
			break;
		case (30):
			path += "30_In_Prigione/30_In_Prigione";
			break;
		case (31):
			path += "31_Russia/31_Russia";
			break;
		case (32):
			path += "32_India/32_India";
			break;
		case (33):
			path += "33_Probabilita/33_Probabilita";
			break;
		case (34):
			path += "34_China/34_China";
			break;
		case (35):
			path += "35_Aeroporto_Internazionale_Pechino/35_Aeroporto_Internazionale_Pechino";
			break;
		case (36):
			path += "36_Imprevisti/36_Imprevisti";
			break;
		case (37):
			path += "37_Japan/37_Japan";
			break;
		case (38):
			path += "38_Tassa_di_Lusso/38_Tassa_di_Lusso";
			break;
		case (39):
			path += "39_Australia/39_Australia";
			break;
		default:
			path = null;
			break;
		}
		
		return path;
	}
	
	private IEnumerator Wait(float Seconds)
    {
        yield return new WaitForSeconds(Seconds);
    }
}
