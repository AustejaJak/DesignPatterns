using System;

namespace BloonsProject
{
    public class ConsoleController : IProgramController // Example of how interface can be utilized for another project - Demonstrates understanding of OOP principles and interfaces.
    {
        private bool isRunning;

        public event Action LoseEventHandler;

        public event Action PauseEventHandler;

        public void SetIsGameRunningTo(bool isRunning)
        {
            this.isRunning = isRunning;
        }

        public void Start()
        {
            Console.Write("Starting game!");

            var input = Console.ReadLine();

            if (input.Equals("pause"))
            {
                PauseEventHandler.Invoke();
            }

            if (input.Equals("lose"))
            {
                LoseEventHandler.Invoke();
            }
        }
    }
}