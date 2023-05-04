# Augmented Skip List

This project focuses on extending the skip list data structure to support efficient rank-based operations. Two new methods, `Rank(T item)` and `Rank(int i)`, are added to the Skip List class to find the rank of a given item and the item with a given rank, respectively.

## Methods

1. `int Rank(T item)`: Return the rank of the given item.

2. `T Rank(int i)`: Return the item with the given rank i.

## Requirements

1. Describe in a separate document what additional data is needed and how that data is used to support an expected time complexity of O(log n) for each of the Rank methods. Show as well that the methods Add and Remove can efficiently maintain this data as items are added and removed.

2. Re-implement the methods Add and Remove of the Skip List class to maintain the augmented data. Using the Contains method, ensure that added items are distinct.

3. Implement and test the two new Rank methods.

