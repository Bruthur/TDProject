/* 
This Script can easily to replace the Terrain Splatmap or Lightmap, (Not Undoable!)
It is usefull if you have multiple Splatmaps, just select which Splatmap you want to be replaced.

If  the Backup option checked: the orginal Splatmap will saved to your project folder (parent of the assets folder). 

The format of the new Splatmap should be Uncompressed: 
- it must be ARGB 32 bit and Power of Two (RGB 24 bit for Lightmap)
- "Is Readable" must be checked when imported.
 
 *If New Terrain doesn't show the Splatmap, create the Terrain Lightmap, then it will show it.
 
 BTW, if you just want to Export a Terrain Map, use the script "LightmapExport.cs" from Island Demo
*/ 

import System.IO;

class ReplaceSplatmap extends ScriptableWizard
{
var Splatmap: Texture2D;
var New : Texture2D;
var BackupSplatmap: boolean;

 function OnWizardUpdate(){
        helpString = "'New' Splatmap will OverWrite the 'Splatmap' \nDrag the Texture to here:";
        isValid = (Splatmap != null) && (New != null);
    }
	
function OnWizardCreate () {
   if (BackupSplatmap){
		Backup ();
   } 
   
   	if (New.format != TextureFormat.ARGB32 && New.format != TextureFormat.RGB24) {
		EditorUtility.DisplayDialog("Wrong format", "Splatmap must be in ARGB 32 bit (RGB 24 bit for Lightmap) format", "Cancel"); 
		return;
	}
	
	var w = New.width;
	if (Mathf.ClosestPowerOfTwo(w) != w) {
		EditorUtility.DisplayDialog("Wrong size", "Splatmap width and height must be a power of two", "Cancel"); 
		return;	
	}  
   Splatmap.Resize (New.width, New.height, New.format, true);
   pixels = New.GetPixels ();
   Splatmap.SetPixels (pixels);
   Splatmap.Apply(); 
}

    @MenuItem("CUSTOM/Replace Splatmap...")//, false, 4)
    static function Replace (){
        ScriptableWizard.DisplayWizard(
            "ReplaceSplatmap", ReplaceSplatmap, "Replace");
    }
}

function Backup () {
	filename = Splatmap.name;
	var bytes = Splatmap.EncodeToPNG();
	File.WriteAllBytes(Application.dataPath + "/../"+filename +"_Backup.png", bytes);
	EditorUtility.DisplayDialog("Texture Backup Saved!", "Backup of "+"'"+filename+"'"+" saved in Current Project Folder: " + filename+ "_Backup.png", "OK");	
}		