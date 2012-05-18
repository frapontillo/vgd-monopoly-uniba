using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMover : MonoBehaviour {

    // Valori di velocita' in salita
    public float jumpingVelocity = 0.3F;
    // Valori di velocita' in discesa
    public float fallingVelocity = 0.2F;
    // Valore di altezza raggiunto dalla pedina in ogni movimento
    public float highnessValue = 100;
    // Player cui si riferisce il Mover
    public Player player;

    private int numberOfMovements = 0;
    private Cell movePlayerFromThisCell;
    private Cell movePlayerToThisCell;
    private List<Cell> Cells;

    public void move(Vector3 positionUp, Vector3 positionDown) {
        moveUp(positionUp, positionDown);
    }

    private void moveUp(Vector3 positionUp, Vector3 positionDown)
    {
        // Ottengo l'oggetto da spostare
        GameObject Token = gameObject;
        iTween.MoveTo(Token, iTween.Hash("position", positionUp, "time", jumpingVelocity, "easeType", iTween.EaseType.easeInCirc, "onComplete", "moveDown", "onCompleteParams", positionDown, "onCompleteTarget", gameObject));
    }

    private void moveDown(Vector3 positionDown)
    {
        // Ottengo l'oggetto da spostare
        GameObject Token = gameObject;
        iTween.MoveTo(Token, iTween.Hash("position", positionDown, "time", fallingVelocity, "easeType", iTween.EaseType.easeInOutQuad, "onComplete", "moveOver", "onCompleteTarget", gameObject));
    }

    private void moveOver()
    {
        // Richiamo il moveTokenToCell con i nuovi parametri
        moveTokenToCell(movePlayerFromThisCell, movePlayerToThisCell, Cells);
    }

    public void moveTokenToCell(Cell movePlayerFromThisCell, Cell movePlayerToThisCell, List<Cell> Cells)
    {
        // Aggiorno i valori ricevuti in input
        this.movePlayerFromThisCell = movePlayerFromThisCell;
        this.movePlayerToThisCell = movePlayerToThisCell;
        this.Cells = Cells;

        numberOfMovements = 0;

        // Calcolo il numero di movimenti ancora da eseguire
        numberOfMovements = (movePlayerToThisCell.getPosition() - movePlayerFromThisCell.getPosition()) % 40;
        if (numberOfMovements < 0)
        {
            numberOfMovements = 40 + numberOfMovements;
        }
		
		// UNDONE: eliminare seguente riga di testing
		// numberOfMovements = 1;

        // Se ci sono movimenti da eseguire
        if (numberOfMovements > 0)
        {
            // Effettuo il movimento
            moveTokenToNextCell();
        }
        else
        {
            player.setCurrentCell(movePlayerToThisCell);
            movementCompleted();
        }
    }

    private void movementCompleted()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().SendMessage("playerMovementCompleted");
    }

    private void moveTokenToNextCell()
    {
        // Ottengo la prossima cella
        Cell nextCell = Cells[(movePlayerFromThisCell.getPosition() + 1) % 40];
        // Ottengo l'oggetto da spostare
        GameObject Token = gameObject;

        // Variabili che conterranno le nuove coordinate per gli spostamenti
        Vector3 newHighPosition = new Vector3();
        Vector3 newLowPosition = new Vector3();

        // Controllo se c'e' qualche altro giocatore nella cella adiacente e in quella attuale.
        // In tal caso, lo spostamento sara' shiftato di 50m per ogni pedina in piu'.
        int startRefactorValue = movePlayerFromThisCell.getTokensOnCell();
        int endRefactorValue = nextCell.getTokensOnCell();

        List<Vector3> positions = getPositionsByCell(nextCell);
        move(positions[0], positions[1]);

        // Sposto la camera sulla cella che sara' quella di arrivo della pedina
        GameObject.Find("camera").GetComponent<MoveCamera>().moveCameraForCell(nextCell);

        // Aggiorno i valori di cella corrente e di numero di movimenti da compiere
        movePlayerFromThisCell = nextCell;
        numberOfMovements--;
    }

    public List<Vector3> getPositionsByCell(Cell cell)
    {
        List<Vector3> positions = new List<Vector3>();

        Vector3 newHighPosition = new Vector3();
        Vector3 newLowPosition = new Vector3();

        // Ottengo l'oggetto da spostare
        GameObject Token = gameObject;

        // Controllo se c'e' qualche altro giocatore nella cella adiacente e in quella attuale.
        // In tal caso, lo spostamento sara' shiftato di 50m per ogni pedina in piu'.
        int endRefactorValue = cell.getTokensOnCell();

        // Se la prossima cella e' d'angolo, mi comporto in una maniera
        if (cell.isCorner())
        {
            if (cell.getSide() == 1)
            {
                float xValue = GameObject.FindWithTag(cell.getPosition().ToString()).transform.position.x;
                newLowPosition = new Vector3(
                    57,
                    0,
                    140 + (50 * endRefactorValue)
                    );

                newHighPosition = new Vector3(
                    Token.transform.position.x - (Token.transform.position.x - newLowPosition.x) / 2,
                    highnessValue,
                    Token.transform.position.z
                    );
            }
            else if (cell.getSide() == 2)
            {
                float zValue = GameObject.FindWithTag(cell.getPosition().ToString()).transform.position.x;
                newLowPosition = new Vector3(
                    140 + (50 * endRefactorValue),
                    0,
                    1790
                    );

                newHighPosition = new Vector3(
                    Token.transform.position.x,
                    highnessValue,
                    Token.transform.position.z - (Token.transform.position.z - newLowPosition.z) / 2
                    );
            }
            else if (cell.getSide() == 3)
            {
                float xValue = GameObject.FindWithTag(cell.getPosition().ToString()).transform.position.x;
                newLowPosition = new Vector3(
                    1790,
                    0,
                    1750 + (50 * endRefactorValue)
                    );

                newHighPosition = new Vector3(
                    Token.transform.position.x - (Token.transform.position.x - newLowPosition.x) / 2,
                    highnessValue,
                    Token.transform.position.z
                    );
            }
            else if (cell.getSide() == 4)
            {
                float zValue = GameObject.FindWithTag(cell.getPosition().ToString()).transform.position.x;
                newLowPosition = new Vector3(
                    1710 + (50 * endRefactorValue),
                    0,
                    57
                    );

                newHighPosition = new Vector3(
                    Token.transform.position.x,
                    highnessValue,
                    Token.transform.position.z - (Token.transform.position.z - newLowPosition.z) / 2
                    );

            }
        }
        // altrimenti faccio avanzare verso destra/sinistra/alto/basso normalmente
        else
        {
            // Se la cella si trova sul primo lato, devo diminuire le X di 150 +/- i refactors
            if (cell.getSide() == 1)
            {
                float xValue = GameObject.FindWithTag(cell.getPosition().ToString()).transform.position.x;
                newLowPosition = new Vector3(
                    xValue + 100 - (50 * endRefactorValue),
                    0,
                    57
                    );

                newHighPosition = new Vector3(
                    Token.transform.position.x - (Token.transform.position.x - newLowPosition.x) / 2,
                    highnessValue,
                    57
                    );
            }
            else if (cell.getSide() == 2)
            {
                float zValue = GameObject.FindWithTag(cell.getPosition().ToString()).transform.position.z;
                newLowPosition = new Vector3(
                    57,
                    0,
                    zValue + 50 + (50 * endRefactorValue)
                    );

                newHighPosition = new Vector3(
                    57,
                    highnessValue,
                    Token.transform.position.z - (Token.transform.position.z - newLowPosition.z) / 2
                    );
            }
            else if (cell.getSide() == 3)
            {
                float xValue = GameObject.FindWithTag(cell.getPosition().ToString()).transform.position.x;
                newLowPosition = new Vector3(
                    xValue + 40 + (50 * endRefactorValue),
                    0,
                    1790
                    );

                newHighPosition = new Vector3(
                    Token.transform.position.x - (Token.transform.position.x - newLowPosition.x) / 2,
                    highnessValue,
                    1790
                    );
            }
            else if (cell.getSide() == 4)
            {
                float zValue = GameObject.FindWithTag(cell.getPosition().ToString()).transform.position.z;
                newLowPosition = new Vector3(
                    1790,
                    0,
                    zValue - 50 + (50 * endRefactorValue)
                    );

                newHighPosition = new Vector3(
                    1790,
                    highnessValue,
                    Token.transform.position.z - (Token.transform.position.z - newLowPosition.z) / 2
                    );
            }
        }

        positions.Add(newHighPosition);
        positions.Add(newLowPosition);

        return positions;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
