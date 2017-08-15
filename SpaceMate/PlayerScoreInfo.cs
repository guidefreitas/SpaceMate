using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceMate
{
    //Guarda a informação de score de um player
    public class PlayerScoreInfo
    {
        public String ClientUUID { get; set; }
        public int Deaths { get; set; }
        public int AsteroidsDestroied { get; set; }
    }
}
