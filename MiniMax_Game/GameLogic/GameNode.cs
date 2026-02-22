using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MiniMax_Game.GameLogic
{
    public class GameNode
    {
        public int Number;
        public int Score;
        public int Bank;
        public int CurrentPlayer;

        public int MoveWeight;
        public int Depth;
        public bool IsTerminal;
        public int HeuristicValue;

        public List<GameNode> Children = new List<GameNode>();
        public GameNode Parent;

        public GameNode(
            int number, 
            int score, 
            int bank,          
            int currentPlayer, 
            int moveWeight = 0,         
            int depth = 0,
            GameNode parent = null
        )
        {
            Number = number;
            Score = score;
            Bank = bank;
            CurrentPlayer = currentPlayer;
            MoveWeight = moveWeight;
            Depth = depth;
            Children = new List<GameNode>();
            Parent = parent;
            IsTerminal = number >= 3000;
        }
    }
}
