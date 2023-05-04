using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Based on http://igoro.com/archive/skip-lists-are-fascinating/

namespace SkipLists
{
    // Interface for SkipList

    interface ISkipList<T> where T : IComparable
    {
        void Insert(T item);     // Inserts item into the skip list (duplicates are permitted)
        bool Contains(T item);   // Returns true if item is found; false otherwise
        void Remove(T item);     // Removes one occurrence of item (if possible) from the skip list
        int Rank(T item);
        T RankC(int i);
    }

    // Class SkipList

    class SkipList<T> : ISkipList<T> where T : IComparable
    {
        private Node head;          // Header node of height 32
        private int maxHeight;     // Maximum height among non-header nodes
        private Random rand;          // For generating random heights


        // Class Node (used by SkipList)

        private class Node
        {
            public T Item { get; set; }
            public int Height { get; set; }
            public Node[] Next { get; set; }
            public int leftChildren { get; set; }
            public Node[] prev { get; set; }         // reference to the prev node

            // Constructor
            public Node(T item, int height)
            {
                Item = item;
                Height = height;
                Next = new Node[Height];
                prev = new Node[Height];
                leftChildren = 0;
                //numNodes[Height,0] = 0 ;
            }
        }

        // Constructor
        // Creates an empty skip list with a header node of height 32
        // Time complexity: O(1)

        public SkipList()
        {
            head = new Node(default(T), 32);    // Set to NIL by default
            maxHeight = 0;                      // Current maximum height of the skip list
            rand = new Random();
        }

        // Insert
        // Inserts the given item into the skip list
        // Duplicate items are permitted
        // Expected time complexity: O(log n)

        public void Insert(T item)
        {
            // Randomly determine height of a node

            if (Contains(item) == true)
            {
                throw new ArgumentException("The item already exisits");
            }
            else
            {
                int height = 0;
                int R = rand.Next();   // R is a random 32-bit positive integer
                while ((height < maxHeight) && ((R & 1) == 1))
                {
                    height++;   // Equal to the number consecutive lower order 1-bits
                    R >>= 1;    // Right shift one bit
                }

                if (height == maxHeight)
                {
                    maxHeight++;
                }

                // Create and insert node



                Node newNode = new Node(item, height + 1);
                Node cur = head;

                for (int i = maxHeight - 1; i >= 0; i--)  // for each level
                {
                    while (cur.Next[i] != null && cur.Next[i].Item.CompareTo(item) < 0)
                        cur = cur.Next[i];

                    // Adjust references at level i once the height is within range
                    if (i < newNode.Height)
                    {
                        //head.numNodes[i, 0] += 1;   // incrementing the size at each level.
                        // only done in the header node.
                        // next pointers
                        newNode.Next[i] = cur.Next[i];
                        cur.Next[i] = newNode;

                        // prev pointers
                        newNode.prev[i] = cur;

                        if (newNode.Next[i] != null)
                        {
                            newNode.Next[i].prev[i] = newNode;

                        }

                    }
                }
            }


        }

        // Contains
        // Returns true if the given item is found in the skip list; false otherwise
        // Expected time complexity: O(log n)

        public bool Contains(T item)
        {
            Node cur = head;
            for (int i = maxHeight - 1; i >= 0; i--)  // for each level
            {
                while (cur.Next[i] != null && cur.Next[i].Item.CompareTo(item) < 0)
                    cur = cur.Next[i];

                if (cur.Next[i] != null && cur.Next[i].Item.CompareTo(item) == 0)
                    return true;
            }
            return false;
        }

        // Remove
        // Removes one occurrence (if possible) of the given item from the skip list
        // Expected time complexity: O(log n)

        public void Remove(T item)
        {
            Node cur = head;
            for (int i = maxHeight - 1; i >= 0; i--)  // for each level
            {
                while (cur.Next[i] != null && cur.Next[i].Item.CompareTo(item) < 0)
                    cur = cur.Next[i];

                if (cur.Next[i] != null && cur.Next[i].Item.CompareTo(item) == 0)
                {

                    //head.numNodes[i, 0] -= 1;

                    cur.Next[i].Height--;                 // Decrease height by 1 when a level is removed
                    cur.Next[i] = cur.Next[i].Next[i];    // Remove reference at level i

                    cur.Next[i].prev[i] = cur;

                }

                // Decrease maximum height by 1 when the number of nodes at height i is 0
                if (head.Next[i] == null)
                {
                    maxHeight--;
                }
            }
        }

