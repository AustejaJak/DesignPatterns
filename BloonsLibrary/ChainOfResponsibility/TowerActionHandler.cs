using BloonsProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonLibrary.ChainOfResponsibility
{
    public abstract class TowerActionHandler
    {
        protected TowerActionHandler _nextHandler;

        public void SetNext(TowerActionHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public abstract void Handle(string option, Tower tower, TowerGuiOptions towerOptions, GameState gameState, GameClient gameClient);
    }
}
