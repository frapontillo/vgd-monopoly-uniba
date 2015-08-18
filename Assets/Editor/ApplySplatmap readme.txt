In order to apply splatmaps exported from Fractscape to terrains made in Unity, you'll need the ApplySplatmap.js script, which adds this ability to Unity.  Follow these steps, using Unity 2.6 or later:

1) If you don't already have an Editor folder in your current project, make one.
2) Copy the ApplySplatmap.js script to this Editor folder and wait for script compilation to finish.
3) Make sure your terrain in Unity has at least 4 textures.
4) Copy the splatmap you want to use to your project, if it's not saved there already.
5) Click once on the splatmap in the Project pane. In the Inspector pane, change the Texture Format to ARGB 32 bit, make sure the Is Readable box is checked, then click Apply. 
6) While the texture is still highlighted, select Apply Splatmap from the Terrain menu.
7) [optional] if exporting a splatmap using the ExportTexture tool, your terrains splat may need to be rotated in some versions of Unity, use the rotImage var to rotate the splatmap on export so that when you apply it, it gets applied correctly. You may have a 1 pixel edge that needs cleaned up. Read top of ExportTexture.js for ideas on how to clean up the image even more. 0, 90, 180 and 270 are accepted values for rotating the splatmap.

This will make the splatmap you selected be applied to the currently active terrain. Any splatmap that already exists will be overwritten.

Note that the ApplySplatmap.js script uses APIs that are currently undocumented, as of Unity version 2.6.1. Future versions of Unity may cause this script to stop functioning, although every attempt will be made to provide an updated version of the script in this event.  You should avoid using these APIs in your own projects for this reason, especially web players, where updated versions of the Unity web plugin will cause your web player to stop working.  Use of the actual splatmaps themselves is not a problem in any case Ñ once they are applied by the script, they are the same as splatmaps you create in Unity and will always work.
