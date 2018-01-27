using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Numerics;

namespace ConsoleApplication1
{
    class Helpers
    {
        

        /// <summary>
        /// Constructs a Depth-Reducing set via the greedy algorithm for a two-pass DAG. 
        ///   Initially S = emptyset
        ///   while (G-S contains length d path)
        ///     Find vertex v incident to (approximately) most length d paths in G-S (using CountPathsApx --- approximate version)
        ///     Add v to S 
        ///   Return S
        /// </summary>
        /// <param name="DAG">input DAG G (two pass)</param>
        /// <param name="d">target depth d</param>
        /// <returns>set S such that depth(G-S)<= d_{tgt}. Item 1. Lower bound on size of *any* depth-reducing set S' s.t depth(G-S') <= d. Item 2. Size of S, Item 3. The depth reducing set S. </returns>
        public static Tuple<double, int, bool[]> GreedyDRSetsApxTwoPass(int[] DAG, int d)
        {
            int count = 0;
            double eLowerBound = 0;
            bool[] S = new bool[DAG.Length];

            Tuple<double, double, int> T = CountPathsApx(DAG, d, S);
            double numInitialPaths = T.Item1;
            double logInitPaths = Math.Log(numInitialPaths);
            double currentPaths = T.Item1;// - T.Item2;
            count++;
            S[T.Item3] = true;
            // eLowerBound = Math.Max(eLowerBound, 1.0 / (1.0 - Math.Exp((-logInitPaths + Math.Log(currentPaths)) / (1.0 * count))));
            while (T.Item1 > 0)
            {
                // Console.WriteLine("E Lower Bound: " + eLowerBound + "   (count = " + count + ")");
                T = CountPathsApxTwoPass(DAG, d, S);
                eLowerBound = Math.Max(eLowerBound, 1.0 / (1.0 - (T.Item1 / currentPaths)));

                currentPaths = T.Item1;
                //double currentPaths2 = T.Item1 - T.Item2;
                // if (currentPaths != T.Item1 - T.Item2) Console.WriteLine("Problem...");
                count++;
                S[T.Item3] = true;
                eLowerBound = Math.Max(eLowerBound, 1.0 / (1.0 - Math.Exp((-logInitPaths + Math.Log(currentPaths)) / (1.0 * count))));
            }
            return new Tuple<double, int, bool[]>(eLowerBound, count, S);
        }
        /// <summary>
        /// Constructs a Depth-Reducing set via the greedy algorithm. 
        ///   Initially S = emptyset
        ///   while (G-S contains length d path)
        ///     Find vertex v incident to (approximately) most length d paths in G-S (using CountPathsApx --- approximate version)
        ///     Add v to S 
        ///   Return S
        /// </summary>
        /// <param name="DAG">input DAG G</param>
        /// <param name="d">target depth d</param>
        /// <returns>set S such that depth(G-S)<= d_{tgt}. Item 1. Lower bound on size of *any* depth-reducing set S' s.t depth(G-S') <= d. Item 2. Size of S, Item 3. The depth reducing set S. </returns>
        public static Tuple<double, int, bool[]> GreedyDRSetsApx(int[] DAG, int d)
        {
            int count = 0;
            double eLowerBound = 0;
            bool[] S = new bool[DAG.Length];

            Tuple<double, double, int> T = CountPathsApx(DAG, d, S);
            double numInitialPaths = T.Item1;
            double logInitPaths = Math.Log(numInitialPaths);
            double currentPaths = T.Item1;// - T.Item2;
            count++;
            S[T.Item3] = true;
           // eLowerBound = Math.Max(eLowerBound, 1.0 / (1.0 - Math.Exp((-logInitPaths + Math.Log(currentPaths)) / (1.0 * count))));
            while (T.Item1 > 0)
            {
                // Console.WriteLine("E Lower Bound: " + eLowerBound + "   (count = " + count + ")");
                T = CountPathsApx(DAG, d, S);
                eLowerBound = Math.Max(eLowerBound, 1.0 / (1.0 - (T.Item1/currentPaths ) ));

                currentPaths = T.Item1;
                //double currentPaths2 = T.Item1 - T.Item2;
               // if (currentPaths != T.Item1 - T.Item2) Console.WriteLine("Problem...");
                count++;
                S[T.Item3] = true;
                eLowerBound = Math.Max(eLowerBound, 1.0 / (1.0 - Math.Exp((-logInitPaths + Math.Log(currentPaths)) / (1.0 * count))));
            }
            return new Tuple<double, int, bool[]>(eLowerBound, count, S);
        }
        /// <summary>
        /// Constructs a Depth-Reducing set via the greedy algorithm. 
        ///   Initially S = emptyset
        ///   while (G-S contains length d path)
        ///     Find vertex v incident to most length d paths in G-S (using CountPaths --- exact version)
        ///     Add v to S 
        ///   Return S
        /// </summary>
        /// <param name="DAG">input DAG G</param>
        /// <param name="d">target depth d</param>
        /// <returns>set S such that depth(G-S)<= d_{tgt}. Item 1. Lower bound on size of *any* depth-reducing set S' s.t depth(G-S') <= d. Item 2. Size of S, Item 3. The depth reducing set S. </returns>
        public static Tuple<double, int, bool[]> GreedyDRSets(int[] DAG, int d)
        {
            int count=0;
            double eLowerBound = 0;
            bool[] S = new bool[DAG.Length];
            
            Tuple<BigInteger, BigInteger,int> T = CountPaths(DAG, d, S);
            BigInteger numInitialPaths = T.Item1;
            double logInitPaths = BigInteger.Log(numInitialPaths);
            BigInteger currentPaths = T.Item1 - T.Item2;
            count++;
            S[T.Item3] = true;
            eLowerBound = Math.Max(eLowerBound, 1.0 / (1.0 - Math.Exp((-logInitPaths + BigInteger.Log(currentPaths)) / (1.0 * count))));
            while (T.Item1 > 0)
            {
               // Console.WriteLine("E Lower Bound: " + eLowerBound + "   (count = " + count + ")");
                T = CountPaths(DAG, d, S);
                currentPaths -= T.Item2;
                if (currentPaths != T.Item1 - T.Item2) Console.WriteLine("Problem...");
                count++;
                S[T.Item3] = true;
                eLowerBound = Math.Max(eLowerBound,1.0/(1.0- Math.Exp((-logInitPaths + BigInteger.Log(currentPaths)) / (1.0 * count))));
            }
            return new Tuple<double,int,bool[]>(eLowerBound, count,S);
        }
        /// <summary>
        /// Makes a deep copy of input array
        /// </summary>
        /// <param name="input"> input array</param>
        /// <returns>copied array</returns>
        public static BigInteger[] copyArray (BigInteger[] input)
        {
            BigInteger [] copy = new BigInteger[input.Length];
            for (int i = 0; i < input.Length; i++) copy[i] = input[i];
            return copy;
        }
        /// <summary>
        /// Makes a deep copy of input array
        /// </summary>
        /// <param name="input"> input array</param>
        /// <returns>copied array</returns>
        public static double[] copyArray(double[] input)
        {
            double[] copy = new double[input.Length];
            for (int i = 0; i < input.Length; i++) copy[i] = input[i];
            return copy;
        }
        /// <summary>
        /// Counts the number of length d paths in G-S. This version gives exact value since we use the BigInteger type but is less efficient in terms of running time/space.
        /// </summary>
        /// <param name="DAG">DAG G</param>
        /// <param name="d">path length</param>
        /// <param name="S">set of deleted nodes S</param>
        /// <returns>Tuple. Item 1 = Number of length d paths in G-s, Item 2 = max_v #depth d paths incident to v. Item 3= index of vertex v incident to most length d paths</returns>
        public static Tuple<BigInteger, BigInteger,int> CountPaths(int[] DAG, int d, bool[] S)
        {
            int biggestDistanceInStack = 0;
            Stack<Tuple<BigInteger[], int>> DistDPathCountEndAtV = new Stack<Tuple<BigInteger[], int>>();
            BigInteger[] tailArray = new BigInteger[d + 1];
            BigInteger[] copyEndingAtNodeVOfLengthZero = new BigInteger[DAG.Length];
            BigInteger[] pathsEndingAtNodeVofLengthi = new BigInteger[DAG.Length];
            BigInteger[] pathsEndingAtNodeVofLengthiMinus1 = new BigInteger[DAG.Length];
            BigInteger[] pathsStartingAtNodeVofLengthi = new BigInteger[DAG.Length];
            BigInteger[] pathsStartingAtNodeVofLengthiMinus1 = new BigInteger[DAG.Length];
            BigInteger totalPathsOfLengthd = 0, maxVNumPathsOfLengthdIncidentToV=0;
            BigInteger[] pathsIncidentToV = new BigInteger[DAG.Length];
            
            tailArray[0] = (DAG.Length >= d)? 1:0;
            for (int j = 0; j < DAG.Length; j++)
            {
                copyEndingAtNodeVOfLengthZero[j] = (S[j] == false) ? 1 : 0;
                pathsEndingAtNodeVofLengthiMinus1[j] = (S[j] == false)?1:0;
                pathsStartingAtNodeVofLengthiMinus1[j] =  (S[j] == false) ? 1 : 0;
            }
            DistDPathCountEndAtV.Push(new Tuple<BigInteger[], int>(copyArray(pathsEndingAtNodeVofLengthiMinus1), 0));
            for (int i = 1; i <= d; i++)
            {
                if ((i % 1000) == 0) Console.Write(".");
                pathsEndingAtNodeVofLengthi[0] = 0;
                for (int v = 1; v < DAG.Length; v++)
                {
 
                    if (S[v]) pathsEndingAtNodeVofLengthi[v] = 0;
                    else pathsEndingAtNodeVofLengthi[v] = (S[v - 1] ? 0 : pathsEndingAtNodeVofLengthiMinus1[v - 1])
                            + ((DAG[v] < v - 1 && S[DAG[v]] == false) ? pathsEndingAtNodeVofLengthiMinus1[DAG[v]] : 0);

                }
               //if ((i % 20) == 4) Console.WriteLine("Num Paths of Length" + i + "ending at  node" + (DAG.Length / 2-1) + " is: " + pathsEndingAtNodeVofLengthi[DAG.Length / 2  - 1]);
                if (i >1+ d-(d- biggestDistanceInStack) /2)//???
                {

                    DistDPathCountEndAtV.Push(new Tuple<BigInteger[],int>(copyArray(pathsEndingAtNodeVofLengthi), i));
                    biggestDistanceInStack = i;
                }
                for (int v = 0; v < DAG.Length; v++)
                {
                    pathsEndingAtNodeVofLengthiMinus1[v] = pathsEndingAtNodeVofLengthi[v];
                }
                tailArray[i] = pathsEndingAtNodeVofLengthi[DAG.Length-1];
            }
           
            for (int v = 0; v < DAG.Length; v++)
            {
                pathsIncidentToV[v] += (S[v])?0: pathsEndingAtNodeVofLengthi[v];
                totalPathsOfLengthd += (S[v]) ? 0 : pathsEndingAtNodeVofLengthi[v];
            }
            //Console.WriteLine("\nHalfway There");
            BigInteger totalPathsOfLengthd2 = 0, totalPathsOfLengthd3 = 0;
            for (int i = 1; i <= d; i++)
            {
                if ((i % 1000) == 0) Console.Write(".");
                pathsStartingAtNodeVofLengthi[DAG.Length-1] = 0;
                for (int v = DAG.Length - 1; v >= 0; v--)
                {
                    if (DAG.Length - v  < i || S[v]) pathsStartingAtNodeVofLengthi[v] = 0;
                    else if (DAG.Length - v == i)
                    {
                        pathsStartingAtNodeVofLengthi[v] = 0;//???
                        if (DAG[v] < v-1 && S[DAG[v]] == false) pathsStartingAtNodeVofLengthi[DAG[v]] += pathsStartingAtNodeVofLengthiMinus1[v];

                    }
                    else
                    {
                        pathsStartingAtNodeVofLengthi[v] += S[v+1]?0: pathsStartingAtNodeVofLengthiMinus1[v + 1];
                        if (DAG[v] < v - 1 && S[DAG[v]] == false) pathsStartingAtNodeVofLengthi[DAG[v]] += pathsStartingAtNodeVofLengthiMinus1[v];
                    }



                }
                
                
                while (biggestDistanceInStack > d - i ) { 
                	if (DistDPathCountEndAtV.Count > 1) { DistDPathCountEndAtV.Pop(); biggestDistanceInStack = DistDPathCountEndAtV.Peek().Item2; } else biggestDistanceInStack = 0; 
                }
                Tuple<BigInteger[], int> T = DistDPathCountEndAtV.Peek();
                if (T.Item2 >= d - i) {DistDPathCountEndAtV.Pop(); if (DistDPathCountEndAtV.Count > 0) biggestDistanceInStack = DistDPathCountEndAtV.Peek().Item2; else biggestDistanceInStack = 0; }
                pathsEndingAtNodeVofLengthi = copyArray(T.Item1);
                pathsEndingAtNodeVofLengthiMinus1 = copyArray( T.Item1);
                for (int y=T.Item2; y < d-i; y++)
                {
                    pathsEndingAtNodeVofLengthi[0] = 0;
                    
                    for (int v = 1; v < DAG.Length; v++)
                    {

                        if (S[v]) pathsEndingAtNodeVofLengthi[v] = 0;// = pathsEndingAtNodeVofLengthiMinus1[v - 1];
                        else pathsEndingAtNodeVofLengthi[v] = (S[v - 1] ? 0 : pathsEndingAtNodeVofLengthiMinus1[v - 1])
                                + ((DAG[v] < v - 1 && S[DAG[v]] == false) ? pathsEndingAtNodeVofLengthiMinus1[DAG[v]] : 0);
                    }
                    if (y+1 >= (d-i) - ((d-i) - biggestDistanceInStack) / 2 && y < d-i)
                    {//???
                        DistDPathCountEndAtV.Push(new Tuple<BigInteger[], int>(copyArray(pathsEndingAtNodeVofLengthi), y+1));
                        biggestDistanceInStack = y+1;
                    }
                    for (int v = 0; v < DAG.Length && y+1 < d-i; v++)
                    {
                        pathsEndingAtNodeVofLengthiMinus1[v] = pathsEndingAtNodeVofLengthi[v];
                    }
                }
                for (int v = 0; v < DAG.Length; v++)
                {
                    pathsIncidentToV[v] += pathsEndingAtNodeVofLengthi[v] * pathsStartingAtNodeVofLengthi[v];

                }
              
                 
            //    if (((d-i)%20)==4)    Console.WriteLine("Num Paths of Length" + (d-i) + " ending at  node " + (DAG.Length/2 - 1) + " is: " + pathsEndingAtNodeVofLengthi[DAG.Length / 2 - 1]);

                for (int v = 0; v < DAG.Length; v++)
                {
                    pathsStartingAtNodeVofLengthiMinus1[v] = pathsStartingAtNodeVofLengthi[v];
                    pathsStartingAtNodeVofLengthi[v] = 0;
                    pathsEndingAtNodeVofLengthi[v] = pathsEndingAtNodeVofLengthiMinus1[v];
                }
            }
            int vMax = 0;
            for (int v = 0; v < DAG.Length; v++)
            {
                totalPathsOfLengthd3 += pathsIncidentToV[v] ;
                pathsStartingAtNodeVofLengthi[v]= pathsStartingAtNodeVofLengthiMinus1[v];

                totalPathsOfLengthd2 += pathsStartingAtNodeVofLengthi[v];

                if (maxVNumPathsOfLengthdIncidentToV < pathsIncidentToV[v])
                {
                    maxVNumPathsOfLengthdIncidentToV = pathsIncidentToV[v];
                    vMax = v;
                }
                maxVNumPathsOfLengthdIncidentToV = BigInteger.Max(maxVNumPathsOfLengthdIncidentToV, pathsIncidentToV[v]);
            }
            totalPathsOfLengthd3 = totalPathsOfLengthd3 / (d + 1);

            return new Tuple<BigInteger, BigInteger,int>(totalPathsOfLengthd, maxVNumPathsOfLengthdIncidentToV,vMax);
        }

