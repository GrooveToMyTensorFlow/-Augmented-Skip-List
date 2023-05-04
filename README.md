# Augmented Skip List

This project focuses on extending the skip list data structure to support efficient rank-based operations. Two new methods, `Rank(T item)` and `Rank(int i)`, are added to the Skip List class to find the rank of a given item and the item with a given rank, respectively.

## Methods

1. `int Rank(T item)`: Return the rank of the given item.

2. `T Rank(int i)`: Return the item with the given rank i.

## Requirements

1. Describe in a separate document what additional data is needed and how that data is used to support an expected time complexity of O(log n) for each of the Rank methods. Show as well that the methods Add and Remove can efficiently maintain this data as items are added and removed.

2. Re-implement the methods Add and Remove of the Skip List class to maintain the augmented data. Using the Contains method, ensure that added items are distinct.

3. Implement and test the two new Rank methods.

# Additional Data and Time Complexity

To achieve an expected time complexity of O(log n) for the Rank methods in a Skip List, the data structure needs to maintain additional information for each node. Specifically, each node should store the number of its left children (additional data). The left children count is the number of nodes to the left of the current node at the same level.
For the Rank method, the algorithm starts at the top level of the Skip List and iterates through the levels, moving down to the appropriate level at each step. At each level, the algorithm moves to the next node to the right until it finds the node containing the target item or reaches the end of the level.
 During this process, it updates the rank count by adding the left child count of the current node and 1 for the current node if it is not the target item. If the target item is found, it adds the left child count of the node containing the target item to the rank count.
By storing the left child count for each node, the Rank method can efficiently compute the rank of an item in O(log n) time complexity. At each level, the algorithm can determine the number of left children of the current node and add it to the rank count. This operation takes constant time, and the algorithm only needs to visit a logarithmic number of nodes, resulting in a total time complexity of O(log n).

Updating Left Children Count
The left child count for each node can be efficiently maintained during the Add and Remove operations of the Skip List. When a new node is inserted into the Skip List, its left child count can be computed as the sum of the left child counts of its predecessor and its predecessor's left children plus one. The left child count for all nodes to the right of the new node at the same level needs to be incremented by one.
When a node is removed from the Skip List, the left child count for all nodes to the right of the removed node at the same level needs to be decremented by one. The left child count for the parent of the removed node needs to be updated to include the left child count of the removed node.
These operations take constant time, and the additional data structure can be maintained efficiently during Add and Remove operations, resulting in the expected time complexity of O(log n) for the Rank methods.


