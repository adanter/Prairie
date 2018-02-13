using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
 
public class GrassCreator : ScriptableWizard  {
 
    public Terrain terrain;
    public int detailIndexToMassPlace;
    public int[] splatTextureIndicesToAffect;
    public int detailCountPerDetailPixel = 0;
       
    [MenuItem ("Terrain/Mass Grass Placement")]
   
    static void createWizard() {
       
            ScriptableWizard.DisplayWizard("Select terrain to put grass on", typeof (GrassCreator), "Place Grass on Terrain");
       
    }
   
    void OnWizardCreate () {
       
        if (!terrain) {
            Debug.Log("You have not selected a terrain object");
            return;
        }
       
        if (detailIndexToMassPlace >= terrain.terrainData.detailPrototypes.Length) {
            Debug.Log("You have chosen a detail index which is higher than the number of detail prototypes in your detail libary. Indices starts at 0");
            return;
        }
       
        if (splatTextureIndicesToAffect.Length > terrain.terrainData.splatPrototypes.Length) {
            Debug.Log("You have selected more splat textures to paint on, than there are in your libary.");
            return;
        }
       
        for (int i = 0; i < splatTextureIndicesToAffect.Length; i ++) {
            if (splatTextureIndicesToAffect[i] >= terrain.terrainData.splatPrototypes.Length) {
                Debug.Log("You have chosen a splat texture index which is higher than the number of splat prototypes in your splat libary. Indices starts at 0");
                return;
            }
        }
       
        if (detailCountPerDetailPixel > 16) {
            Debug.Log("You have selected a non supported amount of details per detail pixel. Range is 0 to 16");
            return;
        }
       
        int alphamapWidth = terrain.terrainData.alphamapWidth;
        int alphamapHeight = terrain.terrainData.alphamapHeight;
        int detailWidth = terrain.terrainData.detailResolution;
        int detailHeight = detailWidth;
       
        float resolutionDiffFactor = (float)alphamapWidth/detailWidth;
       
       
        float[,,] splatmap = terrain.terrainData.GetAlphamaps(0,0,alphamapWidth,alphamapHeight);
       
       
        int[,] newDetailLayer = new int[detailWidth,detailHeight];
       
        //loop through splatTextures
        for (int i = 0; i < splatTextureIndicesToAffect.Length; i++) {
           
            //find where the texture is present
            for (int j = 0; j < detailWidth; j++) {
           
                for (int k = 0; k < detailHeight; k++) {
                       
                    float alphaValue = splatmap[(int)(resolutionDiffFactor*j),(int)(resolutionDiffFactor*k),splatTextureIndicesToAffect[i]];
                       
                    newDetailLayer[j,k] = (int)Mathf.Round(alphaValue * ((float)detailCountPerDetailPixel)) + newDetailLayer[j,k];
           
                }
               
            }
           
        }
       
        terrain.terrainData.SetDetailLayer(0,0,detailIndexToMassPlace,newDetailLayer); 
       
    }
   
    void OnWizardUpdate ()
    {
        helpString = "Ready";
       
   
       
       
    }
   
}