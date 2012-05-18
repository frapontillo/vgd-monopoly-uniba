using UnityEngine;
using System.Collections;

public class Property {
    public enum PROPERTY_TYPE { CITY, AIRPORT, SOCIETY };

    // Tipo di proprieta'
    PROPERTY_TYPE PropertyType;
    // Proprietario attuale
    Player Owner;
    // Cella di riferimento
    Cell RelCell;
    // Gruppo di riferimento;
    Group RelGroup;

    // Nome della cella
    string Name = "";
    // Costo di: proprieta', casa, albergo
    float[] Cost = new float[3];
    // Valore di ipoteca
    float Mortgage;
    // Rendita di: terreno, 1 casa, 2 case, 3 case, 4 case, 1 albergo
    float[] Rent = new float[6];

    // Numero di costruzioni edificate
    int Buildings;

    public Property(Cell RelCell, string Name, Group newGroup, float[] Cost, float[] Rent, PROPERTY_TYPE PropertyType)
    {
        this.RelCell = RelCell;
		RelCell.setProperty(this);
        this.Name = Name;
        this.RelGroup = newGroup;
		newGroup.addProperty(this);
        this.Cost = Cost;
        this.Mortgage = Cost[0]/2;
        this.Rent = Rent;
        this.PropertyType = PropertyType;
        this.Owner = null;
        this.Buildings = 0;
    }

    // SETTERS
        // Metodo che imposta il gruppo di appartenenza
        public void setGroup(Group newGroup)
        {
            RelGroup = newGroup;
        }
        // Metodo che imposta il proprietario della cella
        public void setOwner(Player newOwner)
        {
            this.Owner = newOwner;
        }

    // GETTERS
        // Metodo che restituisce il gruppo
        public Group getGroup()
        {
            return RelGroup;
        }
        // Metodo che restituisce il proprietario
        public Player getOwner()
        {
            return this.Owner;
        }
        // Metodo che restituisce la cella di riferimento
        public Cell getCell()
        {
            return this.RelCell;
        }
        // Metodo che restituisce il nome
        public string getName()
        {
            return this.Name;
        }
		// Metodo che restituisce il numero di costruzioni
		public int getNumberOfBuildings() {
			return Buildings;
		}
	
    // PROPERTY VALUES GETTERS
        // Metodo che restituisce il costo del terreno vuoto
        public float getPropertyCost()
        {
            return this.Cost[0];
        }
        // Metodo che restituisce il costo di 1 casa
        public float getHouseCost()
        {
            return this.Cost[1];
        }
        // Metodo che restituisce il costo di 1 albergo
        public float getHotelCost()
        {
            return this.Cost[2];
        }
		// Metodo che restituisce il costo di X costruzioni (da 0 a 5) sul terreno
		public float getBuildingsCost(int numberOfBuildings) {
			if (numberOfBuildings >= 0 && numberOfBuildings <= 4) {
				return numberOfBuildings*getHouseCost();
			} else if (numberOfBuildings == 5) {
				return (4*getHouseCost())+getHotelCost();
			}
			return 0;		
		}
        // Metodo che restituisce la rendita attuale del terreno
        public float getRent()
        {
            float calculatedRent = 0;

            if (this.PropertyType == PROPERTY_TYPE.CITY)
            {	
				// Se il proprietario del terreno corrente possiede tutti i terreni del lotto e se non ci sono costruzioni
				if ((this.Buildings == 0) && (this.getOwner().countGroupPropertiesOwned(this.RelGroup) == this.RelGroup.getPropertyList().Count)) {
					// la rendita raddoppia
					calculatedRent = 2 * this.Rent[this.Buildings];
				}
				else {
	                // Calcolo la rendita in base al numero di costruzioni edificate
	                calculatedRent = this.Rent[this.Buildings];
				}
            }
            else if ((this.PropertyType == PROPERTY_TYPE.SOCIETY) || (this.PropertyType == PROPERTY_TYPE.AIRPORT))
            {
                // Ottengo il proprietario dell'aeroporto/societa'
                Player currentOwner = this.getOwner();
                // Se l'aeroporto/societa' e' posseduto dal giocatore o dal computer
                if ((currentOwner.getPlayerType() != Player.PLAYER_TYPE.BANK) && (currentOwner.getPlayerType() != null))
                {
                    // Conto quanti altri aeroporti/societa' il Player possiede nel gruppo
                    int propertiesNumber = currentOwner.countGroupPropertiesOwned(this.getGroup());
                    // Restituisco il valore appropriato dell'array
                    // (dall'indice 0 a 3 per numero aeroporti da 1 a 4, dall'indice 0 a 1 per numero di societa' da 1 a 2)
                    calculatedRent = this.Rent[propertiesNumber-1];
                }
                else
                    calculatedRent = 0;
            }

            return calculatedRent;
        }
        // Metodo che restituisce il valore di ipoteca del terreno
        public float getMortgage()
        {
            return this.Mortgage;
        }

    // Metodo che controlla se la proprieta' e' acquisita o meno
    public bool isOwned()
    {
        return ((Owner != null) && (Owner.getPlayerType() != Player.PLAYER_TYPE.BANK));
    }
    // Metodo che controlla se una proprieta' e' edificabile
    public bool isBuilding()
    {
        return ((this.Buildings <= 5) && (this.PropertyType == PROPERTY_TYPE.CITY));
    }
	// Metodo che avvia l'acquisto di edifici
	public float build(int numberOfBuildings) {
		this.Buildings += numberOfBuildings;
		return (numberOfBuildings*getHouseCost());
	}
	// Metodo che avvia la vendita di edifici
	public float sell(int numberOfBuildings)  {
		this.Buildings -= numberOfBuildings;
		return (numberOfBuildings*getHouseCost()/2);
	}
}
