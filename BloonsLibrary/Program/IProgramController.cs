using System;

namespace BloonsProject
{
    public interface IProgramController
    {
        public void Start();

        public void SetIsGameRunningTo(bool isRunning);

        public event Action LoseEventHandler;

        public event Action PauseEventHandler;
    }
}