        // Print
        // Outputs the item in sorted order
        // Time complexity:  O(n)

        public void Print()
        {
            Node cur = head.Next[0];
            while (cur != null)
            {
                Console.Write(cur.Item + " ");
                cur = cur.Next[0];
            }
            Console.WriteLine();
        }

        // Profile
        // Prints out a "skyline" of the skip list
        // Time complexity: O(n)

        public void Profile()
        {
            Node cur = head;

            while (cur != null)
            {
                Console.WriteLine(new string('*', cur.Height));  // Outputs a string of *s  //also includes Head NODE
                cur = cur.Next[0];
            }
        }

        // Rank
        // Finding te rank if the item
        public int Rank(T item)
        {
            int Trank = 0;
            Node curr = head;

            for (int i = maxHeight - 1; i >= 0; i--)  // for each level
            {
                while (curr.Next[i] != null && curr.Next[i].Item.CompareTo(item) < 0)
                {
                    curr = curr.Next[i];
                    Trank = Trank + curr.leftChildren + 1;
                }
                if (curr.Next[i] != null && curr.Next[i].Item.CompareTo(item) == 0)
                {
                    Trank = Trank + curr.Next[i].leftChildren + 1;
                    curr = curr.Next[i];
                }
            }

            if (curr.Item.CompareTo(item) != 0)
            {
                Console.WriteLine("Rank not found");
                return -1; // or return any other value that indicates rank not found
            }

            return Trank;
        }


        // Returns the item with the given rank in the skip list
        public T RankC(int i)
        {
            Node current = head;

            // Iterate through the levels of the skip list
            for (int j = maxHeight - 1; j >= 0; j--)
            {
                // Search for the node with the given rank
                while (current.Next[j] != null && current.leftChildren + current.Next[j].leftChildren + 1 <= i)
                {
                    current = current.Next[j];
                }

                if (current.leftChildren + 1 == i)
                {
                    return current.Item;
                }
                else if (current.Next[j] == null || current.leftChildren + current.Next[j].leftChildren + 1 > i)
                {
                    continue;
                }
                else
                {
                    i -= current.leftChildren + 1;
                    current = current.Next[j];
                }
            }

            Console.WriteLine("Item not found");
            return default(T); // Item not found in the skip list
        }


        public int leftChild(T item)
        {
            Node cur = head;
            int numLeftChild = 0;

            bool found = false;
            int i = maxHeight - 1;

            while (i > 0 || found == false)  // for each level
            {
                while (cur.Next[i] != null && cur.Next[i].Item.CompareTo(item) < 0)
                {
                    cur = cur.Next[i];
                }
                if (cur.Next[i] != null && cur.Next[i].Item.CompareTo(item) == 0)
                {
                    cur = cur.Next[i];
                    found = true;
                }

                i--;
            }

            Node curItem = cur;

            while (curItem.prev[0] != null && curItem.prev[0].Height < cur.Height && !curItem.prev.Equals(head))
            {

                numLeftChild++;
                curItem = curItem.prev[0];
            }
            cur.leftChildren = numLeftChild;
            return numLeftChild;

        }

    }

    class Program
    {
        static void Main(string[] args)
        {

            SkipList<int> S = new SkipList<int>();

            int i;
            // insert items.
            for (i = 10; i <= 100; i+=10)
            {
                S.Insert(i);
            }

            S.Print();
            S.Profile();

            // calculate the left children of the nodes.
            for (i = 10; i <= 100; i += 10)
            {
                S.leftChild(i);
            }

            Console.WriteLine("The rank for 50 is > " + S.Rank(50));
            Console.WriteLine("The rank for 45 is > " + S.Rank(45));


            //Console.WriteLine("The item with rank 8 is > " + S.RankC(8));
            //Console.WriteLine("The item with rank 11 is > " + S.RankC(11));
            

            Console.ReadKey();

            Console.ReadKey();
        }
    }
}
