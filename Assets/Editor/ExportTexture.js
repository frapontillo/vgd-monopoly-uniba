/*
As well as GetPixel, Texture2D also has a GetPixelBilinear method. Image processing transforms like this inevitably want non-integer pixel coordinates from the source image. GetPixelBilinear handles this for you by returning a weighted average of four neighbouring pixels. The rotated texture will look much better if you use this rather than rounding pixel coordinates off to the nearest integer and using GetPixel.
*/

import System.IO;

@MenuItem("Assets/Export Texture")

static function Apply () {

   var rotImage:int = 0;
   var ang = rotImage * Mathf.Deg2Rad;
   
   var texture : Texture2D = Selection.activeObject as Texture2D;
   var ntexture = new Texture2D(texture.width,texture.height);
   
   for (var i = 0; i < texture.height; i++) { 
      for (var j = 0; j < texture.width; j++) {
           
         var x_new : int=(j) * Mathf.Cos(ang) - (i)*Mathf.Sin(ang);
         var y_new : int=(i) * Mathf.Cos(ang) + (j)*Mathf.Sin(ang);
           
         ntexture.SetPixel ( x_new, y_new, texture.GetPixel(i,j));
      }
   }
   ntexture.Apply();
   
   
   var bytes = ntexture.EncodeToPNG();
   File.WriteAllBytes(Application.dataPath + "/exported_texture_rot0.png", bytes);
}
