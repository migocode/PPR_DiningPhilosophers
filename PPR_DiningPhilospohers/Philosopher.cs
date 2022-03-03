using System.Diagnostics;

namespace PPR_DiningPhilospohers
{
    public record Philosopher
    {
        private readonly int thinkingTime;
        private readonly int eatingTime;

        private bool leftForkAcquired;
        private bool rightForkAcquired;

        private readonly Random random;
        private readonly Stopwatch waitStopwatch;
        private readonly Stopwatch executionStopwatch;

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
            waitStopwatch = new Stopwatch();
            executionStopwatch = new Stopwatch();
        }

        public void Eat(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Philosopher: {this.ToString()}");

            executionStopwatch.Start();
            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    int thinkingTime = GetThinkingTime();
                    Thread.Sleep(thinkingTime);
                    Console.WriteLine($"Philosopher {PhilosopherNumber} finished thinking after {thinkingTime}ms.");

                    if (PhilosopherNumber % 2 == 0)
                    {
                        TakeLeftFork();
                        Console.WriteLine($"Philosopher {PhilosopherNumber} took left fork {LeftFork.ForkNumber}.");

                        TakeRightFork();
                        Console.WriteLine($"Philosopher {PhilosopherNumber} took right fork {RightFork.ForkNumber}.");
                    }
                    else
                    {
                        TakeRightFork();
                        Console.WriteLine($"Philosopher {PhilosopherNumber} took right fork {RightFork.ForkNumber}.");

                        TakeLeftFork();
                        Console.WriteLine($"Philosopher {PhilosopherNumber} took left fork {LeftFork.ForkNumber}.");
                    }

                    int eatingTime = GetEatingTime();
                    Thread.Sleep(eatingTime);

                    Console.WriteLine($"Philosopher {PhilosopherNumber} is done eating.");
                }
                finally
                {
                    PutBackForks();
                }
            }
            executionStopwatch.Stop();

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

        private void TakeLeftFork()
        {
            leftForkAcquired = false;
            try
            {
                waitStopwatch.Start();
                Monitor.Enter(LeftFork, ref leftForkAcquired);
                waitStopwatch.Stop();
                LeftFork.IsUsed = true;
            }
            catch
            {
                if (leftForkAcquired)
                {
                    Monitor.Exit(LeftFork);
                }
            }
        }

        private void TakeRightFork()
        {
            rightForkAcquired = false;
            try
            {
                waitStopwatch.Start();
                Monitor.Enter(RightFork, ref rightForkAcquired);
                waitStopwatch.Stop();
                RightFork.IsUsed = true;
            }
            catch
            {
                if (rightForkAcquired)
                {
                    Monitor.Exit(RightFork);
                }
            }
        }

        public long GetWaitTime()
        {
            return waitStopwatch.ElapsedMilliseconds;
        }

        public long GetExecutionTime()
        {
            return executionStopwatch.ElapsedMilliseconds;
        }

        private void PutBackForks()
        {
            if (rightForkAcquired)
            {
                Monitor.Exit(RightFork);
            }
            if (leftForkAcquired)
            {
                Monitor.Exit(LeftFork);
            }
        }
    }

    public record Fork
    {
        public int ForkNumber { get; }
        public bool IsUsed { get; set; }

        public Fork(int forkNumber)
        {
            ForkNumber = forkNumber;
        }
    }
}
