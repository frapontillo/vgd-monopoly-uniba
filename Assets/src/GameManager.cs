using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/** Classe che si occupa di gestire la partita.
 * 
 * */
public class GameManager : MonoBehaviour {
	
	// Variabile booleana, se true e' bello.
    public bool debug = false;
    public List<Cell> Cells = new List<Cell>();
    public List<Group> Groups = new List<Group>();
    public List<Property> Properties = new List<Property>();
    public List<Player> Players = new List<Player>();

    public enum GAME_STATUS { START, ROLL_FOR_ORDER, START_TURN, IN_TURN, TURN_OVER };
    public GAME_STATUS gameStatus = GAME_STATUS.START;
    
    // Valori per l'ordine di partenza
    int humanRoll = 0;
    int computerRoll = 0;
    // Player attualmente in uso
    public Player curPlayer;
    // Valore che specifica il Player che deve lanciare i dadi
    int turn = -1;
    // Numero del personaggio scelto
    int human = -1;
    int computer = -1;

    const int VALORE_TASSA_1 = 20;
    const int VALORE_TASSA_2 = 40;

    string statusLabel;
    bool rollDice;
	
	bool ritira = false;
	bool playersSet = false;
	Player avversario;
	GameObject Human;
	GameObject Computer;
	// Riferimento al GUIManager corrente
	private GUIManager guiMgr;

    void Awake()
    {
		// Ottengo il riferimento al GUIManager corrente
		guiMgr = GameObject.Find("GUIManager").GetComponent<GUIManager>();
		
        // Ottengo l'ID del personaggio scelto
        if (GameObject.Find("character_selection") != null)
            human = GameObject.Find("character_selection").GetComponent<CharacterSelection>().personaggio;
        // Se sto testando questa scena, l'human sara' 3 di default
        else human = 3;

        // Ottengo le variabili che mi servono
        statusLabel = guiMgr.label;
    }

	// Use this for initialization
	void Start ()
    {
        Debug.Log(debug);
        // INIZIO GIOCO E DISTRIBUZIONE SOLDI E CONTRATTI
        createGame();
        positionPlayers();
        // DECISIONE ORDINE
        setLabel("Benvenuto in Monopoly! Tira i dadi per decidere l'ordine di partenza!");
        playersOrder();
		
        // DA QUI IN POI ALTRE FUNZIONI, CHIAMATE DI SEGUITO, SI OCCUPERANNO DELLA GESTIONE DEL GIOCO
    }

    // Update is called once per frame
    void Update ()
    {
    }

    public GameManager () {
        // Debug.Log("GameManager just got created!");
	}

