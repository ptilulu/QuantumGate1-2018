using UnityEngine;
using UnityEngine.UI;

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

        int i = 1;

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
        }

	}
}
