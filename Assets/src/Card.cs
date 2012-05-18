using UnityEngine;
using System.Collections;


/** Classe che modella una carta di probabilita' o imprevisto.
 * Memorizza:
 * - colore della carta
 * - tipo di operazione da compiere
 * - descrizione testuale
 * - valore collegato all'operazione da compiere
 * 
 * */
public class Card {

    public enum CARD_TYPE { BONUS, MALUS, EXIT_JAIL, GO_TO_JAIL, GO_TO };
    public enum CARD_COLOR { ORANGE, GREEN };

    CARD_TYPE CardType;
    string Description;
    float Value;
    CARD_COLOR CardColor;

    // Costruttore per carte di tipo generico
    public Card(CARD_TYPE CardType, string Description, CARD_COLOR CardColor)
    {
        this.CardType = CardType;
        this.Description = Description;
        this.CardColor = CardColor;
    }

    // Costruttore per carte di tipo vincita/perdita o di movimento verso una casella
    public Card(CARD_TYPE CardType, string Description, CARD_COLOR CardColor, float Value)
    {
        this.CardType = CardType;
        this.Description = Description;
        this.CardColor = CardColor;
        this.Value = Value;
    }

    // Imposta il valore della carta
    public void setValue(float Value)
    {
        this.Value = Value;
    }
    // Restituisce il tipo di carta
    public CARD_TYPE getCardType()
    {
        return CardType;
    }
    // Restituisce la descrizione
    public string getDescription()
    {
        return Description;
    }
    // Restituisce il valore
    public float getValue()
    {
        return Value;
    }
    // Restituisce il colore della carta
    public CARD_COLOR getCardColor()
    {
        return CardColor;
    }

    // Lista di funzioni booleane che servono per capire che tipo di azione comporta la carta
    public bool isBonus()
    {
        return this.CardType == CARD_TYPE.BONUS;
    }
    public bool isMalus()
    {
        return this.CardType == CARD_TYPE.MALUS;
    }
    public bool isExitJail()
    {
        return this.CardType == CARD_TYPE.EXIT_JAIL;
    }
    public bool isGoToJail()
    {
        return this.CardType == CARD_TYPE.GO_TO_JAIL;
    }
    public bool isGoTo()
    {
        return this.CardType == CARD_TYPE.GO_TO;
    }
    public bool isGreen()
    {
        return this.CardColor == CARD_COLOR.GREEN;
    }
    public bool isOrange()
    {
        return this.CardColor == CARD_COLOR.ORANGE;
    }
}