    public void createGame()
    {
        // PARTE 0
        // Creo le Cell
        Cells.Add(new Cell(0, Cell.CELL_TYPE.VIA));
        Cells.Add(new Cell(1, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(2, Cell.CELL_TYPE.PROBABILITA));
        Cells.Add(new Cell(3, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(4, Cell.CELL_TYPE.TASSA, VALORE_TASSA_1));
        Cells.Add(new Cell(5, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(6, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(7, Cell.CELL_TYPE.IMPREVISTO));
        Cells.Add(new Cell(8, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(9, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(10, Cell.CELL_TYPE.TRANSITO));
        Cells.Add(new Cell(11, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(12, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(13, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(14, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(15, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(16, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(17, Cell.CELL_TYPE.PROBABILITA));
        Cells.Add(new Cell(18, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(19, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(20, Cell.CELL_TYPE.POSTEGGIO));
        Cells.Add(new Cell(21, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(22, Cell.CELL_TYPE.IMPREVISTO));
        Cells.Add(new Cell(23, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(24, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(25, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(26, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(27, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(28, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(29, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(30, Cell.CELL_TYPE.VAI_PRIGIONE));
        Cells.Add(new Cell(31, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(32, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(33, Cell.CELL_TYPE.PROBABILITA));
        Cells.Add(new Cell(34, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(35, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(36, Cell.CELL_TYPE.IMPREVISTO));
        Cells.Add(new Cell(37, Cell.CELL_TYPE.PROPRIETA));
        Cells.Add(new Cell(38, Cell.CELL_TYPE.TASSA, VALORE_TASSA_2));
        Cells.Add(new Cell(39, Cell.CELL_TYPE.PROPRIETA));
	    // Creo i Gruppi
        Groups.Add(new Group("0", "Purple", new Color(202,0,202)));
        Groups.Add(new Group("1", "Blue", new Color(0, 102, 204)));
        Groups.Add(new Group("2", "Orange", new Color(255, 102, 0)));
        Groups.Add(new Group("3", "Brown", new Color(98, 41, 0)));
        Groups.Add(new Group("4", "Red", new Color(255, 0, 0)));
        Groups.Add(new Group("5", "Yellow", new Color(255, 255, 0)));
        Groups.Add(new Group("6", "Green", new Color(0, 51, 0)));
        Groups.Add(new Group("7", "Violet", new Color(153, 0, 255)));
        Groups.Add(new Group("Airports", "NONE", new Color(0, 0, 0)));
        Groups.Add(new Group("Societies", "NONE", new Color(0, 0, 0)));
        // Creo le Proprieta'
        // CITTA'
        Properties.Add(new Property(Cells[1], "Tripoli", Groups[0], new float[] { 60, 50, 50 }, new float[] { 2, 10, 30, 90, 160, 250 }, Property.PROPERTY_TYPE.CITY));
        Properties.Add(new Property(Cells[3], "Sahara", Groups[0], new float[] { 60, 50, 50 }, new float[] { 4, 20, 60, 180, 320, 450 }, Property.PROPERTY_TYPE.CITY));
        Properties.Add(new Property(Cells[6], "Madagascar", Groups[1], new float[] { 100, 50, 50 }, new float[] { 6, 30, 90, 270, 400, 550 }, Property.PROPERTY_TYPE.CITY));
        Properties.Add(new Property(Cells[8], "Egitto", Groups[1], new float[] { 100, 50, 50 }, new float[] { 6, 30, 90, 270, 400, 550 }, Property.PROPERTY_TYPE.CITY));
        Properties.Add(new Property(Cells[9], "Citta' del Capo", Groups[1], new float[] { 120, 50, 50 }, new float[] { 8, 40, 100, 300, 450, 600 }, Property.PROPERTY_TYPE.CITY));
        Properties.Add(new Property(Cells[11], "Roma", Groups[2], new float[] { 140, 100, 100 }, new float[] { 10, 50, 150, 450, 625, 750 }, Property.PROPERTY_TYPE.CITY));
        Properties.Add(new Property(Cells[13], "Barcellona", Groups[2], new float[] { 140, 100, 100 }, new float[] { 10, 50, 150, 450, 625, 750 }, Property.PROPERTY_TYPE.CITY));
        Properties.Add(new Property(Cells[14], "Parigi", Groups[2], new float[] { 160, 100, 100 }, new float[] { 12, 60, 180, 500, 700, 900 }, Property.PROPERTY_TYPE.CITY));
        Properties.Add(new Property(Cells[16], "Berlino", Groups[3], new float[] { 180, 100, 100 }, new float[] { 14, 70, 200, 550, 750, 950 }, Property.PROPERTY_TYPE.CITY));
        Properties.Add(new Property(Cells[18], "Londra", Groups[3], new float[] { 180, 100, 100 }, new float[] { 14, 70, 200, 550, 750, 950 }, Property.PROPERTY_TYPE.CITY));
        Properties.Add(new Property(Cells[19], "Ghiacciaio Islandese", Groups[3], new float[] { 200, 100, 100 }, new float[] { 16, 80, 220, 600, 800, 1000 }, Property.PROPERTY_TYPE.CITY));
        Properties.Add(new Property(Cells[21], "Rio de Janeiro", Groups[4], new float[] { 220, 150, 150 }, new float[] { 18, 90, 250, 700, 875, 1050 }, Property.PROPERTY_TYPE.CITY));
        Properties.Add(new Property(Cells[23], "Piantagione Colombiana", Groups[4], new float[] { 220, 150, 150 }, new float[] { 18, 90, 250, 700, 875, 1050 }, Property.PROPERTY_TYPE.CITY));
        Properties.Add(new Property(Cells[24], "Piramidi Messicane", Groups[4], new float[] { 240, 150, 150 }, new float[] { 20, 100, 300, 750, 925, 1100 }, Property.PROPERTY_TYPE.CITY));
        Properties.Add(new Property(Cells[26], "Washington", Groups[5], new float[] { 260, 150, 150 }, new float[] {22, 110, 330, 800, 975, 1150 }, Property.PROPERTY_TYPE.CITY));
        Properties.Add(new Property(Cells[27], "New York", Groups[5], new float[] { 260, 150, 150 }, new float[] { 22, 110, 330, 800, 975, 1150 }, Property.PROPERTY_TYPE.CITY));
        Properties.Add(new Property(Cells[29], "Montreal", Groups[5], new float[] { 280, 150, 150 }, new float[] { 24, 120, 360, 850, 1025, 1200 }, Property.PROPERTY_TYPE.CITY));
        Properties.Add(new Property(Cells[31], "Mosca", Groups[6], new float[] { 300, 200, 200 }, new float[] { 26, 130, 390, 900, 1100, 1275 }, Property.PROPERTY_TYPE.CITY));
        Properties.Add(new Property(Cells[32], "New Dehli", Groups[6], new float[] { 300, 200, 200 }, new float[] { 26, 130, 390, 900, 1100, 1275 }, Property.PROPERTY_TYPE.CITY));
        Properties.Add(new Property(Cells[34], "Pechino", Groups[6], new float[] { 320, 200, 200 }, new float[] { 28, 150, 450, 1000, 1200, 1400 }, Property.PROPERTY_TYPE.CITY));
        Properties.Add(new Property(Cells[37], "Tokyo", Groups[7], new float[] { 350, 200, 200 }, new float[] { 35, 175, 500, 1100, 1300, 1500 }, Property.PROPERTY_TYPE.CITY));
        Properties.Add(new Property(Cells[39], "Sydney", Groups[7], new float[] { 400, 200, 200 }, new float[] { 50, 200, 600, 1400, 1700, 2000 }, Property.PROPERTY_TYPE.CITY));
        // AEROPORTI
        Properties.Add(new Property(Cells[5], "Aeroporto del Cairo", Groups[8], new float[] { 200 }, new float[] { 25, 50, 100, 200 }, Property.PROPERTY_TYPE.AIRPORT));
        Properties.Add(new Property(Cells[15], "Aeroporto di Amsterdam", Groups[8], new float[] { 200 }, new float[] { 25, 50, 100, 200 }, Property.PROPERTY_TYPE.AIRPORT));
        Properties.Add(new Property(Cells[25], "Aeroporto di Washington", Groups[8], new float[] { 200 }, new float[] { 25, 50, 100, 200 }, Property.PROPERTY_TYPE.AIRPORT));
        Properties.Add(new Property(Cells[35], "Aeroporto di Pechino", Groups[8], new float[] { 200 }, new float[] { 25, 50, 100, 200 }, Property.PROPERTY_TYPE.AIRPORT));
        // SOCIETA'
        Properties.Add(new Property(Cells[12], "Apple", Groups[9], new float[] { 150 }, new float[] { 20, 100 }, Property.PROPERTY_TYPE.SOCIETY));
        Properties.Add(new Property(Cells[28], "Google", Groups[9], new float[] { 150 }, new float[] { 20, 100 }, Property.PROPERTY_TYPE.SOCIETY));

        guiMgr.Players = Players;
        // TODO: Creo le carte di Imprevisti e Probabilita'
        // LE CARTE DI IMPREVISTI O PROBABILITA' LE FACCIAMO CREARE A RUN-TIME, IN MODO TALE CHE SIANO DIVERSE OGNI VOLTA!!!
		
		// Faccio apparire solo le case effettivamente costruite
		updateAllBuildings();
    }

    public void positionPlayers()
    {
        // Creo una lista di interi contenente (numero pedine - 1) valori, dai quali estrarre
        // un intero rappresentante la pedina del computer
        List<int> tokenGenerator = new List<int>();
        for (int i = 0; i < 6; i++)
        {
            if (i != human)
                tokenGenerator.Add(i);
        }
        // Genero un numero random da 0 a 4 e lo utilizzo come indice dell'array
        computer = tokenGenerator[Random.Range(0, 4)];
        //Debug.Log("Pedina human #" + human);
        //Debug.Log("Pedina computer #" + computer);
		
		// Soldi da distribuire per ogni giocatore
		float Money = 0;
		
		// UNDONE: se sono in debug, faccio una partita "corta" e distribuisco meno soldi
		if (debug) Money = 2000;
		else Money = 2000;
		
		// Creo i Giocatori
        Players.Add(new Player(Player.PLAYER_TYPE.HUMAN, human, Money, Properties));
        Players.Add(new Player(Player.PLAYER_TYPE.COMPUTER, computer, Money, Properties));
        Players.Add(new Player(Player.PLAYER_TYPE.BANK, -1, 100000000, Properties));
		
		// Ciclo su tutte le proprieta'
		int cont = 0;
		for (cont = 0; cont < Properties.Count; cont++) {
			// alle proprieta' con Owner == null
			if (!(Properties[cont].isOwned())) {
				// imposto come proprietario la banca
				Properties[cont].setOwner(Players[2]);
			}
			Debug.Log("Proprieta' " + Properties[cont].getName() + ": " + Properties[cont].getOwner().ToString());
		}
		
        // Creo le pedine
        Human = (GameObject)Instantiate(Resources.Load("Tokens/3D Tokens/Original Tokens/" + human.ToString(), typeof(GameObject)));
        Computer = (GameObject)Instantiate(Resources.Load("Tokens/3D Tokens/Original Tokens/" + computer.ToString(), typeof(GameObject)));
        Computer.tag = "Token";
		Human.tag = "Token";

        // Assegno lo script alle pedine
        Human.gameObject.AddComponent("PlayerMover");
        Computer.gameObject.AddComponent("PlayerMover");
        // Assegno i players agli script di movimento
        Human.gameObject.GetComponent<PlayerMover>().player = Players[0];
        Computer.gameObject.GetComponent<PlayerMover>().player = Players[1];
        
        // Imposto le pedine nel Player
        Players[0].setToken(Human);
        Players[1].setToken(Computer);

        // Imposto la cella di appartenenza deli Player all'inizio della partita
        Players[0].setCurrentCell(Cells[0]);
        Players[1].setCurrentCell(Cells[0]);
		
	    // Posiziono le pedine sulla prima cella
	    Human.transform.position = new Vector3(1710, 0, 57);
	    Computer.transform.position = new Vector3(1660, 0, 57);
		
    }

    /** Funzione per decidere l'ordine dei giocatori.
     * Restituisce l'ID del giocatore che deve iniziare la partita
     **/
    private void playersOrder()
    {
		// Se sono in debug, salto la fase di decisione del turno:
		// inizia sempre l'umano
		if (debug) {
			curPlayer = Players[0];
            // Imposto le pedine sulla casella iniziale
            moveTokenToCell(Players[1], Cells[0]);
            // Imposto il gameStatus nel turno
            gameStatus = GAME_STATUS.START_TURN;
            moveTokenToCell(Players[0], Cells[0]);
		} else {
	        // Imposto lo status al roll for order
	        gameStatus = GAME_STATUS.ROLL_FOR_ORDER;
	        // Avvio il rolling dell'umano
	        humanRollDice();
		}
    }

    private void humanRollDice()
    {
        guiMgr.rollComputer = false;
        // Imposto il turno sull'umano
        curPlayer = Players[0];
        // Viene avviata la procedura per eseguire il rolling da parte dell'umano
        guiMgr.guiMode = GUIManager.GUIMode.RollDice;
        guiMgr.rollHuman = true;
    }

    private void computerRollDice()
    {
        guiMgr.rollHuman = false;
        // Imposto il turno sull computer
        curPlayer = Players[1];
        // Viene avviata la procedura per eseguire il rolling da parte del computer
        guiMgr.guiMode = GUIManager.GUIMode.RollDice;
        guiMgr.rollComputer = true;
    }

    private void cameraForDiceMovementCompleted()
    {
        // Feedback utente
        setLabel("...lanciati!");
        // Lancio dei dadi
        GameObject.Find("app").GetComponent<AppDemo>().Roll();
    }

    private void cameraForStartMovementCompleted()
    {
        Wait(1);
        // Feedback utente
        setLabel("Si inizia!");        
    }

    private void RollingCompleted(int[] res)
    {
		int result = res[0];
        // Se il rolling e' nella fase di selezione turno
        if (gameStatus == GAME_STATUS.ROLL_FOR_ORDER)
        {
            if (curPlayer.getPlayerType() == Player.PLAYER_TYPE.HUMAN)
            {
                // Mostro il risultato al giocatore
                setLabel("Hai totalizzato " + result + ". Adesso tocca al computer...");
                humanRoll = result;
                Wait(1);
                computerRollDice();
            }
            else if (curPlayer.getPlayerType() == Player.PLAYER_TYPE.COMPUTER)
            {
                // Attendo 1 secondo
                Wait(1);
                computerRoll = result;
                // Restituisce il controllo al gestore di decisione turno
                setPlayersOrder();
            }
        }
        // Se ci si trova all'interno di un turno
        else if (gameStatus == GAME_STATUS.IN_TURN)
        {
			ritira = res[1]==1 ? true : false;
			Debug.Log(ritira);
            // Alla fine del rolling, torno nella main GUI
            guiMgr.guiMode = GUIManager.GUIMode.Main;

            Wait(2);

            // se il giocatore e' umano
            if (curPlayer.getPlayerType() == Player.PLAYER_TYPE.HUMAN)            
            {
                // Mostro il risultato in una certa maniera
                Debug.Log("L'umano ha tirato i dadi.");
                setLabel("Hai totalizzato " + result + ". Vediamo dove andrai a finire...");
                Wait(1);
                curPlayer.moveTokenBy(result, Cells);
            }
            else
            {
                // Mostro il risultato in una certa maniera
                Debug.Log("Il computer ha tirato i dadi.");
                setLabel("Il computer ha totalizzato " + result + ". Vediamo dove andra' a finire...");
                Wait(1);
                curPlayer.moveTokenBy(result, Cells);
            }
        }
    }

    private void setPlayersOrder()
    {
        // UNDONE: la seguente riga e' per testing
        if (debug) humanRoll = computerRoll+1;

        // Controllo chi dei due giocatori ha totalizzato il punteggio piu' alto
        if (humanRoll > computerRoll)
        {
            setLabel("Il computer ha totalizzato " + computerRoll + "." + "Complimenti! Sei il primo a tirare i dadi!");
            curPlayer = Players[0];
            // Imposto le pedine sulla casella iniziale
            moveTokenToCell(Players[1], Cells[0]);
            // Imposto il gameStatus nel turno
            gameStatus = GAME_STATUS.START_TURN;
            moveTokenToCell(Players[0], Cells[0]);
			playersSet = true;
        }
        else if (humanRoll < computerRoll)
        {
            setLabel("Il computer ha totalizzato " + computerRoll + "." + "Dannazione, il computer inizia gia' a vincere...");
            curPlayer = Players[1];
            // Imposto le pedine sulla casella iniziale
            moveTokenToCell(Players[0], Cells[0]);
            // Imposto il gameStatus nel turno
            gameStatus = GAME_STATUS.START_TURN;
            moveTokenToCell(Players[1], Cells[0]);
			playersSet = true;
			
	        // Posiziono le pedine sulla prima cella
	        Human.transform.position = new Vector3(1710, 0, 57);
	        Computer.transform.position = new Vector3(1660, 0, 57);
        }
        else
        {
            // In caso di pareggio faccio rilanciare i dadi ad entrambi
            setLabel("Il computer ha totalizzato " + computerRoll + "." + "Pareggio! Ricominciamo daccapo...");
            playersOrder();
        }
    }

    /** Metodo per lo spostamento di un Token su una cella * */
    public void moveTokenToCell(Player moveThisPlayer, Cell movePlayerToThisCell) {
        // Sposto il giocatore
        moveThisPlayer.moveTokenToCell(movePlayerToThisCell, Cells);
        // Sposto anche la telecamera
        MoveCamera scriptCamera = GameObject.Find("camera").GetComponent<MoveCamera>();
        scriptCamera.moveCameraForCell(movePlayerToThisCell);
    }
	
    private void cameraForCellMovementCompleted()
    {
        
    }
	
	private void playerMovementCompleted()
    {
        // Se il turno dvee iniziare
        switch (gameStatus) {
            case (GAME_STATUS.START_TURN):
				if (playersSet) {
					// Posiziono le pedine sulla prima cella
			        Human.transform.Translate(1710, 0, 57);
			        Computer.transform.Translate(1660, 0, 57);
				} else {
					playersSet = true;
				}
				TurnManager(curPlayer);
                break;

            case (GAME_STATUS.IN_TURN):
                // Avviso il GUIManager che puo' mostrare il pulsante di fine turno
                guiMgr.guiMode = GUIManager.GUIMode.InTurn;
                // Debug.Log("Sulla cella di arrivo " + curPlayer.getCurrentCell().getPosition() + " ora ci sono " + curPlayer.getCurrentCell().getTokensOnCell() + " pedine.");
                TurnManager(curPlayer);
				break;

            default:
                break;
        }
    }

    /** Gestore del turno per un giocatore */
    private void TurnManager(Player player)
    {
		bool human = (player.getPlayerType() == Player.PLAYER_TYPE.HUMAN);
        switch (gameStatus) {
			// Se il turno deve iniziare
            case (GAME_STATUS.START_TURN):
                // Controllo che tipo di giocatore e' quello che deve iniziare il turno
                // se il giocatore e' umano
                if (human)
                    humanRollDice();
                else
                    computerRollDice();
                // Dopo il lancio dei dadi, lo status e' IN_TURN (turno iniziato)
                gameStatus = GAME_STATUS.IN_TURN;
                break;
			
			// Se siamo all'interno del turno
			case (GAME_STATUS.IN_TURN):
                // Controllo che tipo di giocatore e' quello che deve giocare il turno
                // se il giocatore e' umano
				avversario = (Players[0] == curPlayer ? Players[1] : Players[0]);
				if (human)
                {
                    // TODO: PARTE DI GIOCO UMANO
					// Avviso dell'inizio del caricamento
					guiMgr.setLabel("Caricamento cella in corso...");
					// Avvio la parte di apparizione cella
					GameObject.Find("MAIN").GetComponent<LevelLoader>().loadLevel(player.getCurrentCell().getPosition());
					// Dico al GUIManager chi e' il giocatore
					guiMgr.curPlayer = curPlayer;
				}
				
            	// Controllo il tipo di cella, per capire come comportarmi
				switch (player.getCurrentCell().getType()) {
					case (Cell.CELL_TYPE.VIA):
						curPlayer.increaseMoney(200);
						Debug.Log("VIA!");
						break;
					case (Cell.CELL_TYPE.IMPREVISTO):
						Debug.Log("Imprevisto");
						if (human) guiMgr.guiMode = GUIManager.GUIMode.OnOtherCell;
						/*int num_rand = Random.Range(-500, +500);
						if(num_rand > 0)
						{
							curPlayer.increaseMoney(num_rand);
							setLabel("Guadagni " +  num_rand);		
						}
						else
						{	int pos = -num_rand;
							curPlayer.decreaseMoney(pos);
							setLabel("Perdi " + pos);
						}*/
						break;
					case (Cell.CELL_TYPE.PROBABILITA):
						if (human) guiMgr.guiMode = GUIManager.GUIMode.OnOtherCell;
						Debug.Log("Probabilita'");
						/*	int to_pay = Random.Range(-500, +500);
						if(to_pay > 0)
						{
							curPlayer.increaseMoney(to_pay);
							setLabel("Guadagni " +  to_pay);		
						}
						else
						{	int pos = -to_pay;
							curPlayer.decreaseMoney(pos);
							setLabel("Perdi " + pos);
						}*/
						break;
					case (Cell.CELL_TYPE.POSTEGGIO):
						Debug.Log("Posteggio");
						if (human) guiMgr.guiMode = GUIManager.GUIMode.OnOtherCell;
						break;
					case (Cell.CELL_TYPE.TRANSITO):
						Debug.Log("Transito");
						if (human) guiMgr.guiMode = GUIManager.GUIMode.OnOtherCell;
						break;
					case (Cell.CELL_TYPE.VAI_PRIGIONE):
						Debug.Log("Vai in prigione!");
						if (human) guiMgr.guiMode = GUIManager.GUIMode.OnOtherCell;
						// curPlayer.decreaseMoney(200);
						break;
					case (Cell.CELL_TYPE.PRIGIONE):
						Debug.Log("Prigione");
						if (human) guiMgr.guiMode = GUIManager.GUIMode.OnOtherCell;
						break;
					case (Cell.CELL_TYPE.TASSA):
						Debug.Log("Tassa!");
						curPlayer.decreaseMoney(200);
						if (human) guiMgr.guiMode = GUIManager.GUIMode.OnOtherCell;
						break;
					case (Cell.CELL_TYPE.PROPRIETA):
						// controllo di chi è la proprietà
						// se la proprieta' e' dell'umano
						Property relProperty = curPlayer.getCurrentCell().getProperty();
						Group grup =  relProperty.getGroup();
						if(player.getCurrentCell().getProperty().getOwner().getPlayerType() == curPlayer.getPlayerType() )
						{
							if(human)
							{
								// se l'umano possiede tutte le proprieta' del gruppo, puo' costruire
								if (player.canBuild(relProperty) && grup != Groups[8] && grup != Groups[9]) {
								// mostro la GUI per la costruzione su questo terreno (Vendi, ipoteca e costruisci qui)
								guiMgr.guiMode = GUIManager.GUIMode.OnCanBuildHere;
								} else guiMgr.guiMode = GUIManager.GUIMode.OnMyCell;
							}
							else if (player.canBuild(relProperty) && grup != Groups[8] && grup != Groups[9]) {
								int i = 5 - curPlayer.getCurrentCell().getProperty().getNumberOfBuildings();
								int randomNumber = Random.Range(1, i);
								if (curPlayer.getMoney() > curPlayer.getCurrentCell().getProperty().getBuildingsCost(randomNumber))
								{	
									
										curPlayer.build(curPlayer.getCurrentCell().getProperty(),randomNumber);
										setLabel("Il computer ha costruito" + randomNumber + "case");
										
										// Faccio apparire solo le case effettivamente costruite
										updateAllBuildings();
										break;
								}
							} else {
								setLabel("Il computer ha deciso di non comprare nulla.");
								Debug.Log("Proprieta' del computer");
							}
						}
						else if(player.getCurrentCell().getProperty().getOwner().getPlayerType() == avversario.getPlayerType())
						{
								float decrease = player.getCurrentCell().getProperty().getRent();
						
								player.decreaseMoney(decrease);
								avversario.increaseMoney(decrease);
								setLabel("Disdetta! Sei finito su una cella dell'avversario! Hai pagato " + decrease + "M.");
								if(human)
								{
									guiMgr.guiMode = GUIManager.GUIMode.OnOtherCell;
								}
						}
						else if(player.getCurrentCell().getProperty().getOwner().getPlayerType() == Player.PLAYER_TYPE.BANK)
						{
							if(human) {
								// imposto la cella corrente
								guiMgr.curCell = player.getCurrentCell();
								// se il giocatore ha abbastanza soldi per acquistare la proprieta'
								if (player.getMoney() > player.getCurrentCell().getProperty().getPropertyCost()) {
									// il GUIManager disegna i pulsanti per la proprieta' libera
									guiMgr.guiMode = GUIManager.GUIMode.OnFreeCell;
									Debug.Log("Proprieta' libera.");
								} else {
									// il GUIManager viene avvisato dell'impossibilita' di comprare il terreno
									guiMgr.guiMode = GUIManager.GUIMode.OnCantBuyCell;
									Debug.Log("Proprieta' libera. NON puo' essere acquistata, troppi pochi soldi!");
								}
							} else {
								guiMgr.curCell = player.getCurrentCell();
								// se il giocatore ha abbastanza soldi per acquistare la proprieta'
								Group gruppo = curPlayer.getCurrentCell().getProperty().getGroup();
								int y = Random.Range(0,100);
								Debug.Log(gruppo.getDimension()- curPlayer.countGroupPropertiesOwned(gruppo));
								switch(gruppo.getDimension()- curPlayer.countGroupPropertiesOwned(gruppo))
								{
									case(1):
										if (player.getMoney() > player.getCurrentCell().getProperty().getPropertyCost()) {
											// il GUIManager disegna i pulsanti per la proprieta' libera
											//guiMgr.guiMode = GUIManager.GUIMode.OnFreeCell;
											//compra
											curPlayer.buyProperty(curPlayer.getCurrentCell().getProperty());
											setLabel("Il computer ha acquistato la cella " + curPlayer.getCurrentCell().getProperty().getName().ToString());
											TurnManager(curPlayer);
											return;
										}
										else setLabel("Il computer ha deciso di non acquistare");
										break;
									case(2):
										if(y > 25)
										{
											if (player.getMoney() > player.getCurrentCell().getProperty().getPropertyCost()) 
											{curPlayer.buyProperty(curPlayer.getCurrentCell().getProperty());
										setLabel("Il computer ha acquistato la cella " + curPlayer.getCurrentCell().getProperty().getName().ToString());}
										}
										else setLabel("Il computer ha deciso di non acquistare");
										break;
									case (3):
										if(y > 45)
										{if (player.getMoney() > player.getCurrentCell().getProperty().getPropertyCost()) 
											{curPlayer.buyProperty(curPlayer.getCurrentCell().getProperty());
										setLabel("Il computer ha acquistato la cella " + curPlayer.getCurrentCell().getProperty().getName().ToString());}
										}
										else setLabel("Il computer ha deciso di non acquistare");
										break;
								default: if(y > 55)
										{if (player.getMoney() > player.getCurrentCell().getProperty().getPropertyCost()) 
											{curPlayer.buyProperty(curPlayer.getCurrentCell().getProperty());
										setLabel("Il computer ha acquistato la cella " + curPlayer.getCurrentCell().getProperty().getName().ToString());}
										}
										else setLabel("Il computer ha deciso di non acquistare");
										break;
								}
							}
						}
				break;
			default:
				break;
						}
					if(!human)
					{	// UNDONE: far terminare il turno alla fine delle azioni dell'AIManager
						Wait(2);
		            	guiMgr.rollComputer = false;
						turnOver();
					}
			break;

            case (GAME_STATUS.TURN_OVER):
				// Se il turno era dell'umano
				if (human) {
					// faccio riapparire tutto
					GameObject.Find("MAIN").GetComponent<LevelLoader>().returnToBoard();
					// se la cella su cui l'umano si trovava e' una proprieta' ancora libera
					if ((curPlayer.getCurrentCell().getType() == Cell.CELL_TYPE.PROPRIETA) &&
				    	(curPlayer.getCurrentCell().getProperty().getOwner().getPlayerType() != Player.PLAYER_TYPE.HUMAN)) {
							// TODO: inserire la proprieta' in asta per tutti
							Debug.Log("Proprieta' inserita in asta.");
					}
					// Faccio apparire solo le case effettivamente costruite
					updateAllBuildings();
				}
				
				if(Players[0].getMoney() <= 0 || Players[1].getMoney() <= 0)
				{
					gameOver();
					return;
				}
				// UNDONE: se sono in debug, il turno e' sempre dell'umano
				if (!debug && !ritira) {
	                // Modifico il curPlayer
	                curPlayer = (Players[0] == curPlayer ? Players[1] : Players[0]);
				}
				
				Wait(1);
			
				if(ritira)
					setLabel("E' uscito un numero doppio.. Ritira i dadi");
				// Modifico lo status del turno: inizio un nuovo turno
				gameStatus = GAME_STATUS.START_TURN;
				// Riavvio il turno
					
                TurnManager(curPlayer);
                break;
			
		default:break;	
    	}
	}
	
	private void gameOver() {
		// GameObject da posizionare
		GameObject Model;
		
        Vector3 position = GameObject.Find("OverPoint").transform.position;
        Quaternion rotation = GameObject.Find("OverPoint").transform.rotation;
		
		if (Players[0].getMoney() >= Players[1].getMoney()) {
			Model = (GameObject) Instantiate(Resources.Load("3D/YouWon/YouWon", typeof(GameObject)), position, rotation);
		} else {
			Model = (GameObject) Instantiate(Resources.Load("3D/YouLost/YouLost", typeof(GameObject)), position, rotation);
		}
		// Muovo la camera
		getMoveCamera().moveCameraForOver();
	}
	
	public void cameraForOverMovementCompleted() {
		// Disegno il pulsante di ritorno al menu
		guiMgr.guiMode = GUIManager.GUIMode.GameOver;
	}

    private void turnOver()
    {
		// Il turno e' finito
        gameStatus = GAME_STATUS.TURN_OVER;
		// Chiamo il gestore dei turni
        TurnManager(curPlayer);
    }

    public Group getGroupByName(List<Group> GroupList, string findWithThisName)
    {
        return GroupList.Find(
            delegate(Group findThisGroup)
            {
                return (findThisGroup.getName() == findWithThisName);
            }
        );
    }

    private IEnumerator Wait(float Seconds)
    {
        yield return new WaitForSeconds(Seconds);
    }

    private void setLabel(string label)
    {
        guiMgr.setLabel(label);
    }

    private MoveCamera getMoveCamera()
    {
        return GameObject.Find("camera").GetComponent<MoveCamera>();
    }
	
	/** Funzione che serve al GUIManager per confermare l'acquisto di un terreno.
	 *	Non c'e' bisogno di nessun parametro perche' il GameManager sa quale giocatore
	 *	sta giocando e su quale terreno si trova.
	 * */
	public void buyLand() {
		// Il giocatore corrente compra il terreno
		curPlayer.buyProperty(curPlayer.getCurrentCell().getProperty());
		// Cambio lo stato del GUIManager, in modo tale che mostri i pulsanti per la proprieta' posseduta
		if (curPlayer.canBuild(curPlayer.getCurrentCell().getProperty()))
			guiMgr.guiMode = GUIManager.GUIMode.OnCanBuildHere;
		else
			guiMgr.guiMode = GUIManager.GUIMode.OnMyCell;
	}
	
	public void updateAllBuildings() {
		// Faccio apparire solo le case effettivamente costruite
		GlobalBuildingCompany company = GameObject.Find("BuildingCompany").GetComponent<GlobalBuildingCompany>();
		company.updateAllBuildings();
	}
}
