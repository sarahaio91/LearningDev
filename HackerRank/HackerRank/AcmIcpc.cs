//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Numerics;

//namespace HackerRank
//{
//    class AcmIcpc
//    {
//        public void execute()
//        {
//            string firstLine = Console.ReadLine();
//            int numOfTeam = Int32.Parse(firstLine.Split(' ')[0]);
//            int numOfProb = Int32.Parse(firstLine.Split(' ')[0]);
//            double da = (double) numOfProb / 63;
//            long[,] teamProb = new long[numOfTeam,(int) Math.Ceiling(da)];
//            int match = 0;
//            int largestOne = 0;
//            for (int i = 0; i < numOfTeam; i++)
//            {
//                bool flag = true;
//                string[] subInput = divideInput(Console.ReadLine());
                
//                    long result;
//                    teamProb[i] = Convert.ToInt64(sub, 2);
//                    for (int j = i - 1; j >= 0; j--)
//                    {
//                        foreach (string sub in subInput)
//                        { 
                        
//                        }
//                        result = teamProb[i] | teamProb[j];
//                        string resultString = Convert.ToString(result, 2);
//                        Console.WriteLine("Result of {0} {1} {2}", i, j, resultString);
//                        if (resultString.Contains('0'))
//                        {
//                            flag = false;
//                        }
//                        tempOne[j] = tempOne[j] + resultString.Count(x => x == '1');
//                        tempOneFinal[j] = tempOneFinal[j] + tempOne[j];
//                    }
                
//                if (tempOne > largestOne)
//                    largestOne = tempOne;
//                if (flag)
//                {
//                    match++;
//                }
//            }
//            Console.WriteLine(largestOne);
//            Console.WriteLine(match);
//        }
//        public string[] divideInput(string input)
//        {
//            double da = (double) input.Length / 63;
//            string[] output = new string[(int)Math.Ceiling(da)];
//            for (int i = 0; i < input.Length; i = i + 63)
//            {
//                if (input.Length - i < 63)
//                    output[i / 63] = input.Substring(i);
//                else
//                    output[i / 63] = input.Substring(i, 63);
//            }
//            return output;
//        }
//    }
//}
