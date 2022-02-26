# Subtask 1: A naive implementation
With long waiting periods (around 1000ms) deadlocks do not occur (at least in a reasonably long observation period).
A deadlock was encounterd with 3 philosophers and waiting periods (Thinkingtime and Eatingtime) of 10 ms.

The shorter the waiting periods, the more likely was a deadlock (all philosophers are waiting for forks).

# Subtask 2: Deadlock prevention
## Why does a deadlock occur?
- **What are the necessary conditions for deadlocks (discussed in the lecture) [0.5 points]?**

    1. Mutual exclusion
    2. Hold and wait
    3. No preemption
    4. Curcular wait

- **Why does the initial solution lead to a deadlock (by looking at the deadlock conditions) [0.5 points]?
Hint: if you cannot provoke a deadlock add sleep's in order to make it more frequent (in the lecture
we also had aribrary sleeps)**

    Deadlocks occur, because all four conditions are present in the naive solution.
    1. the forks are the limited resources which are requested concurrently
    2. the forks are held and other resources/forks are requested in the meantime
    3. no stealing resource locks by other threads
    4. like the philosophers sitting on a round table each waiting for a neighbors fork
    