        /// <summary>
        /// Counts the number of length d paths in G-S. This version only gives an approximation since we use the double type for dynamic programming.
        /// </summary>
        /// <param name="DAG">DAG G</param>
        /// <param name="d">path length</param>
        /// <param name="S">set of deleted nodes S</param>
        /// <returns>Tuple. Item 1 = Number of length d paths in G-s, Item 2 = max_v #depth d paths incident to v. Item 3= index of vertex v incident to most length d paths</returns>
        public static Tuple<double, double, int> CountPathsApx(int[] DAG, int d, bool[] S)
        {
            int biggestDistanceInStack = 0;
            Stack<Tuple<double[], int>> DistDPathCountEndAtV = new Stack<Tuple<double[], int>>();
            double[] tailArray = new double[d + 1];
            double[] copyEndingAtNodeVOfLengthZero = new double[DAG.Length];
            double[] pathsEndingAtNodeVofLengthi = new double[DAG.Length];
            double[] pathsEndingAtNodeVofLengthiMinus1 = new double[DAG.Length];
            double[] pathsStartingAtNodeVofLengthi = new double[DAG.Length];
            double[] pathsStartingAtNodeVofLengthiMinus1 = new double[DAG.Length];
            double totalPathsOfLengthd = 0, maxVNumPathsOfLengthdIncidentToV = 0;
            double[] pathsIncidentToV = new double[DAG.Length];

            // @Lu Array holds the number of len d paths incident to each node
            double[] lenDPathOfEachNode = new double[DAG.Length]

            tailArray[0] = (DAG.Length >= d) ? 1 : 0;
            for (int j = 0; j < DAG.Length; j++)
            {
                copyEndingAtNodeVOfLengthZero[j] = (S[j] == false) ? 1 : 0;
                pathsEndingAtNodeVofLengthiMinus1[j] = (S[j] == false) ? 1 : 0;
                pathsStartingAtNodeVofLengthiMinus1[j] = (S[j] == false) ? 1 : 0;
            }
            DistDPathCountEndAtV.Push(new Tuple<double[], int>(copyArray(pathsEndingAtNodeVofLengthiMinus1), 0));
            for (int i = 1; i <= d; i++)
            {
                if ((i % 1000) == 0) Console.Write(".");
                pathsEndingAtNodeVofLengthi[0] = 0;
                for (int v = 1; v < DAG.Length; v++)
                {

                    if (S[v]) pathsEndingAtNodeVofLengthi[v] = 0;
                    else pathsEndingAtNodeVofLengthi[v] = (S[v - 1] ? 0 : pathsEndingAtNodeVofLengthiMinus1[v - 1])
                            + ((DAG[v] < v - 1 && S[DAG[v]] == false) ? pathsEndingAtNodeVofLengthiMinus1[DAG[v]] : 0);

                }
                //if ((i % 20) == 4) Console.WriteLine("Num Paths of Length" + i + "ending at  node" + (DAG.Length / 2-1) + " is: " + pathsEndingAtNodeVofLengthi[DAG.Length / 2  - 1]);
                if (i > 1 + d - (d - biggestDistanceInStack) / 2)
                {

                    DistDPathCountEndAtV.Push(new Tuple<double[], int>(copyArray(pathsEndingAtNodeVofLengthi), i));
                    biggestDistanceInStack = i;
                }
                for (int v = 0; v < DAG.Length; v++)
                {
                    pathsEndingAtNodeVofLengthiMinus1[v] = pathsEndingAtNodeVofLengthi[v];
                }
                tailArray[i] = pathsEndingAtNodeVofLengthi[DAG.Length - 1];
            }

            for (int v = 0; v < DAG.Length; v++)
            {
                pathsIncidentToV[v] += (S[v]) ? 0 : pathsEndingAtNodeVofLengthi[v];
                totalPathsOfLengthd += (S[v]) ? 0 : pathsEndingAtNodeVofLengthi[v];
            }
            //Console.WriteLine("\nHalfway There");
            double totalPathsOfLengthd2 = 0, totalPathsOfLengthd3 = 0;
            for (int i = 1; i <= d; i++)
            {
                if ((i % 1000) == 0) Console.Write(".");
                pathsStartingAtNodeVofLengthi[DAG.Length - 1] = 0;
                for (int v = DAG.Length - 1; v >= 0; v--)
                {
                    if (DAG.Length - v < i || S[v]) pathsStartingAtNodeVofLengthi[v] = 0;
                    else if (DAG.Length - v == i)
                    {
                        pathsStartingAtNodeVofLengthi[v] = 0;
                        if (DAG[v] < v - 1 && S[DAG[v]] == false) pathsStartingAtNodeVofLengthi[DAG[v]] += pathsStartingAtNodeVofLengthiMinus1[v];

                    }
                    else
                    {
                        pathsStartingAtNodeVofLengthi[v] += S[v + 1] ? 0 : pathsStartingAtNodeVofLengthiMinus1[v + 1];
                        if (DAG[v] < v - 1 && S[DAG[v]] == false) pathsStartingAtNodeVofLengthi[DAG[v]] += pathsStartingAtNodeVofLengthiMinus1[v];
                    }



                }


                while (biggestDistanceInStack > d - i) { if (DistDPathCountEndAtV.Count > 1) { DistDPathCountEndAtV.Pop(); biggestDistanceInStack = DistDPathCountEndAtV.Peek().Item2; } else biggestDistanceInStack = 0; }
                Tuple<double[], int> T = DistDPathCountEndAtV.Peek();
                if (T.Item2 >= d - i) { DistDPathCountEndAtV.Pop(); if (DistDPathCountEndAtV.Count > 0) biggestDistanceInStack = DistDPathCountEndAtV.Peek().Item2; else biggestDistanceInStack = 0; }
                pathsEndingAtNodeVofLengthi = copyArray(T.Item1);
                pathsEndingAtNodeVofLengthiMinus1 = copyArray(T.Item1);
                for (int y = T.Item2; y < d - i; y++)
                {
                    pathsEndingAtNodeVofLengthi[0] = 0;

                    for (int v = 1; v < DAG.Length; v++)
                    {

                        if (S[v]) pathsEndingAtNodeVofLengthi[v] = 0;// = pathsEndingAtNodeVofLengthiMinus1[v - 1];
                        else pathsEndingAtNodeVofLengthi[v] = (S[v - 1] ? 0 : pathsEndingAtNodeVofLengthiMinus1[v - 1])
                                + ((DAG[v] < v - 1 && S[DAG[v]] == false) ? pathsEndingAtNodeVofLengthiMinus1[DAG[v]] : 0);
                    }
                    if (y + 1 >= (d - i) - ((d - i) - biggestDistanceInStack) / 2 && y < d - i)
                    {
                        DistDPathCountEndAtV.Push(new Tuple<double[], int>(copyArray(pathsEndingAtNodeVofLengthi), y + 1));
                        biggestDistanceInStack = y + 1;
                    }
                    for (int v = 0; v < DAG.Length && y + 1 < d - i; v++)
                    {
                        pathsEndingAtNodeVofLengthiMinus1[v] = pathsEndingAtNodeVofLengthi[v];
                    }
                }
                for (int v = 0; v < DAG.Length; v++)
                {
                    pathsIncidentToV[v] += pathsEndingAtNodeVofLengthi[v] * pathsStartingAtNodeVofLengthi[v];

                }


                //    if (((d-i)%20)==4)    Console.WriteLine("Num Paths of Length" + (d-i) + " ending at  node " + (DAG.Length/2 - 1) + " is: " + pathsEndingAtNodeVofLengthi[DAG.Length / 2 - 1]);

                for (int v = 0; v < DAG.Length; v++)
                {
                    pathsStartingAtNodeVofLengthiMinus1[v] = pathsStartingAtNodeVofLengthi[v];
                    pathsStartingAtNodeVofLengthi[v] = 0;
                    pathsEndingAtNodeVofLengthi[v] = pathsEndingAtNodeVofLengthiMinus1[v];
                }
            }
            int vMax = 0;
            for (int v = 0; v < DAG.Length; v++)
            {
                totalPathsOfLengthd3 += pathsIncidentToV[v];
                pathsStartingAtNodeVofLengthi[v] = pathsStartingAtNodeVofLengthiMinus1[v];

                totalPathsOfLengthd2 += pathsStartingAtNodeVofLengthi[v];

                if (maxVNumPathsOfLengthdIncidentToV < pathsIncidentToV[v])
                {
                    maxVNumPathsOfLengthdIncidentToV = pathsIncidentToV[v];
                    vMax = v;
                }
                maxVNumPathsOfLengthdIncidentToV = Math.Max(maxVNumPathsOfLengthdIncidentToV, pathsIncidentToV[v]);
            }
            totalPathsOfLengthd3 = totalPathsOfLengthd3 / (d + 1);

            return new Tuple<double, double, int>(totalPathsOfLengthd, maxVNumPathsOfLengthdIncidentToV, vMax);
        }

        
    }
}
