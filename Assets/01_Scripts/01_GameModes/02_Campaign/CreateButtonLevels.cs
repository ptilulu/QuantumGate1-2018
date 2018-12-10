using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml;

/**
 * Créer les boutons dans le menu pour accéder aux niveaux
 * */
public class CreateButtonLevels : MonoBehaviour
{
    public string chapterName;
    public Sprite lockSprite;
    public GameObject buttonPrefab;

	void Start ()
    {
        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();

        /* int i = 1;

         for (; i <= CampaignState.chapterStats[chapterName].levelsFinished; i++)
         {
             GameObject newButton = Instantiate(buttonPrefab);
             newButton.GetComponentInChildren<Text>().text = i.ToString();
             newButton.GetComponent<LoadScene>().sceneName = chapterName + i.ToString();
             newButton.transform.SetParent(grid.transform, false);
         }

         for (; i <= CampaignState.chapterStats[chapterName].countLevels; i++)
         {
             GameObject newButton = Instantiate(buttonPrefab);
             newButton.GetComponent<Image>().sprite = lockSprite;
             newButton.GetComponent<Button>().enabled = false;
             newButton.transform.SetParent(grid.transform, false);
         }*/

        //var levelCollection = LevelContainer.Load(Path.Combine(Application.dataPath, "Resources", "test.xml"));
        var levelCollection = new LevelContainer();
        if (Application.platform == RuntimePlatform.WindowsEditor) levelCollection = LevelContainer.Load(Path.Combine(Application.dataPath, "StreamingAssets", "levels.xml"));
        else if (Application.platform == RuntimePlatform.Android) levelCollection = LevelContainer.Load("jar:file://" + Application.dataPath + "!/assets/levels.xml");

        for (int i = 0; i < levelCollection.Levels.Count; i++)
        {
            
            GameObject newButton = Instantiate(buttonPrefab);
            newButton.GetComponentInChildren<Text>().text = levelCollection.Levels[i].id.ToString();
            newButton.GetComponent<LoadLevel>().level = levelCollection.Levels[i];
            newButton.GetComponent<LoadScene>().sceneName = "CampaignLevel";
            newButton.transform.SetParent(grid.transform, false);
        }

        GameMode.levelCollection = levelCollection;

    }
}
