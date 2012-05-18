// For save splatmap as PNG file.
import System.IO;

@MenuItem("Assets/Export Texture")

static function Apply () {
   
   var texture : Texture2D = Selection.activeObject as Texture2D;
   if (texture == null)
   {
      EditorUtility.DisplayDialog("Select Texture", "You Must Select a Texture first!", "Ok");
      return;
   }
   
   var bytes = texture.EncodeToPNG();
   File.WriteAllBytes(Application.dataPath + "/exported_texture.png", bytes);
}