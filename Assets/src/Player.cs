using UnityEngine;
using System.Collections.Generic;

public class Player
{
    // Lista di possibili valori di giocatore
    public enum PLAYER_TYPE { BANK, HUMAN, COMPUTER };

    // Tipo di giocatore
    PLAYER_TYPE PlayerType;
    // Identificatore pedina giocatore (da 1 a 6)
    int TokenID;
    // Soldi posseduti dal giocatore
    float Money;

    // Lista di proprieta' possedute
    List<Property> OwnedProperties = new List<Property>();
    // Cella occupata dal giocatore in questo momento
    Cell curCell;
    // GameObject rappresentante la pedina 3D
    GameObject Token;

    /* Costruttore di classe, accetta in input
     * PlayerType: tipo enum di giocatore
     * TokenID: intero che rappresenta la pedina da utilizzare
     * Money: somma da assegnare al giocatore
     * PropertyList: lista di proprieta' della partita (globale)
     */
    public Player(PLAYER_TYPE PlayerType, int TokenID, float Money, List<Property> PropertyList)
    {
        this.PlayerType = PlayerType;
        this.TokenID = TokenID;
        this.Money = Money;
		
		if (PlayerType != PLAYER_TYPE.BANK) {
	        // Il numero che sara' random
	        int randomNumber = -1;
	        // Lista che conterra' le proprieta' libere
	        List<Property> tempList = new List<Property>();
	        for (int i = 0; i < PropertyList.Count; i++)
	        {
	            if (!PropertyList[i].isOwned()) {
	                tempList.Add(PropertyList[i]);
	            }
	        }
	
	        // A questo punto posso eseguire 6 volte il random sulla tempList
	        for (int i = 0; i < 7; i++)
	        {
	            randomNumber = Random.Range(0, tempList.Count - 1);
	            // Il giocatore compra la proprieta'
	            this.buyProperty(tempList[randomNumber]);
	            // La proprieta' viene eliminata dalla tempList perche' non piu' disponibile
	            tempList.RemoveAt(randomNumber);
	        }
		}
    }
	
	public List<Property> getPropertyList() {
		return this.OwnedProperties;
	}

    /** Metodo per lanciare i dadi
     * /

    /** Metodo per acquistare una proprieta', passata in input
     * 
     *  buyThisProperty: Property da acquistare 
     * 
     * */
    public float buyProperty(Property buyThisProperty)
    {
        // Imposto il nuovo proprietario della proprieta'
        buyThisProperty.setOwner(this);
        // Aggiungo la proprieta' alla lista delle proprieta' possedute dal giocatore
        OwnedProperties.Add(buyThisProperty);

        // Ottengo il costo della proprieta'
        float cost = buyThisProperty.getPropertyCost();
        // Decremento i soldi del giocatore
        this.Money -= cost;
        // Restituisco la somma pagata
        return cost;
    }

    /** Metodo per controllare che il giocatore corrente possa costruire su una proprieta'
     * */
    public bool canBuild(Property buildOnThisProperty)
    {
        bool possibility = true;
        Group relGroup;

        // Ottengo il gruppo dalla proprieta'
        relGroup = buildOnThisProperty.getGroup();

        // Controllo che il giocatore possieda tutte le altre proprieta' del gruppo
        possibility = (this.countGroupPropertiesOwned(relGroup) == relGroup.getDimension());

        return possibility;
    }

    /** Metodo per contare quante proprieta' di un gruppo sono possedute dal giocatore corrente
     * */
    public int countGroupPropertiesOwned(Group checkThisGroup)
    {
        int counter = 0;
        // Ottengo la lista delle proprieta' del gruppo
        List<Property> groupProperties = checkThisGroup.getPropertyList();

        // Ciclo su tutte le proprieta' del gruppo
        for (int i = 0; i < checkThisGroup.getDimension(); i++)
        {
            // Se la proprieta' e' posseduta dal giocatore corrente
            if (groupProperties[i].getOwner() == this)
            {
                // incremento il contatore
                counter++;
            }
        }

        // Restituisco il numero di proprieta' del gruppo possedute dal giocatore
        // Se counter == relGroup.getDimension() vuol dire che il giocatore possiede tutte le proprieta'
        return counter;
    }
	
