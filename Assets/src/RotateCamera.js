static var rotationAmount: float = 5.0;
var rotate: boolean;

function Update () {
	if (rotate) {
		transform.RotateAround (GameObject.Find("rotateAroundThis").transform.position, Vector3.up, rotationAmount * Time.deltaTime);
	}
}

public function startRotate() {
	rotate = true;
}

public function stopRotate() {
	rotate = false;
}

function Start() {
	stopRotate();
}