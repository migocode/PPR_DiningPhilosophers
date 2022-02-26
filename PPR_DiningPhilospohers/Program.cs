using PPR_DiningPhilospohers;

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

for (int i = 0; i < numberOfPhilosophers; i++)
{
    int leftForkIndex = i;
    int rightForkIndex = (i + 1) % forks.Length;
    int philosopherNumber = leftForkIndex;

    philosophers[philosopherNumber] = Task.Run(() =>
    {
        Philosopher philosopher = new(philosopherNumber, forks[leftForkIndex], forks[rightForkIndex], thinkingTime, eatingTime);
        philosopher.Eat(cts.Token);
    }, cts.Token);
}

Console.ReadKey();
cts.Cancel();
Console.WriteLine("Cancelling philosophers...");

Task.WaitAll(philosophers);

Console.WriteLine("All philosophers cancelled.");