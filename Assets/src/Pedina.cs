using UnityEngine;
using System.Collections;

public class Pedina : MonoBehaviour {

    string position;
    bool move = false;
    string nextposition;

	// Use this for initialization
	void Start () {

        position = "00";
	}
	
	// Update is called once per frame
    void Update()
    {

       if (move)
       {
           /* se la pedina deve andare nella seconda metà della tavola controlla la posizione della pedina e in
            * base alla sua posizione (prima metà o seconda metà) ne determina il movimento */
           if (nextposition[0] == '2' || nextposition[0] == '3')
           {
               if (position[0] == '2' || position[0] == '3')
               {
                   if (GameObject.Find(nextposition).transform.position.x != GameObject.Find("Pedina").transform.position.x)
                       GameObject.Find("Pedina").transform.Translate(Vector3.right * 2);
                   else if (GameObject.Find(nextposition).transform.position.z != GameObject.Find("Pedina").transform.position.z)
                       GameObject.Find("Pedina").transform.Translate(Vector3.back * 2);
                   else
                       stopMove();
               }
               else
               {

                   if (GameObject.Find("20").transform.position.x != GameObject.Find("Pedina").transform.position.x)
                       GameObject.Find("Pedina").transform.Translate(Vector3.left * 2);
                   else
                       if (GameObject.Find("20").transform.position.z != GameObject.Find("Pedina").transform.position.z)
                           GameObject.Find("Pedina").transform.Translate(Vector3.forward * 2);
                       else
                           position = "20";
               }

           }
           else
           {
               if (position[0] == '0' || position[0] == '1')
               {
                   if (GameObject.Find(nextposition).transform.position.x != GameObject.Find("Pedina").transform.position.x)
                       GameObject.Find("Pedina").transform.Translate(Vector3.left * 2);
                   else if (GameObject.Find(nextposition).transform.position.z != GameObject.Find("Pedina").transform.position.z)
                       GameObject.Find("Pedina").transform.Translate(Vector3.forward * 2);
                   else stopMove();
               }
               else
               {

                   if (GameObject.Find("00").transform.position.x != GameObject.Find("Pedina").transform.position.x)
                       GameObject.Find("Pedina").transform.Translate(Vector3.right * 2);
                   else
                       if (GameObject.Find("00").transform.position.z != GameObject.Find("Pedina").transform.position.z)
                           GameObject.Find("Pedina").transform.Translate(Vector3.back * 2);
                       else
                           position = "00";
               }

           }
       }
       else
           position = nextposition;
    }

    public void Move(int next)
    {

        if (next < 10)
            nextposition = "0" + next;
        else
            nextposition = "" + next;


        startMove();

    }

    public void startMove()
    {
        move = true;
    }

    public void stopMove()
    {
        move = false;
    }

    // restituisce un intero con la posizione
    public int getPosition()
    {
       return  int.Parse(position);
    }

    //sposta la pedina nella cella newpos (utile x il go to jail etc etc)
    public void setPosition(int newpos)
    {
        string value;

        if (newpos < 10)
            value = "0" + newpos;
        else
            value = "" + newpos;

        GameObject.Find("Pedina").transform.position = GameObject.Find(value).transform.position;
        position = value;
    }
}
