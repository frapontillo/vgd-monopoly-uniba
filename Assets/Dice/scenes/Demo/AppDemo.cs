/**
 * Copyright (c) 2010-2015, WyrmTale Games and Game Components
 * All rights reserved.
 * http://www.wyrmtale.com
 *
 * THIS SOFTWARE IS PROVIDED BY WYRMTALE GAMES AND GAME COMPONENTS 'AS IS' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL WYRMTALE GAMES AND GAME COMPONENTS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */ 
using UnityEngine;
using System.Collections;

// Demo application script
public class AppDemo : MonoBehaviour {

    //// constant of current demo mode
    //private const int MODE_GALLERY = 1;
    //private const int MODE_ROLL = 2;
    //// current demo mode
    //private int mode = 0;
    //// next camera position when moving the camera after switching mode
    //private GameObject nextCameraPosition = null;
    //// start camera position when moving the camera after switching mode
    //private GameObject startCameraPosition = null;
    //// store gameObject (empty) for mode MODE_ROLL camera position
    //private GameObject camRoll = null;
    //// store gameObject (empty) for mode MODE_GALLERY camera position
    //private GameObject camGallery = null;
    //// speed of moving camera after switching mode
    //private float cameraMovementSpeed = 0.8F;
    //private float cameraMovement = 0;

	// initial/starting die in the gallery
	private string galleryDie = "d6-red";
	private GameObject galleryDieObject = null;
    public bool roll = false;

    public int cont = 0;
    
    //// handle drag rotating the die in the gallery
    //private bool dragging = false;
    //private Vector2 dragStart;
    //private Vector3 dragStartAngle;
    //private Vector3 dragLastAngle;
	
    //// rectangle GUI area's 
    //private Rect rectGallerySelectBox;
    //private Rect rectGallerySelect;	
    //private Rect rectModeSelect;

    //// GUI gallery die selector texture
    //private Texture txSelector = null;
    
    //// Use this for initialization
    //void Start () {
    //    if (startDice)
    //    {
    //        // set (first) mode to gallery
    //        SetMode(MODE_ROLL);
    //    }
    //}	
	
    //private void SetMode(int pMode)
    //{		
    //    mode = pMode;	
    //}

    //// Moving the camera
    //void MoveCamera()
    //{
    //    // increment total moving time
    //    cameraMovement += Time.deltaTime * 1;
    //    // if we surpass the speed we have to cap the movement because we are 'slerping'
    //    if (cameraMovement>cameraMovementSpeed) 
    //        cameraMovement = cameraMovementSpeed;

    //    // slerp (circular interpolation) the position between start and next camera position
    //    Camera.main.transform.position = Vector3.Slerp(startCameraPosition.transform.position, nextCameraPosition.transform.position,  cameraMovement / cameraMovementSpeed );
    //    // slerp (circular interpolation) the rotation between start and next camera rotation
    //    Camera.main.transform.rotation = Quaternion.Slerp(startCameraPosition.transform.rotation, nextCameraPosition.transform.rotation,  cameraMovement / cameraMovementSpeed );

    //    // stop moving if we arrived at the desired next camera postion
    //    if (cameraMovement == cameraMovementSpeed)
    //        nextCameraPosition = null;	
    //}


    // dertermine random rolling force
    private GameObject spawnPoint = null;
    private Vector3 Force()
    {
        Vector3 rollTarget = Vector3.zero + new Vector3(2 + 7 * Random.value, .5F + 4 * Random.value, -2 - 3 * Random.value);
        return Vector3.Lerp(spawnPoint.transform.position, rollTarget, 1).normalized * (-35 - Random.value * 20);
    }

    public void Roll()
    {
        UpdateRoll();
        roll = false;
    }

	void UpdateRoll()
	{
        spawnPoint = GameObject.Find("spawnPoint");
        // right mouse button clicked so roll 8 dice of dieType 'gallery die'
        Dice.Clear();
        string[] a = galleryDie.Split('-');
        Dice.Roll("2" + a[0], galleryDie, spawnPoint.transform.position, Force());
    }

    void Update()
    {
        //Debug.Log(roll);
        if (Dice.Count("") > 0 && roll== true)
        {
                if (!Dice.IsRolling())
                {
                    roll = false;
                    check();
                }
        }
        //else if (roll && !(Dice.IsRolling()) && !(Dice.Valid()))
        //{
        //    roll = false;
        //    GameObject.Find("GameManager").GetComponent<GameManager>().SendMessage("RollingCompleted", -1);
        //}
    }

    void check()
    {
		int number = Dice.Value("");
		int uguali = Dice.Equals() ? 1 : 0;
		
		int[] arr = {number,uguali};
        GameObject.Find("GameManager").GetComponent<GameManager>().SendMessage("RollingCompleted", arr);
    }

    //// handle GUI
    //void OnGUI()
    //{
    //    // display rolling message on bottom
    //    GUI.Box(new Rect((Screen.width - 520) / 2, Screen.height - 40, 520, 25), "");
    //    GUI.Label(new Rect(((Screen.width - 520) / 2) + 10, Screen.height - 38, 520, 22), "Il dado è in movimento? " + Dice.Movimento());
    //    if (Dice.Count("") > 0)
    //    {
    //        // we have rolling dice so display rolling status
    //        GUI.Box(new Rect(10, Screen.height - 75, Screen.width - 20, 30), "");
    //        GUI.Label(new Rect(20, Screen.height - 70, Screen.width, 20), "La somma dei dadi è: " + Dice.AsString(""));
    //        GUI.Label(new Rect(20, Screen.height - 100, Screen.width, 20), "Il lancio è valido? " + Dice.Valid());
    //    }
    //}

    //// check if a point is within a rectangle
    //private bool PointInRect(Vector2 p, Rect r)
    //{
    //    return  (p.x>=r.xMin && p.x<=r.xMax && p.y>=r.yMin && p.y<=r.yMax);
    //}
	

    //// translate Input mouseposition to GUI coordinates using camera viewport
    //private Vector2 GuiMousePosition()
    //{
    //        Vector2 mp = Input.mousePosition;
    //        Vector3 vp = Camera.main.ScreenToViewportPoint (new Vector3(mp.x,mp.y,0));
    //        mp = new Vector2(vp.x * Camera.main.pixelWidth, (1-vp.y) * Camera.main.pixelHeight);
    //        return mp;
    //}

    void Start()
    {
        cont = 0;
        roll = true;
    }
		
}
