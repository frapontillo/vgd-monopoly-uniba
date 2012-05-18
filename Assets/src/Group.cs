using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Group {
    // Nome del gruppo
    private string Name;
    // Numero di lotti nel gruppo
    private int Dimension = 0;
    // Descrizione del colore
    private string ColorDescription;
    // Colore in esadecimale
    private Color TrueColor;
    // Lista di proprieta'
    private List<Property> propertyList = new List<Property>();

    // Costruttore della classe
    public Group(string Name, string ColorDescription, Color TrueColor)
    {
        this.Name = Name;
        this.ColorDescription = ColorDescription;
        this.TrueColor = TrueColor;
    }

    public string getName() {
        return Name;
    }

    public int getDimension() {
        return Dimension;
    }

    public string getColorDescription() {
        return ColorDescription;
    }

    public Color getTrueColor() {
        return TrueColor;
    }

    public void addProperty(Property newProperty) {
        propertyList.Add(newProperty);
        newProperty.setGroup(this);
        Dimension++;
    }

    public List<Property> getPropertyList()
    {
        return propertyList;
    }
	
	/** Metodo che restituisce il costo preventivato per la costruzione di un certo
	 * numero di edifici sul lotto. **/
	public float estimateBuildingCost(int numberOfBuildings) {
		float estimatedCost = 0;
		for (int i = 0; i < this.Dimension; i++) {
			estimatedCost += propertyList[i].getBuildingsCost(numberOfBuildings);
		}
		return estimatedCost;
	}
}
