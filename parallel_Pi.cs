using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;


public class piEst
{

      static void Main()
      {
        List<double> guesses = new List<double>(); //Create a list to keep track of the approximations

        double piApprox = 0; //Value to store the best approximation of pi
        double piAvg = 0; //Used to find the average value for pi

        RandomGen2 rand = new RandomGen2(); //Creates an instance of the custom thread safe random class

        Console.WriteLine("How many points would you like to genearate? (Between 10 Thousand and 10 Million)");
          int pointsCount = reqInt();

          Console.WriteLine("How many approximations do you wish to make? (pick 10 for your first try)");
            int approxCount = reqInt();

        Parallel.For(1, approxCount + 1, (j, loop) => //Run a loop in parallel to make multiple approximations of pi and pick the closest one
        {
          int circCount = 0; //To keep track of the number of points inside of an imaginary circle

          Parallel.For(1, pointsCount + 1, (i, state) => //Creates a bunch of imaginary points (Plus one as the upper range is exclusive)
          {

            double x = ( rand.NextDouble() ) - 0.5; //generates an x and y value with a range of -0.5 to 0.5
            double y = ( rand.NextDouble() ) - 0.5;

              x = Math.Pow(x, 2);
              y = Math.Pow(y, 2);

            double dist = Math.Pow(x + y, 0.5); //Gets the magnitude distance of the point from the origin

              if(dist <= 0.5) //If the distance is less than the radius of our imaginary circle, then the point is within that circle...
                Interlocked.Increment(ref circCount); //and so we add one to the circle count
          });

        double tempPi = (4 * (double)circCount) / (double)pointsCount; //Formula to generate our approximation

        guesses.Add(tempPi); //Add the approximation to the list of guesses

        Console.WriteLine(j + " : " + tempPi); //Write this approximation to the consle (purely for fun)

        });

        foreach(double d in guesses)
        {
            if(Math.Pow(d - Math.PI, 2) < Math.Pow(piApprox - Math.PI, 2)) //If this guess is better than our current best guess...
              piApprox = d; //Replace it as the new best guess

            piAvg += d; //Add this value of pi to the rolling average
        }

        Console.WriteLine("Best Guess : " + piApprox); //Print the best approximation to the console
        Console.WriteLine("Average Guess : " + piAvg / approxCount); //Print the average of all the guesses
        Console.ReadLine(); //Prevents the console closing immedietley afterwards
      }

      public static int reqInt()
      {
        Retry:

        try
        { return int.Parse( Console.ReadLine() ); } //The number of points generated (we parse to convert the string response to an int)

        catch { Console.WriteLine("Enter a valid Number"); goto Retry;
        }
      }
}

public class RandomGen2
{
    private static RNGCryptoServiceProvider _global =
      new RNGCryptoServiceProvider();
  [ThreadStatic]
  private static Random _local;

  public double NextDouble()
  {
      Random inst = _local;
      if (inst == null)
      {
          byte[] buffer = new byte[4];
          _global.GetBytes(buffer);
          _local = inst = new Random(
              BitConverter.ToInt32(buffer, 0));
      }
      return (double)inst.Next() / (double)Int32.MaxValue;
  }
}
