using PPR_DiningPhilospohers;
using System.Collections.Concurrent;
using System.Diagnostics;

Console.Write("Number of Philosophers: ");
int numberOfPhilosophers = int.Parse(Console.ReadLine());

Console.Write("Thinkingtime (ms): ");
int thinkingTime = int.Parse(Console.ReadLine());

Console.Write("Eatingtime (ms): ");
int eatingTime = int.Parse(Console.ReadLine());

Fork[] forks = new Fork[numberOfPhilosophers];
for(int i = 0; i < forks.Length; i++)
{
    forks[i] = new(i);
}

CancellationTokenSource cts = new CancellationTokenSource();

Task[] philosophers = new Task[numberOfPhilosophers];

ConcurrentBag<Philosopher> finishedPhilosophers = new ConcurrentBag<Philosopher>();

for (int i = 0; i < numberOfPhilosophers; i++)
{
    int leftForkIndex = i;
    int rightForkIndex = (i + 1) % forks.Length;
    int philosopherNumber = leftForkIndex;

    philosophers[philosopherNumber] = Task.Run(() =>
    {
        Philosopher philosopher = new(philosopherNumber, forks[leftForkIndex], forks[rightForkIndex], thinkingTime, eatingTime);
        philosopher.Eat(cts.Token);

        finishedPhilosophers.Add(philosopher);
    }, cts.Token);
}

Console.ReadKey();
cts.Cancel();
Console.WriteLine("Cancelling philosophers...");

Task.WaitAll(philosophers);

long totalWaitTime = 0;
long totalExecutionTime = 0;
foreach (Philosopher philosopher in finishedPhilosophers)
{
    Console.WriteLine($"Philosopher {philosopher.PhilosopherNumber} waited for {philosopher.GetWaitTime():N}ms");
    totalWaitTime += philosopher.GetWaitTime();
    totalExecutionTime += philosopher.GetExecutionTime();
}
Console.WriteLine($"Total waittimes: {totalWaitTime:N}ms / Total run time: {totalExecutionTime:N}ms");

Console.WriteLine("All philosophers cancelled.");