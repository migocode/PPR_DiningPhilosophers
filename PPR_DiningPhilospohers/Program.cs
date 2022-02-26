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
Stopwatch stopwatch = Stopwatch.StartNew();

stopwatch.Start();
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
stopwatch.Stop();
cts.Cancel();
Console.WriteLine("Cancelling philosophers...");

Task.WaitAll(philosophers);

long totalWaitTimes = 0;
foreach (Philosopher philosopher in finishedPhilosophers)
{
    Console.WriteLine($"Philosopher {philosopher.PhilosopherNumber} waited for {philosopher.GetWaitTimes():N}ms");
    totalWaitTimes += philosopher.GetWaitTimes();
}
Console.WriteLine($"Total waittimes: {totalWaitTimes:N}ms / Total run time: {stopwatch.ElapsedMilliseconds:N}ms");

Console.WriteLine("All philosophers cancelled.");