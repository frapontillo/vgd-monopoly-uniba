function Update () {
	var cur_camera : GameObject;
	cur_camera = GameObject.Find("camera");
	var n = cur_camera.transform.position - transform.position;
	transform.rotation = Quaternion.LookRotation(n) * Quaternion.Euler(90, 0, 0);
}