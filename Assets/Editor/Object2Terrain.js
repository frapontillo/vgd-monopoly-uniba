@MenuItem ("Terrain/Object to Terrain")

static function Object2Terrain () {
    // See if a valid object is selected
    var obj = Selection.activeObject as GameObject;
    if (obj == null) { 
        EditorUtility.DisplayDialog("No object selected", "Please select an object.", "Cancel");
        return;
    }
    if (obj.GetComponent(MeshFilter) == null) {
        EditorUtility.DisplayDialog("No mesh selected", "Please select an object with a mesh.", "Cancel");
        return;
    }
    else if ((obj.GetComponent(MeshFilter) as MeshFilter).sharedMesh == null) {
        EditorUtility.DisplayDialog("No mesh selected", "Please select an object with a valid mesh.", "Cancel");
        return;  
    }
    if (Terrain.activeTerrain == null) {
        EditorUtility.DisplayDialog("No terrain found", "Please make sure a terrain exists.", "Cancel");
        return;
    }   
    var terrain = Terrain.activeTerrain.terrainData;
    
    // If there's no mesh collider, add one (and then remove it later when done)
    var addedCollider = false;
    var addedMesh = false;
    var objCollider = obj.collider as MeshCollider;
    if (objCollider == null) {
        objCollider = obj.AddComponent(MeshCollider);
        addedCollider = true;
    }
    else if (objCollider.sharedMesh == null) {
        objCollider.sharedMesh = (obj.GetComponent(MeshFilter) as MeshFilter).sharedMesh;
        addedMesh = true;
    }
    
    Undo.RegisterUndo (terrain, "Object to Terrain");

    var resolutionX = terrain.heightmapWidth;
    var resolutionZ = terrain.heightmapHeight;
    var heights = terrain.GetHeights(0, 0, resolutionX, resolutionZ);
    
    // Use bounds a bit smaller than the actual object; otherwise raycasting tends to miss at the edges
    var objectBounds = objCollider.bounds;
    var leftEdge = objectBounds.center.x - objectBounds.extents.x + .01;
    var bottomEdge = objectBounds.center.z - objectBounds.extents.z + .01;
    var stepX = (objectBounds.size.x - .019) / resolutionX;
    var stepZ = (objectBounds.size.z - .019) / resolutionZ;
    
    // Set up raycast vars
    var y = objectBounds.center.y + objectBounds.extents.y + .01;
    var hit : RaycastHit;
    var ray = new Ray(Vector3.zero, -Vector3.up);
    var rayDistance = objectBounds.size.y + .02;
    var heightFactor = 1.0 / rayDistance;
        
    // Do raycasting samples over the object to see what terrain heights should be
    var z = bottomEdge;
    for (zCount = 0; zCount < resolutionZ; zCount++) {
        var x = leftEdge;
        for (xCount = 0; xCount < resolutionX; xCount++) {
            ray.origin = Vector3(x, y, z);
            if (objCollider.Raycast(ray, hit, rayDistance)) {
                heights[zCount, xCount] = 1.0 - (y - hit.point.y)*heightFactor;
            }
            else {
                heights[zCount, xCount] = 0.0;
            }
            x += stepX;
        }
        z += stepZ;
    }
    
    terrain.SetHeights(0, 0, heights);
    
    if (addedMesh) {
        objCollider.sharedMesh = null;
    }
    if (addedCollider) {
        DestroyImmediate(objCollider);
    }
}