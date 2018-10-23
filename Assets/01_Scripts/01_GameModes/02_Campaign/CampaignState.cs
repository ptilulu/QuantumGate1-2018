using System.Collections.Generic;
using UnityEngine;
using System.IO;

/**
* Garde la progression du joueur dans la campagne et
* les stats de la campagne
* */
public class CampaignState : MonoBehaviour
{

    public class ChapterStat
    {
		public int countLevels;
		public int levelsFinished;

        public ChapterStat(int countLevels, int levelsFinished)
        {
            if (countLevels < levelsFinished) countLevels = levelsFinished;
            this.countLevels = countLevels;
            this.levelsFinished = levelsFinished;
        }
    }

    public static Dictionary<string, ChapterStat> chapterStats;

	void Start ()
    {
        chapterStats = new Dictionary<string, ChapterStat>();
        DirectoryInfo dir = new DirectoryInfo("Assets/00_Scenes/Campaign");
        foreach (DirectoryInfo d in dir.GetDirectories())
            chapterStats.Add(d.Name, new ChapterStat(d.GetFiles("*.unity").Length, 0));
    }
}