	/** Metodo che permette l'edificazione sui terreni di un gruppo
	 * */
	public void build(Property curProperty, int numberOfBuildings) {
		// Ottengo il gruppo
		Group curGroup = curProperty.getGroup();
		// Per ogni proprieta' nel gruppo
		foreach (Property curP in curGroup.getPropertyList()) {
			float decrease = curP.build(numberOfBuildings);
			curP.getOwner().decreaseMoney(decrease);
		}
	}
	
	/** Metodo che permette la vendita di edifici sui terreni di un gruppo
	 * */
	public void sellBuildings(Property curProperty, int numberOfBuildings) {
		// Ottengo il gruppo
		Group curGroup = curProperty.getGroup();
		// Per ogni proprieta' nel gruppo
		foreach (Property curP in curGroup.getPropertyList()) {
			float increase = curP.sell(numberOfBuildings);
			curP.getOwner().increaseMoney(increase);
		}
	}
	                       
	
    /** Metodo che restituisce il tipo di giocatore (BANK, HUMAN, COMPUTER)
     * */
    public PLAYER_TYPE getPlayerType()
    {
        return this.PlayerType;
    }

    /** Imposta la cella corrente del giocatore
     * */
    public void setCurrentCell(Cell currentCell)
    {
        this.curCell = currentCell;
        List<Vector3> positions = Token.gameObject.GetComponent<PlayerMover>().getPositionsByCell(currentCell);
        Token.gameObject.transform.position = positions[1];
        currentCell.setTokensOnCell(currentCell.getTokensOnCell() + 1);
    }

    /** Restituisce la cella correntemente occupata dal Player
     * */
    public Cell getCurrentCell()
    {
        return this.curCell;
    }

    /** Metodo che si occupa di trovare la nuova cella per il giocatore
     * a partire dalla lista di celle del gioco corrente
     * */
    public Cell getCellBySum(List<Cell> Cells, int findWithThisSum)
    {
        return Cells[(curCell.getPosition() + findWithThisSum) % 40];
    }

    /** Imposta il GameObject della pedina */
    public void setToken(GameObject playerToken)
    {
        this.Token = playerToken;
    }

    /** Restituisce il GameObject della pedina */
    public GameObject getToken()
    {
        return this.Token;
    }

    public void moveTokenToCell(Cell movePlayerToThisCell, List<Cell> Cells)
    {
        // Ottengo la cella correntemente occupata dal giocatore
        Cell movePlayerFromThisCell = this.getCurrentCell();

        // Decremento il numero di giocatori sulla cella corrente
        movePlayerFromThisCell.setTokensOnCell(movePlayerFromThisCell.getTokensOnCell() - 1);

        Token.gameObject.GetComponent<PlayerMover>().moveTokenToCell(movePlayerFromThisCell, movePlayerToThisCell, Cells);
    }

    public void moveTokenBy(int number, List<Cell> Cells)
    {
        // Ottengo la cella correntemente occupata dal giocatore
        Cell movePlayerFromThisCell = this.getCurrentCell();

        // Ottengo la nuova cella di arrivo
		int spostamento = movePlayerFromThisCell.getPosition() + number;
		if (spostamento > 39)
			increaseMoney(200);
        Cell newCell = Cells[spostamento % 40];

        // Chiamo la routine di spostamento
        moveTokenToCell(newCell, Cells);
    }

    public int throwDice()
    {
        int result = 0;



        return result;
    }

    public override string ToString()
    {
        switch (this.getPlayerType())
        {
            case (PLAYER_TYPE.COMPUTER):
                return "COMPUTER";
                break;
            case (PLAYER_TYPE.HUMAN):
                return "GIOCATORE";
                break;
            default:
                return "BANCA";
                break;
        }
    }

    public float getMoney()
    {
        return this.Money;
    }
	
	public void setMoney(float newMoney) {
		this.Money = newMoney;
	}
	
	public void increaseMoney(float increase) {
		this.Money += increase;
	}
	
	public void decreaseMoney(float decrease) {
		this.Money -= decrease;
	}
}