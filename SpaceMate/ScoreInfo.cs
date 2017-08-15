using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceMate
{
    public class ScoreInfo
    {
        public List<PlayerScoreInfo> playersScoreInfo;

        public ScoreInfo()
        {
            this.playersScoreInfo = new List<PlayerScoreInfo>();
        }

        public int ScoreTotal()
        {
            int sumScore = 0;
            foreach(var playerScore in playersScoreInfo)
            {
                sumScore += playerScore.AsteroidsDestroied * 100;
            }
            return sumScore;
        }
    }
}
