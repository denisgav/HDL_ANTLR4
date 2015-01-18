//
//  Copyright (C) 2010-2014  Denis Gavrish
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace VHDLInputGenerators.Random
{
    internal enum RandomTypes
    {
        Continuous,
        Discrete
    }

    internal enum ContinuousRandomTypes
    {
        Chisquare,
        Erlang,
        Exponential,
        Lognormal,
        Normal,
        SimpleContinuous,
        Student
    }

    internal enum DiscreteRandomTypes
    {
        Bernoulli,
        Binomial,
        Equilikely,
        Geometric,
        Pascal,
        Poisson,
        SimpleDiscrete
    }

    /// <summary>
    /// базовый класс для генератора случайных чисел
    /// </summary>

    public abstract class My_Random_Base : BaseGenerator, IGeneratorDataFill<Int64>, IGeneratorDataFill<Double>, IGeneratorDataFill<bool[]>
    {
        //обьявление констант
        protected const long MODULUS      = 2147483647; /* DON'T CHANGE THIS VALUE                  */
        protected const long MULTIPLIER   = 48271;      /* DON'T CHANGE THIS VALUE                  */
        protected const long CHECK        = 399268537;  /* DON'T CHANGE THIS VALUE                  */
        protected const long STREAMS      = 256;        /* # of streams, DON'T CHANGE THIS VALUE    */
        protected const long A256         = 22925;      /* jump multiplier, DON'T CHANGE THIS VALUE */
        protected const long DEFAULT      = 123456789;  /* initial seed, use 0 < DEFAULT < MODULUS  */
        protected static long[] seed;       /* current state of each stream   */
        protected static long stream      = 0;          /* stream index, 0 is the default */
        protected static long initialized = 0;          /* test for stream initialization */

        static My_Random_Base()
        {
            seed = new long[256];
            seed[0] = DEFAULT;
            //seed = new long [STREAMS] { DEFAULT };
        }
        public uint SizeVector { get; set; }

        /// <summary>
        /// Random returns a pseudo-random real number uniformly distributed 
        ///  between 0.0 and 1.0. 
        /// </summary>
        /// <returns></returns>
        protected double Random()
        {
            const long Q = MODULUS / MULTIPLIER;
            const long R = MODULUS % MULTIPLIER;
            long t;

            t = MULTIPLIER * (seed[stream] % Q) - R * (seed[stream] / Q);
            if (t > 0)
                seed[stream] = t;
            else
                seed[stream] = t + MODULUS;
            return ((double)seed[stream] / MODULUS);
        }


        /// <summary>
        ///  Use this function to set the state of all the random number generator 
        ///  streams by "planting" a sequence of states (seeds), one per stream, 
        ///  with all states dictated by the state of the default stream. 
        ///  The sequence of planted states is separated one from the next by 
        ///  8,367,782 calls to Random().
        /// </summary>
        /// <param name="x"></param>
        protected void PlantSeeds(long x)
        {
            const long Q = MODULUS / A256;
            const long R = MODULUS % A256;
            long j;
            long s;

            initialized = 1;
            s = stream;                            /* remember the current stream */
            SelectStream(0);                       /* change to stream 0          */
            PutSeed(x);                            /* set seed[0]                 */
            stream = s;                            /* reset the current stream    */
            for (j = 1; j < STREAMS; j++)
            {
                x = A256 * (seed[j - 1] % Q) - R * (seed[j - 1] / Q);
                if (x > 0)
                    seed[j] = x;
                else
                    seed[j] = x + MODULUS;
            }
        }

        /// <summary>
        /// Use this function to set the state of the current random number 
        /// generator stream according to the following conventions:
        ///    if x > 0 then x is the state (unless too large)
        ///    if x < 0 then the state is obtained from the system clock
        ///    if x = 0 then the state is to be supplied interactively
        /// </summary>
        /// <param name="x"></param>
        protected void PutSeed(long x)
        {
            bool ok = false;

            if (x > 0)
                x = x % MODULUS;                       /* correct if x is too large  */
            if (x < 0)
            {
                DateTime time = DateTime.Now;
                x = (time.Ticks) % MODULUS;
            }
            if (x == 0)                                
                while (!ok)
                {
                    Console.WriteLine("\nEnter a positive integer seed (9 digits or less) >> ");
                    x = long.Parse(Console.ReadLine());
                    ok = (0 < x) && (x < MODULUS);
                    if (!ok)
                        Console.WriteLine("\nInput out of range ... try again\n");
            }
            seed[stream] = x;
        }

        /// <summary>
        /// Use this function to get the state of the current random number 
        /// generator stream.                                                   
        /// </summary>
        /// <returns></returns>
        protected long GetSeed()
        {
            return seed[stream];
        }

        /// <summary>
        /// Use this function to set the current random number generator
        /// stream -- that stream from which the next random number will come.
        /// </summary>
        /// <param name="index"></param>
        void SelectStream(int index)
        {
            stream = ((uint) index) % STREAMS;
            if ((initialized == 0) && (stream != 0))   /* protect against        */
                PlantSeeds(DEFAULT);                     /* un-initialized streams */
        }

        /// <summary>
        /// Use this (optional) function to test for a correct implementation.
        /// </summary>
        /// <returns></returns>
        public string TestRandom()
        {
            long   i;
            long   x;
            double u;
            bool   ok = false;  

            SelectStream(0);                  /* select the default stream */
            PutSeed(1);                       /* and set the state to 1    */
            for(i = 0; i < 10000; i++)
                u = Random();
            x = GetSeed();                    /* get the new state value   */
            ok = (x == CHECK);                /* and check for correctness */

            SelectStream(1);                  /* select stream 1                 */ 
            PlantSeeds(1);                    /* set the state of all streams    */
            x = GetSeed();                    /* get the state of stream 1       */
            ok = ok && (x == A256);           /* x should be the jump multiplier */    
            if (ok)
                return "\n The implementation is correct.\n\n";
            else
                return "\n ERROR -- the implementation is not correct.\n\n";
        }

        public abstract IEnumerable<long> GetIntegerEnumerator();
        public abstract IEnumerable<double> GetDoubleEnumerator();
        public abstract IEnumerable<bool[]> GetBitArrayEnumerator();

        SortedList<ulong, long> IGeneratorDataFill<Int64>.InsertValues(ulong StartTime, ulong EndTime)
        {
            SortedList<UInt64, Int64> data = new SortedList<ulong, Int64>();
            UInt64 time = StartTime;

            foreach (var b in GetIntegerEnumerator())
            {
                data.Add(time, b);
                time += timeStep.GetTimeUnitInFS();

                if (time > EndTime)
                    break;
            }
            return data;
        }

        SortedList<ulong, double> IGeneratorDataFill<double>.InsertValues(ulong StartTime, ulong EndTime)
        {
            SortedList<UInt64, Double> data = new SortedList<ulong, Double>();
            UInt64 time = StartTime;

            foreach (var b in GetDoubleEnumerator())
            {
                data.Add(time, b);
                time += timeStep.GetTimeUnitInFS();

                if (time > EndTime)
                    break;
            }
            return data;
        }

        SortedList<ulong, bool[]> IGeneratorDataFill<bool[]>.InsertValues(ulong StartTime, ulong EndTime)
        {
            SortedList<UInt64, bool[]> data = new SortedList<ulong, bool[]>();
            UInt64 time = StartTime;

            foreach (var b in GetBitArrayEnumerator())
            {
                data.Add(time, b);
                time += timeStep.GetTimeUnitInFS();

                if (time > EndTime)
                    break;
            }

            return data;
        }
    }
}
