using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {
    // Variabile che tiene traccia del chiamante in modo tale da eseguire una risposta pertinente
    private enum CALLER { NONE, FORDICE, START_GAME, TO_CELL, GAME_OVER, GENERAL };
    private CALLER caller = CALLER.NONE;

    private int moveCameraTo(Vector3 position, float velocity)
    {
        iTween.MoveTo(gameObject, iTween.Hash("x", position.x, "y", position.y, "z", position.z, "time", velocity, "easeType", iTween.EaseType.easeInOutCubic, "onComplete", "movementCompleted"));
        return 0;
    }

    private int rotateCameraTo(Vector3 rotation, float velocity)
    {
        iTween.RotateTo(gameObject, iTween.Hash("x", rotation.x, "y", rotation.y, "z", rotation.z, "time", velocity, "easeType", iTween.EaseType.easeInOutCubic));
        return 0;
    }

    private void movementCompleted()
    {
        if (caller == CALLER.FORDICE)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().SendMessage("cameraForDiceMovementCompleted");
        }
        else if (caller == CALLER.START_GAME)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().SendMessage("cameraForStartMovementCompleted");
        }
        else if (caller == CALLER.TO_CELL)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().SendMessage("cameraForCellMovementCompleted");
        }
		else if (caller == CALLER.GAME_OVER)
		{
			GameObject.Find("GameManager").GetComponent<GameManager>().SendMessage("cameraForOverMovementCompleted");
		}

        // Una volta eseguita la funzione, reimpostiamo a NONE il caller
        caller = CALLER.NONE;
    }
	
	public void moveCameraForOver() {
		// Imposto il caller
        caller = CALLER.GAME_OVER;

        Vector3 position = GameObject.Find("OverCamera").transform.position;
        Vector3 rotation = GameObject.Find("OverCamera").transform.rotation.eulerAngles;

        moveCameraTo(position, 2F);
        rotateCameraTo(rotation, 2F);
	}

    public void moveCameraForDice()
    {
        // Imposto il caller
        caller = CALLER.FORDICE;

        Vector3 position = GameObject.Find("DiceCamera").transform.position;
        Vector3 rotation = GameObject.Find("DiceCamera").transform.rotation.eulerAngles;

        moveCameraTo(position, 2F);
        rotateCameraTo(rotation, 2F);
    }

    public void moveCameraForPosition(Vector3 position, Vector3 rotation)
    {
        // Imposto il caller
        caller = CALLER.START_GAME;

        moveCameraTo(position, 2F);
        rotateCameraTo(rotation, 2F);
    }

    public void moveCameraForCell(Cell moveToThisCell)
    {
        // Imposto il caller
        caller = CALLER.TO_CELL;
        // Dimensione della tavola in larghezza e lunghezza
        float board = 1850;
        // Altezza fissa della telecamera
        float height = 600;
        // Distanza x e/o z della telecamera dalla board
        float distance = 700;
        // Misura di mezza larghezza di cella
        float halfCell = 75;

        // Componenti del vettore finale di posizione
        float newX = 0;
        float newY = height;
        float newZ = 0;
        // Componenti del vettore finale di rotazione
        float newRX = 27;
        float newRY = 0;
        float newRZ = 0;
        // Vettore finale di posizione
        Vector3 newPosition;
        // Vettore finale di rotazione
        Vector3 newRotation;

        // GameObject di riferimento alla cella
        GameObject cell = GameObject.FindWithTag(moveToThisCell.getPosition().ToString());

        // Muovo la telecamera lungo gli assi diversamente a seconda del lato della cella
        if (moveToThisCell.getSide() == 1)
        {
            // Se la cella e' d'angolo, imposto coordinate fisse
            if (moveToThisCell.isCorner())
            {
                newX = -distance;
                newZ = -distance;
            }
            else
            {
                // Ottengo i valori della x e z
                newX = cell.transform.position.x + halfCell;
                newZ = -distance;
            }
        }
        else if (moveToThisCell.getSide() == 2)
        {
            // Se la cella e' d'angolo, imposto coordinate fisse
            if (moveToThisCell.isCorner())
            {
                newX = -distance;
                newZ = board + distance;
            }
            else
            {
                // Ottengo i valori della x e z
                newX = -distance;
                newZ = cell.transform.position.z + halfCell;
            }
        } else if (moveToThisCell.getSide() == 3)
        {
            // Se la cella e' d'angolo, imposto coordinate fisse
            if (moveToThisCell.isCorner())
            {
                newX = board + distance;
                newZ = board + distance;
            }
            else
            {
                // Ottengo i valori della x e z
                newX = cell.transform.position.x + halfCell;
                newZ = board + distance;
            }
        }
        else if (moveToThisCell.getSide() == 4)
        {
            // Se la cella e' d'angolo, imposto coordinate fisse
            if (moveToThisCell.isCorner())
            {
                newX = board + distance;
                newZ = -distance;
            }
            else
            {
                // Ottengo i valori della x e z
                newX = board + distance;
                newZ = cell.transform.position.z + halfCell;
            }
        }

        // La rotazione della telecamera sulle Y e' facilmente ottenibile a partire dal lato della cella
        newRY = (moveToThisCell.getSide() - 1) * 90;
        // Se la cella e' corner, devo aggiungere 45 alla rotazione sulle Y
        if (moveToThisCell.isCorner())
        {
            newRY += 45;
        }

        // Creo il vettore di posizione della telecamera
        newPosition = new Vector3(newX, newY, newZ);
        // Creo il vettore di rotazione della telecamera
        newRotation = new Vector3(newRX, newRY, newRZ);

        moveCameraTo(newPosition, 0.5F);
        rotateCameraTo(newRotation, 0.5F);
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
