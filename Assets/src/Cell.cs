using UnityEngine;
using System.Collections;

public class Cell {
    public enum CELL_TYPE { VIA, PROPRIETA, PROBABILITA, IMPREVISTO, TASSA, TRANSITO, POSTEGGIO, VAI_PRIGIONE, PRIGIONE };

    // ID = Posizione casella
    int Position;
    // Tipo di cella
    CELL_TYPE Type;
    // Eventuale proprieta' correlata
    Property RelProperty;
    // Valore di vincita/perdita
    float Value;

    // Numero di pedine sulla cella
    int tokensOnCell;

    // Costruttore per celle con valore di vincita/perdita collegato
    public Cell(int Position, CELL_TYPE Type, int Value)
    {
        this.Position = Position;
        this.Type = Type;
        this.Value = Value;
        this.tokensOnCell = 0;
    }

    // Costruttore per celle con proprieta' collegata
    public Cell(int Position, CELL_TYPE Type, Property RelProperty)
    {
        this.Position = Position;
        this.Type = Type;
        this.RelProperty = RelProperty;
        this.tokensOnCell = 0;
    }

    // Costruttore per celle di tipo neutro o prigione o proprieta' non ancora assegnabile
    public Cell(int Position, CELL_TYPE Type)
    {
        this.Position = Position;
        this.Type = Type;
    }

    public int getPosition()
    {
        return this.Position;
    }

    public CELL_TYPE getType()
    {
        return Type;
    }

    // Imposta la proprieta' (se presente) relativa alla cella
    public void setProperty(Property newProperty)
    {
        RelProperty = newProperty; 
    }

    // Restituisce la proprieta' (se presente) relativa alla cella
    public Property getProperty()
    {
        return RelProperty;
    }

    // Restituisce il valore (se presente) relativo alla vincita/perdita della cella
    public float getValue()
    {
        return this.Value;
    }

    // Metodo che imposta un nuovo numero di pedine sulla cella
    public void setTokensOnCell(int newNumberOfTokens)
    {
        this.tokensOnCell = newNumberOfTokens;
    }

    // Metodo che legge quante pedine ci sono sulla cella
    public int getTokensOnCell()
    {
        return this.tokensOnCell;
    }

    // Metodo per incrementare il numero di pedine sulla cella
    public void tokenOnCell()
    {
        this.tokensOnCell++;
    }

    // Metodo per decrementare il numero di pedine sulla cella
    public void tokenLeftCell()
    {
        this.tokensOnCell--;
    }

    // Metodo per conoscere il lato sul quale si trova la cella (da 1 a 4)
    public int getSide()
    {
        int pos = this.getPosition();
        if (pos >= 1 && pos <= 10) {
            return 1;
        }
        else if (pos >= 11 && pos <= 20) {
            return 2;
        }
        else if (pos >= 21 && pos <= 30) {
            return 3;
        }
        else if ((pos >= 31 && pos <= 39) || pos == 0)
        {
            return 4;
        }
        return -1;
    }

    // Metodo per sapere se la cella e' d'angolo
    public bool isCorner()
    {
        return ((this.getPosition() == 0) ||
                (this.getPosition() == 10) ||
                (this.getPosition() == 20) ||
                (this.getPosition() == 30)
               );
    }

}
