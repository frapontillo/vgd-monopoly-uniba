using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GUIManager : MonoBehaviour {
    public enum GUIMode {Main, RollDice, InTurn,
						OnMyCell, OnOtherCell, OnFreeCell, OnCantBuyCell, CantBuyLand,
						BuyLand, OnCanBuildHere, MyContracts, AllContracts,
						SellProperty, MortgageProperty, BuildOnProperty, GameOver, NoMenu};
    public GUIMode guiMode;
	public GUIMode prevMode;
	public GUIMode nestedMode;
    public bool endRound = false;
    public bool rollHuman = false;
    public bool rollComputer = false;
    public string label = "";
    public bool automaticSwitch = true;

    public List<Player> Players;
	public Player curPlayer;
	public Cell curCell;
    GUISkin BoxSkin;
    GUISkin PlayerNames;
    GUISkin PlayerMoney;
    GUISkin Buttons;
	GUISkin Amiga;
	int selGrid;
	int old;
	float vSbarValue;
	GUIStyle listStyle;
	
	GUIContent[] myContracts;
	GUIContent[] allContracts;
	Property selProperty;
	Vector2 scrollPosition;
	Texture contractTexture;
	bool showButtons = false;
	
	// float per memorizzare il prezzo da pagare (GUI)
	float curMoney = 0;
	// stringa per memorizzare il messaggio
	string buildingManagerMsg = "";
	// float che memorizza il numero di case da costruire
	int hSliderValue = 0;
	Texture buildingsTexture;
	
	// Riferimento allo script GameManager
	private GameManager gMgr;

    public void setLabel(string labelContent)
    {
        label = labelContent;
    }

    void OnGUI()
    {
        switch (guiMode)
        {
        case (GUIMode.Main):
            mainMenu();
            break;

        case (GUIMode.RollDice):
            mainMenu();
            if (rollHuman)
                rollDiceHumanMenu();
            else if (rollComputer)
                rollDiceComputerMenu();
            break;

        case (GUIMode.InTurn):
            mainMenu();
            endRoundMenu();
            break;
		
		case (GUIMode.OnMyCell):
			mainMenu();
			endRoundMenu();
			onCellMenu();
			onMyCellMenu();
			break;
			
		case (GUIMode.OnCanBuildHere):
			mainMenu();
			endRoundMenu();
			onCellMenu();
			onCanBuildHere();
			break;
		
		case (GUIMode.OnOtherCell):
			mainMenu();
			endRoundMenu();
			onCellMenu();
			onOtherCellMenu();
			break;
		
		case (GUIMode.OnFreeCell):
			mainMenu();
			endRoundMenu();
			onCellMenu();
			onFreeCellMenu();
			break;
			
		case (GUIMode.OnCantBuyCell):
			mainMenu();
			endRoundMenu();
			onCellMenu();
			onCantBuyCellMenu();
			break;
		
		case (GUIMode.BuyLand):
			mainMenu();
			onCellMenu();
			onBuyLandMenu();
			break;
			
		case (GUIMode.CantBuyLand):
			mainMenu();
			onCellMenu();
			onCantBuyLandMenu();
			break;
			
		// Mostro una lista dei miei contratti, con le possibili opzioni
		case (GUIMode.MyContracts):
			mainMenu();
			onMyContractsMenu();
			break;
			
		// Mostro una lista di tutti i contratti, con le possibili opzioni
		case (GUIMode.AllContracts):
			mainMenu();
			onAllContractsMenu();
			break;
			
		case (GUIMode.SellProperty):
			mainMenu();
			onSellProperty();
			break;
			
		case (GUIMode.MortgageProperty):
			mainMenu();
			onMortgageProperty();
			break;
			
		case (GUIMode.BuildOnProperty):
			mainMenu();
			onBuildOnProperty();
			break;
			
		case (GUIMode.GameOver):
            GameOver();
            break;

        default:
            break;
        }

    }
	
	void Awake() {
	}
	
	private void GameOver() {
		GUI.skin = Buttons;
        if (GUI.Button(new Rect((Screen.width - 160) / 2, 20, 160, 30), "Ritorna al menu"))
        {
            // TODO: caricare scena menu principale
			Application.LoadLevel("Menu");
        }
	}

    private void mainMenu()
    {
		GUI.skin = Buttons;
        GUI.Box(new Rect(10, 10, 170, 60), new GUIContent());
        GUI.Box(new Rect(10, Screen.height - 10 - 25, (Screen.width) * 1F - 30, 25), new GUIContent());
        // GUI.Box(new Rect(((Screen.width) * 0.8F) + 20, Screen.height - 10 - 25, (Screen.width) * 0.2F - 30, 25), new GUIContent());
		
        GUI.skin = PlayerNames;
        GUI.Label(new Rect(20, 15, 100, 25), firstPlayer().ToString());
        GUI.Label(new Rect(20, 40, 100, 25), secondPlayer().ToString());

        GUI.skin = PlayerMoney;
        GUI.Label(new Rect(120, 15, 50, 25), firstPlayer().getMoney().ToString());
        GUI.Label(new Rect(120, 40, 50, 25), secondPlayer().getMoney().ToString());

        GUI.skin = Buttons;
        GUI.Label(new Rect(15, Screen.height - 10 - 25, (Screen.width) * 1F, 25), label);
//        GUI.Label(new Rect(((Screen.width) * 0.8F) + 25, Screen.height - 10 - 25, (Screen.width) * 0.2F - 30, 25), "Tempo rimanente");

    }

    private void endRoundMenu()
    {
        if (rollHuman)
        {
            GUI.skin = Buttons;
            if (GUI.Button(new Rect((Screen.width - 160) / 2, 20, 160, 30), "Termina il turno!"))
            {
                rollHuman = false;
				GameObject.Find("GameManager").GetComponent<GameManager>().SendMessage("turnOver");
            }
        }
        else
        {
            
        }
        
    }

    private void rollDiceHumanMenu()
    {
        GUI.skin = Buttons;
        if (GUI.Button(new Rect((Screen.width - 160) / 2, 20, 160, 30), "Tira i dadi!"))
        {
            // Torno nel main
            guiMode = GUIMode.Main;
			GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
			// UNDONE: se non siamo in debug, avvia il lancio dei dadi
			// se siamo in debug, muove la pedina sempre e solo di 1
			if (!(gameManager.debug)) {
	            // Imposto la label e avvio il movimento della camera
	            setLabel("Scalda le mani...");
	            MoveCamera scriptCamera = GameObject.Find("camera").GetComponent<MoveCamera>();
	            scriptCamera.moveCameraForDice();
			} else {
				gameManager.curPlayer.moveTokenBy(1, gameManager.Cells);
			}
        }
    }

    private void rollDiceComputerMenu()
    {
        // Avvio il movimento della camera
        MoveCamera scriptCamera = GameObject.Find("camera").GetComponent<MoveCamera>();
        scriptCamera.moveCameraForDice();
        // Torno nel main
        guiMode = GUIMode.Main;
    }
	
	private void onCellMenu() {
		GUI.skin = Buttons;
		
        if (GUI.Button(new Rect(Screen.width - 160, 10, 150, 25), "Miei Contratti")) {
			prevMode = guiMode;
			guiMode = GUIMode.MyContracts;
		}
		if (GUI.Button(new Rect(Screen.width - 160, 40, 150, 25), "Tutti i Contratti")) {
			prevMode = guiMode;
			guiMode = GUIMode.AllContracts;
		}
		// TODO: eliminare commento e gestire carte e aste
//      if (GUI.Button(new Rect(Screen.width - 160, 70, 150, 25), "Mie Carte")) {
//		}
//      if (GUI.Button(new Rect(Screen.width - 160, 100, 150, 25), "Aste")) {
//		}
	}
	
	private void onMyCellMenu() {
		
		// TODO: eliminare commento e gestire vendita e ipoteca
		
//		  GUI.skin = Buttons;
//        GUI.Button(new Rect(10, 80, 150, 25), "Vendi");
//        GUI.Button(new Rect(10, 110, 150, 25), "Ipoteca");
	}
	
	private void onCanBuildHere() {
		GUI.skin = Buttons;
		if (GUI.Button(new Rect(10, 80, 150, 25), "Gestisci edifici")) {
			// imposto l'hSliderValue al numero di case
			hSliderValue = curPlayer.getCurrentCell().getProperty().getNumberOfBuildings();
			buildHere();
		}
		// TODO: eliminare commento e gestire vendita e ipoteca
//      GUI.Button(new Rect(10, 110, 150, 25), "Vendi");
//      GUI.Button(new Rect(10, 140, 150, 25), "Ipoteca");
	}
	
	private void onOtherCellMenu() {
		GUI.skin = Buttons;
	}
	
	private void onFreeCellMenu() {
		GUI.skin = Buttons;
		
        if (GUI.Button(new Rect(10, 80, 150, 25), "Compra!")) {
			guiMode = GUIManager.GUIMode.BuyLand;
		}
	}
	
	private void onCantBuyCellMenu() {
		GUI.skin = Buttons;
		
        if (GUI.Button(new Rect(10, 80, 150, 25), "Compra!")) {
			guiMode = GUIManager.GUIMode.CantBuyLand;
		}
	}
	
	private void onCantBuyLandMenu() {
		// Creo la box
		GUI.skin = Buttons;
		float boxWidth = 300;
		float boxHeight = 150;
    	GUI.Box(new Rect((Screen.width/2)-(boxWidth/2), (Screen.height/2)-(boxHeight/2), boxWidth, boxHeight), new GUIContent());
		
		// Creo la label
		GUI.Label(new Rect((Screen.width/2)-(boxWidth/2)+10, (Screen.height/2)-(boxHeight/2)+10, boxWidth-20, 50), "Attenzione! Non puoi comprare questo terreno.");
		GUI.Label(new Rect((Screen.width/2)-(boxWidth/2)+10, (Screen.height/2)-(boxHeight/2)+60, boxWidth-20, 50), "Possiedi solo " + gMgr.curPlayer.getMoney() + "M, te ne servono " + curCell.getProperty().getPropertyCost() + "M.");
		
		// Creo i pulsanti
		GUI.skin = Buttons;
		float buttonWidth = 135;
		float buttonHeight = 25;
		
		// Al click su OK
		if (GUI.Button(new Rect((Screen.width/2)-(boxWidth/2)+20+(buttonWidth/2), (Screen.height/2)+(boxHeight/2)-40, buttonWidth, buttonHeight), "OK")) {
			// Si ripristina lo stato della GUI a OnCantBuyCell
			guiMode = GUIManager.GUIMode.OnCantBuyCell;
		}
	}
	
	private void onBuyLandMenu() {
		// Creo la box
		GUI.skin = Buttons;
		float boxWidth = 300;
		float boxHeight = 150;
    	GUI.Box(new Rect((Screen.width/2)-(boxWidth/2), (Screen.height/2)-(boxHeight/2), boxWidth, boxHeight), new GUIContent());
		
		// Creo le label
		GUI.Label(new Rect((Screen.width/2)-(boxWidth/2)+10, (Screen.height/2)-(boxHeight/2)+10, boxWidth-20, 50), "Sei sicuro di voler comprare questo terreno?");
		GUI.Label(new Rect((Screen.width/2)-(boxWidth/2)+10, (Screen.height/2)-(boxHeight/2)+60, boxWidth-20, 50), "Il costo sara' di: " + curCell.getProperty().getPropertyCost() + "M.");
		
		// Creo i pulsanti
		GUI.skin = Buttons;
		float buttonWidth = 135;
		float buttonHeight = 25;
		// Al click su OK
		if (GUI.Button(new Rect((Screen.width/2)-(boxWidth/2)+10, (Screen.height/2)+(boxHeight/2)-40, buttonWidth, buttonHeight), "OK")) {
			// Il giocatore compra il terreno
			gMgr.buyLand();
		}
		// Al click su Annulla
		if (GUI.Button(new Rect((Screen.width/2)-(boxWidth/2)+20+buttonWidth, (Screen.height/2)+(boxHeight/2)-40, buttonWidth, buttonHeight), "Annulla")) {
			// Si ripristina lo stato della GUI a OnFreeCell
			guiMode = GUIManager.GUIMode.OnFreeCell;
		}
	}
	
	private GUIContent[] getMyContractsList() {
		// Creo il GUIContent
		List<Property> propertyList = curPlayer.getPropertyList();
		GUIContent[] contractsList = new GUIContent[propertyList.Count];
		int i;
		for (i = 0; i < propertyList.Count; i++) {
			contractsList[i] = new GUIContent(propertyList[i].getName());
		}
		// Ho la lista dei contratti
		return contractsList;
	}
	
	private void onMyContractsMenu() {
		
		// Creo la box
		GUI.skin = Buttons;
		// Dimensione box Contratti
		float boxWidth = Screen.width*70/100;
		float boxHeight = Screen.height*50/100;
    	GUI.Box(new Rect((Screen.width/2)-(boxWidth/2), (Screen.height/2)-(boxHeight/2), boxWidth, boxHeight), new GUIContent());
		
		// Creo le label
		GUI.Label(new Rect((Screen.width/2)-(boxWidth/2)+10, (Screen.height/2)-(boxHeight/2)+10, boxWidth-20, 50), "Seleziona una carta dalla seguente lista. Poi, scegli una fra le azioni disponibili.");
		
		// Creo la lista
		GUI.skin = Buttons;
		myContracts = getMyContractsList();
		Rect ListRect = new Rect((Screen.width/2)-(boxWidth/2)+10, (Screen.height/2)-(boxHeight/2)+40, 200, myContracts.Length*35);
		scrollPosition = GUI.BeginScrollView(new Rect((Screen.width/2)-(boxWidth/2)+10, (Screen.height/2)-(boxHeight/2)+40, 220, Screen.width*20/100), scrollPosition, ListRect);
		selGrid = GUI.SelectionGrid(ListRect, selGrid, myContracts, 1);
		GUI.EndScrollView();
		
		float space1 = ((Screen.width - boxWidth)/2) - 10 + 220 + ((boxWidth - 220 - 220 - 200)/2);
		float space2 = space1 + 220 + ((boxWidth - 220 - 220 - 200)/2);
		
		// Creo la Texture2D che conterrà l'immagine del contratto
		if (contractTexture) GUI.DrawTexture(new Rect(space1, (Screen.height/2)-(boxHeight/2)+40, 220, 260), contractTexture, ScaleMode.StretchToFill, true, 0);
		
		// Visualizzo i pulsanti appositi per il contratto
		if ( (showButtons && contractTexture != null) &&
			(gMgr.Properties[selGrid].getOwner().canBuild(selProperty)) ) {
			// TODO: rimuovere commento e gestire case, vendita e ipoteca
//			if (GUI.Button(new Rect(space2, (Screen.height/2)-(boxHeight/2)+40, 200, 35),"Costruisci qui!")) {
//				// imposto l'hSliderValue al numero di case
//				hSliderValue = selProperty.getNumberOfBuildings();
//				buildHere();
//			}
//			if (GUI.Button(new Rect(space2, (Screen.height/2)-(boxHeight/2)+80, 200, 35),"Vendi proprieta'")) {
//				Debug.Log("Click vendi proprieta'");
//			}
//			if (GUI.Button(new Rect(space2, (Screen.height/2)-(boxHeight/2)+120, 200, 35),"Ipoteca proprieta'")) {
//				Debug.Log("Click ipoteca proprieta'");
//			}
		}
		
		// Creo il pulsante di uscita
		GUI.skin = Buttons;
		float buttonWidth = 100;
		float buttonHeight = 35;
		if (GUI.Button(new Rect(space2 + 100, (Screen.height/2)+(boxHeight/2)-40, buttonWidth, buttonHeight), "Esci")) {
			// resetto le variabili che possono servire dopo
			contractTexture = null;
			showButtons = false;
			old = -2;
			selGrid = -1;
			// Si ritorna allo stato precedente
			guiMode = prevMode;
		}
	}
	
	private GUIContent[] getAllContractsList() {
		// Creo il GUIContent
		List<Property> propertyList = gMgr.Properties;
		GUIContent[] contractsList = new GUIContent[propertyList.Count];
		int i;
		for (i = 0; i < propertyList.Count; i++) {
			contractsList[i] = new GUIContent(propertyList[i].getName());
		}
		// Ho la lista dei contratti
		return contractsList;
	}
	
	private void onAllContractsMenu() {
		
		// Creo la box
		GUI.skin = Buttons;
		// Dimensione box Contratti
		float boxWidth = Screen.width*70/100;
		float boxHeight = Screen.height*50/100;
    	GUI.Box(new Rect((Screen.width/2)-(boxWidth/2), (Screen.height/2)-(boxHeight/2), boxWidth, boxHeight), new GUIContent());
		
		// Creo le label
		GUI.Label(new Rect((Screen.width/2)-(boxWidth/2)+10, (Screen.height/2)-(boxHeight/2)+10, boxWidth-20, 50), "Seleziona una carta dalla seguente lista..");
		
		// Creo la lista
		GUI.skin = Buttons;
		if (allContracts == null) allContracts = getAllContractsList();
		Rect ListRect = new Rect((Screen.width/2)-(boxWidth/2)+10, (Screen.height/2)-(boxHeight/2)+40, 200, allContracts.Length*35);
		scrollPosition = GUI.BeginScrollView(new Rect((Screen.width/2)-(boxWidth/2)+10, (Screen.height/2)-(boxHeight/2)+40, 220, Screen.width*20/100), scrollPosition, ListRect);
		selGrid = GUI.SelectionGrid(ListRect, selGrid, allContracts, 1);
		GUI.EndScrollView();
		
		float space1 = ((Screen.width - boxWidth)/2) - 10 + 220 + ((boxWidth - 220 - 220 - 200)/2);
		float space2 = space1 + 220 + ((boxWidth - 220 - 220 - 200)/2);
		
		// Creo la Texture2D che conterrà l'immagine del contratto
		if (contractTexture) GUI.DrawTexture(new Rect(space1, (Screen.height/2)-(boxHeight/2)+40, 220, 260), contractTexture, ScaleMode.StretchToFill, true, 0);
		
		// Visualizzo i pulsanti appositi per il contratto
		if (showButtons && contractTexture != null) {
			// Controllo di chi e' la proprieta'
			// se la proprieta' e' mia, posso vendera, ipotecarla o costruirci sopra
			if ( (gMgr.Properties[selGrid].getOwner().getPlayerType() == Player.PLAYER_TYPE.HUMAN) && 
			    (gMgr.Properties[selGrid].getOwner().canBuild(selProperty)) ) {
				// TODO: rimuovere commento e gestire case, vendita e ipoteca
//				if (GUI.Button(new Rect(space2, (Screen.height/2)-(boxHeight/2)+40, 200, 35),"Costruisci qui!")) {
//					// imposto l'hSliderValue al numero di case
//					hSliderValue = selProperty.getNumberOfBuildings();
//					buildHere();
//				}
	//			if (GUI.Button(new Rect(space2, (Screen.height/2)-(boxHeight/2)+80, 200, 35),"Vendi proprieta'")) {
	//				Debug.Log("Click vendi proprieta'");
	//			}
	//			if (GUI.Button(new Rect(space2, (Screen.height/2)-(boxHeight/2)+120, 200, 35),"Ipoteca proprieta'")) {
	//				Debug.Log("Click ipoteca proprieta'");
	//			}
			} else if (gMgr.Properties[selGrid].getOwner().getPlayerType() == Player.PLAYER_TYPE.COMPUTER) {
			} else if (gMgr.Properties[selGrid].getOwner().getPlayerType() == Player.PLAYER_TYPE.BANK) {
			}
		}
		
		// Creo il pulsante di uscita
		GUI.skin = Buttons;
		float buttonWidth = 100;
		float buttonHeight = 35;
		if (GUI.Button(new Rect(space2 + 100, (Screen.height/2)+(boxHeight/2)-40, buttonWidth, buttonHeight), "Esci")) {
			// resetto le variabili che possono servire dopo
			contractTexture = null;
			showButtons = false;
			old = -2;
			selGrid = -1;
			// Si ritorna allo stato precedente
			guiMode = prevMode;
		}
	}
	
	private void onSellProperty() {
		// TODO: gestire vendita
	}
	
	private void onMortgageProperty() {
		// TODO: gestire ipoteca
	}
	
	private void onBuildOnProperty() {
		
		// Creo la box
		GUI.skin = Buttons;
		float boxWidth = 300;
		float boxHeight = 200;
    	GUI.Box(new Rect((Screen.width/2)-(boxWidth/2), (Screen.height/2)-(boxHeight/2), boxWidth, boxHeight), new GUIContent());
		
		// Creo le label
		GUI.Label(new Rect((Screen.width/2)-(boxWidth/2)+10, (Screen.height/2)-(boxHeight/2)+10, boxWidth-20, 25), "Muovi lo slider.");
		GUI.Label(new Rect((Screen.width/2)-(boxWidth/2)+10, (Screen.height/2)-(boxHeight/2)+35, boxWidth-20, 25), buildingManagerMsg);
		
		// Creo i pulsanti
		GUI.skin = Buttons;
		float buttonWidth = 135;
		float buttonHeight = 25;
		// Al click su OK
		if (GUI.Button(new Rect((Screen.width/2)-(boxWidth/2)+10, (Screen.height/2)+(boxHeight/2)-40, buttonWidth, buttonHeight), "OK")) {
			// TODO: Il giocatore compra le case
			// controllo sul numero di edifici gia' costruiti
			int actBuildings = curPlayer.getCurrentCell().getProperty().getNumberOfBuildings();
			LocalBuildingCompany bldCpny = GameObject.FindGameObjectWithTag("LocalBuildingCompany").GetComponent<LocalBuildingCompany>();
			// VENDITA EDIFICI
			if (hSliderValue < actBuildings) {
				// Procedo alla vendita
				curPlayer.sellBuildings(curPlayer.getCurrentCell().getProperty(), actBuildings - hSliderValue);
				// Faccio alzare le case dalla cella dettagliata
				bldCpny.higher(actBuildings - hSliderValue);
			}
			// ACQUISTO EDIFICI
			else if (hSliderValue > actBuildings) {
				// Procedo all'acquisto
				curPlayer.build(curPlayer.getCurrentCell().getProperty(), hSliderValue - actBuildings);
				// Faccio cadere le case sulla cella dettagliata
				bldCpny.lower(hSliderValue - actBuildings);
				// Faccio cadere le case sulla cella della board
				// TODO
				
			}
			Debug.Log(bldCpny.buildings + " case.");
			// Si ripristina lo stato della GUI
			guiMode = nestedMode;
		}
		// Al click su Annulla
		if (GUI.Button(new Rect((Screen.width/2)-(boxWidth/2)+20+buttonWidth, (Screen.height/2)+(boxHeight/2)-40, buttonWidth, buttonHeight), "Annulla")) {
			// Si ripristina lo stato della GUI
			guiMode = nestedMode;
		}
		
		// Creo lo slider
		hSliderValue = (int)GUI.HorizontalSlider(new Rect((Screen.width/2)-(boxWidth/2)+10, (Screen.height/2)-(boxHeight/2)+120, boxWidth-20, 50), hSliderValue, 0F, 5F);
		
		// Creo la texture
		if (buildingsTexture) GUI.DrawTexture(new Rect((Screen.width/2)-(boxWidth/2)+10, (Screen.height/2)-(boxHeight/2)+60, boxWidth-20, 70), buildingsTexture, ScaleMode.StretchToFill, true, 0);
	}
	
	private void buildHere() {
		// salvo lo stato della gui prima di annidarmi
		nestedMode = guiMode;
		// cambio lo stato
		guiMode = GUIManager.GUIMode.BuildOnProperty;
	}
	
    private Player firstPlayer()
    {
        if (Players[0].getMoney() > Players[1].getMoney())
            return Players[0];
        else return Players[1];
    }

    private Player secondPlayer()
    {
        if (Players[0].getMoney() <= Players[1].getMoney())
            return Players[0];
        else return Players[1];
    }

	// Use this for initialization
    void Start()
    {
        guiMode = GUIMode.Main;
		
		// Inizializzo le GUISkin
        BoxSkin = null;
        PlayerNames = (GUISkin)Instantiate(Resources.Load("GUISkins/PlayerNames", typeof(GUISkin)));
        PlayerMoney = (GUISkin)Instantiate(Resources.Load("GUISkins/PlayerMoney", typeof(GUISkin)));
        Buttons = (GUISkin)Instantiate(Resources.Load("Extra GUI Skins/MetalGUISkin", typeof(GUISkin)));
		Amiga = (GUISkin)Instantiate(Resources.Load("Extra GUI Skins/Amiga500GUISkin", typeof(GUISkin)));
    	
		// Cerco il riferimento al GameManager della partita
		gMgr = GameObject.Find("GameManager").GetComponent<GameManager>();
		
		// setto i contratti a null
		myContracts = null;
		allContracts = null;
		
		// Creo lo stile della lista
		// Make a GUIStyle that has a solid white hover/onHover background to indicate highlighted items
	    listStyle = new GUIStyle();
		
	    Texture2D normalTex = new Texture2D(2, 2);
	    Color[] colors = new Color[4];
		
	    listStyle.padding.left = listStyle.padding.right = listStyle.padding.top = listStyle.padding.bottom = 4;
		scrollPosition = Vector2.zero;
		selGrid = -1;
		old = -2;
		contractTexture = null;
		buildingsTexture = null;
	}
	
	// Update is called once per frame
	void Update () {
		// se ci troviamo nelle zone interessate
		if ((guiMode == GUIMode.MyContracts) || (guiMode == GUIMode.AllContracts)) {
			// Se e' cambiata la selezione del contratto
			if (old != selGrid) {
				Debug.Log(selGrid);
				// Cambio la Texture del contratto
				int ID = selectionChanged();
				old = selGrid;
				showButtons = true;
			}
		} else if (guiMode == GUIMode.BuildOnProperty) {
			// Cambio la texture dei buildings
			buildingsTexture = (Texture)Resources.Load("BuildingTextures/" +  hSliderValue.ToString(), typeof(Texture));
			// Ottengo il numero di edifici gia' costruiti
			int actBuildings = curPlayer.getCurrentCell().getProperty().getNumberOfBuildings();
			if (hSliderValue < actBuildings) {
				buildingManagerMsg = "Guadagni " + curPlayer.getCurrentCell().getProperty().getGroup().estimateBuildingCost(actBuildings - hSliderValue) / 2 + "M.";
			} else if (hSliderValue > actBuildings) {
				buildingManagerMsg = "Paghi " + curPlayer.getCurrentCell().getProperty().getGroup().estimateBuildingCost(hSliderValue - actBuildings) + "M.";
			} else {
				buildingManagerMsg = "A destra compri, a sinistra vendi.";
			}
		}
	}
	
	private int selectionChanged() {
		int ID = -1;
		// Se ci troviamo in fase di selezione contratto
		if (guiMode == GUIMode.MyContracts) {
			// Ottengo l'ID della proprieta' relativa
			List<Property> propertyList = curPlayer.getPropertyList();
			if (selGrid >= 0) {
				ID = propertyList[selGrid].getCell().getPosition();
				selProperty = propertyList[selGrid].getCell().getProperty();
				// Cambio la texture
				contractTexture = (Texture)Resources.Load("Property Cards/JPGs/" +  ID + "_A", typeof(Texture));
			}
		} else if (guiMode == GUIMode.AllContracts) {
			// Ottengo l'ID della proprieta' relativa
			List<Property> propertyList = gMgr.Properties;
			if (selGrid >= 0) {
				ID = propertyList[selGrid].getCell().getPosition();
				selProperty = propertyList[selGrid].getCell().getProperty();
				// Cambio la texture
				contractTexture = (Texture)Resources.Load("Property Cards/JPGs/" +  ID + "_A", typeof(Texture));
			}
		}
		return ID;
	}

    private IEnumerator Wait(float Seconds) {
        yield return new WaitForSeconds(Seconds);
    }

}
