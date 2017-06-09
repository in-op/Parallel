# Parallel
Custom Parallel and Concurrent
members that compile to .NET 3.5,
C# 4.0. Compatible with Unity 5.
Supports:
* Parallel.For()
* ConcurrentDictionary

### Parallel.For()
A multithreaded for loop. Overloaded:
allows Int64 indexes and thread-local data.

### ConcurrentDictionary<TKey, TValue>
A thread-safe dictionary. All members
are locked instance-wide when invoked.
