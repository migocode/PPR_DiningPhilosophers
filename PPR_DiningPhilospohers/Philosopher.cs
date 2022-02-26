using System.Diagnostics;

namespace PPR_DiningPhilospohers
{
    public record Philosopher
    {
        private readonly int thinkingTime;
        private readonly int eatingTime;

        private readonly Random random;
        private readonly Stopwatch stopwatch;

        public Fork LeftFork { get; }
        public Fork RightFork { get; }
        public int PhilosopherNumber { get; }

        public Philosopher(int philosopherNumber, Fork leftFork, Fork rightFork, int thinkingTime, int eatingTime)
        {
            this.LeftFork = leftFork;
            this.RightFork = rightFork;
            this.PhilosopherNumber = philosopherNumber;

            this.thinkingTime = thinkingTime;
            this.eatingTime = eatingTime;

            random = new Random();
            stopwatch = new Stopwatch();
        }

        public void Eat(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Philosopher: {this.ToString()}");

            while (cancellationToken.IsCancellationRequested == false)
            {
                int thinkingTime = GetThinkingTime();
                Thread.Sleep(thinkingTime);
                Console.WriteLine($"Philosopher {PhilosopherNumber} finished thinking after {thinkingTime}ms.");

                if (PhilosopherNumber % 2 == 0)
                {
                    TakeFork(LeftFork);
                    Console.WriteLine($"Philosopher {PhilosopherNumber} took left fork {LeftFork.ForkNumber}.");

                    TakeFork(RightFork);
                    Console.WriteLine($"Philosopher {PhilosopherNumber} took right fork {RightFork.ForkNumber}.");
                }
                else
                {
                    TakeFork(RightFork);
                    Console.WriteLine($"Philosopher {PhilosopherNumber} took right fork {RightFork.ForkNumber}.");

                    TakeFork(LeftFork);
                    Console.WriteLine($"Philosopher {PhilosopherNumber} took left fork {LeftFork.ForkNumber}.");
                }

                int eatingTime = GetEatingTime();
                Thread.Sleep(eatingTime);

                Console.WriteLine($"Philosopher {PhilosopherNumber} is done eating.");
                PutBackForks(LeftFork, RightFork);
            }

            Console.WriteLine($"Philosopher {PhilosopherNumber} cancelled.");
        }

        private int GetEatingTime()
        {
            return random.Next(0, eatingTime);
        }

        private int GetThinkingTime()
        {
            return random.Next(0, thinkingTime);
        }

        private void TakeFork(Fork fork)
        {
            stopwatch.Start();
            Monitor.Enter(fork);
            stopwatch.Stop();
            fork.IsUsed = true;
        }

        public long GetWaitTimes()
        {
            return stopwatch.ElapsedMilliseconds;
        }

        private void PutBackForks(Fork fork1, Fork fork2)
        {
            Monitor.Exit(fork1);
            Monitor.Exit(fork2);
        }
    }
}
