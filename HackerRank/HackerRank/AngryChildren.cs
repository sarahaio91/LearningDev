using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HackerRank
{
    class AngryChildren
    {
        public void execute()
        {
            int numOfPackets = Int32.Parse(Console.ReadLine());
            int numOfChildren = Int32.Parse(Console.ReadLine());
            int[] arrayPacket = new int[numOfPackets];
            for (int i = 0; i < numOfPackets; i++)
            {
                arrayPacket[i] = Int32.Parse(Console.ReadLine());
            }
            Array.Sort(arrayPacket);
            int minimum = arrayPacket[numOfChildren-1]-arrayPacket[0];
            for (int i = 0; i < numOfPackets - numOfChildren; i++)
            {
                int temp = arrayPacket[i + numOfChildren-1] - arrayPacket[i];
                if (temp < minimum)
                    minimum = temp;
            }
            Console.WriteLine(minimum);
        }
    }
}
