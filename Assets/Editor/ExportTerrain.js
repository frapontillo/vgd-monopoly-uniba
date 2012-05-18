import System.IO;
import System.Text;

enum SaveFormat {Triangles, Quads}
enum SaveResolution {Full, Half, Quarter, Eighth, Sixteenth}

class ExportTerrain extends EditorWindow {
    var saveFormat = SaveFormat.Triangles;
    var saveResolution = SaveResolution.Half;
    static var terrain : TerrainData;
    static var terrainPos : Vector3;
    
    var tCount : int;
    var counter : int;
    var totalCount : int;
        var progressUpdateInterval = 10000;
    
    @MenuItem ("Terrain/Export To Obj...")
    static function Init () {
        terrain = null;
        var terrainObject : Terrain = Selection.activeObject as Terrain;
        if (!terrainObject) {
            terrainObject = Terrain.activeTerrain;
        }
        if (terrainObject) {
            terrain = terrainObject.terrainData;
            terrainPos = terrainObject.transform.position;
        }
        EditorWindow.GetWindow(ExportTerrain).Show();
    }
    
    function OnGUI () {
        if (!terrain) {
            GUILayout.Label("No terrain found");
            if (GUILayout.Button("Cancel")) {
                EditorWindow.GetWindow(ExportTerrain).Close();
            }
            return;
        }
        saveFormat = EditorGUILayout.EnumPopup("Export Format", saveFormat);
        saveResolution = EditorGUILayout.EnumPopup("Resolution", saveResolution);
        
        if (GUILayout.Button("Export")) {
            Export();
        }
    }
    
    function Export () {
        var fileName = EditorUtility.SaveFilePanel("Export .obj file", "", "Terrain", "obj");
        var w = terrain.heightmapWidth;
        var h = terrain.heightmapHeight;
        var meshScale = terrain.size;
        var tRes = Mathf.Pow(2, parseInt(saveResolution));
        meshScale = Vector3(meshScale.x/(w-1)*tRes, meshScale.y, meshScale.z/(h-1)*tRes);
        var uvScale = Vector2(1.0/(w-1), 1.0/(h-1));
        var tData = terrain.GetHeights(0, 0, w, h);
        
        w = (w-1) / tRes + 1;
        h = (h-1) / tRes + 1;
        var tVertices = new Vector3[w * h];
        var tUV = new Vector2[w * h];
        if (saveFormat == SaveFormat.Triangles) {
            var tPolys = new int[(w-1) * (h-1) * 6];
        }
        else {
            tPolys = new int[(w-1) * (h-1) * 4];
        }
        
        // Build vertices and UVs
        for (y = 0; y < h; y++) {
            for (x = 0; x < w; x++) {
                tVertices[y*w + x] = Vector3.Scale(meshScale, Vector3(x, tData[x*tRes,y*tRes], y)) + terrainPos;
                tUV[y*w + x] = Vector2.Scale(Vector2(x*tRes, y*tRes), uvScale);
            }
        }
    
        var index = 0;
        if (saveFormat == SaveFormat.Triangles) {
            // Build triangle indices: 3 indices into vertex array for each triangle
            for (y = 0; y < h-1; y++) {
                for (x = 0; x < w-1; x++) {
                    // For each grid cell output two triangles
                    tPolys[index++] = (y     * w) + x;
                    tPolys[index++] = ((y+1) * w) + x;
                    tPolys[index++] = (y     * w) + x + 1;
        
                    tPolys[index++] = ((y+1) * w) + x;
                    tPolys[index++] = ((y+1) * w) + x + 1;
                    tPolys[index++] = (y     * w) + x + 1;
                }
            }
        }
        else {
            // Build quad indices: 4 indices into vertex array for each quad
            for (y = 0; y < h-1; y++) {
                for (x = 0; x < w-1; x++) {
                    // For each grid cell output one quad
                    tPolys[index++] = (y     * w) + x;
                    tPolys[index++] = ((y+1) * w) + x;
                    tPolys[index++] = ((y+1) * w) + x + 1;
                    tPolys[index++] = (y     * w) + x + 1;
                }
            }   
        }
    
        // Export to .obj
        try {
            var sw = new StreamWriter(fileName);
            sw.WriteLine("# Unity terrain OBJ File");
            
            // Write vertices
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            counter = tCount = 0;
            totalCount = (tVertices.Length*2 + (saveFormat == SaveFormat.Triangles? tPolys.Length/3 : tPolys.Length/4)) / progressUpdateInterval;
            for (i = 0; i < tVertices.Length; i++) {
                UpdateProgress();
                var sb = StringBuilder("v ", 20);
                // StringBuilder stuff is done this way because it's faster than using the "{0} {1} {2}"etc. format
                // Which is important when you're exporting huge terrains.
                sb.Append(tVertices[i].x.ToString()).Append(" ").
                   Append(tVertices[i].y.ToString()).Append(" ").
                   Append(tVertices[i].z.ToString());
                sw.WriteLine(sb);
            }
            // Write UVs
            for (i = 0; i < tUV.Length; i++) {
                UpdateProgress();
                sb = StringBuilder("vt ", 22);
                sb.Append(tUV[i].x.ToString()).Append(" ").
                   Append(tUV[i].y.ToString());
                sw.WriteLine(sb);
            }
            if (saveFormat == SaveFormat.Triangles) {
                // Write triangles
                for (i = 0; i < tPolys.Length; i += 3) {
                    UpdateProgress();
                    sb = StringBuilder("f ", 43);
                    sb.Append(tPolys[i]+1).Append("/").Append(tPolys[i]+1).Append(" ").
                       Append(tPolys[i+1]+1).Append("/").Append(tPolys[i+1]+1).Append(" ").
                       Append(tPolys[i+2]+1).Append("/").Append(tPolys[i+2]+1);
                    sw.WriteLine(sb);
                }
            }
            else {
                // Write quads
                for (i = 0; i < tPolys.Length; i += 4) {
                    UpdateProgress();
                    sb = StringBuilder("f ", 57);
                    sb.Append(tPolys[i]+1).Append("/").Append(tPolys[i]+1).Append(" ").
                       Append(tPolys[i+1]+1).Append("/").Append(tPolys[i+1]+1).Append(" ").
                       Append(tPolys[i+2]+1).Append("/").Append(tPolys[i+2]+1).Append(" ").
                       Append(tPolys[i+3]+1).Append("/").Append(tPolys[i+3]+1);
                    sw.WriteLine(sb);
                }      
            }
        }
        catch (err) {
            Debug.Log("Error saving file: " + err.Message);
        }
        sw.Close();
        
        terrain = null;
        EditorUtility.ClearProgressBar();
        EditorWindow.GetWindow(ExportTerrain).Close();
    }
    
    function UpdateProgress () {
        if (counter++ == progressUpdateInterval) {
            counter = 0;
            EditorUtility.DisplayProgressBar("Saving...", "", Mathf.InverseLerp(0, totalCount, ++tCount));
        }
    }
}