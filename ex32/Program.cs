using System;

namespace ex32
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Node n_1 = new Node();
            int t;
            using (var sr = new StreamReader("../../../input.txt"))
            {
                int treeHeight = int.Parse(sr.ReadLine()); // Высота дерева
                int childNum = int.Parse(sr.ReadLine()); // число потомков
                t = int.Parse(sr.ReadLine()); // число t
                
                // числа в узлах
                Queue<int> nums = new Queue<int>(); 
                foreach (var substring in sr.ReadToEnd().Split(new char[] { ' '})) nums.Enqueue(int.Parse(substring));
                
                // создаём дерево и заполняем его узлы числами
                FillTheTree(n_1, nums, treeHeight, childNum);
            }
            // находим первый узел со значением t
            Node temp = n_1.FindStartingNode(t);
            Node root = temp is not null ? temp : n_1; // если такого узла нет, то ищем с начала дерева
            
            //ищем максимальное отрицательное число и (если оно есть) записываем в файл
            int result = root.FindMaxNegative();
            using (var sw = new StreamWriter("../../../output.txt"))
            {
                if (result != int.MinValue) sw.WriteLine(result);
                else sw.WriteLine("Ответа — нет");
            }
        }

        public static void FillTheTree(Node root, Queue<int> nums, int height, int childNum, int iteration = 0)
        {
            root.Num = nums.Dequeue();
            iteration++; // "глубина" нынешнего узла
            if (iteration == height) return;

            root.DescendingNodes = new List<Node>();
            for (int i = 0; i < childNum; i++) root.DescendingNodes.Add(new Node());

            foreach (Node node in root.DescendingNodes)
                FillTheTree(node, nums, height, childNum, iteration);
        }
    }

    public class Node
    {
        public int Num;
        public List<Node>? DescendingNodes;

        public Node() { }
        public Node(int num)
        {
            this.Num = num;
        }

        public int FindMaxNegative(int currentMax = int.MinValue)
        {
            int temp = currentMax;

            if (DescendingNodes == null) return (Num < 0 && Num > currentMax) ? Num : currentMax;
            
            foreach (Node node in DescendingNodes)
                temp = node.FindMaxNegative(temp);
            return temp;
        }

        public Node? FindStartingNode(int t)
        {
            if (Num == t) return this;

            Node? result = null;
            if (DescendingNodes != null)
                foreach (Node node in DescendingNodes)
                {
                    result = node.FindStartingNode(t);
                    if (result != null) return result;
                }

            return null;
        }
    }
}