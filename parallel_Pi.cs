using System;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;

public class piEst
{

      static void Main()
      {

        double piApprox = 0; //Value to store the best approximation of pi

        Console.WriteLine("How many points would you like to genearate? (Between 10 Thousand and 10 Million)");
          int pointsCount = reqInt();

          Console.WriteLine("How many approximations do you wish to make? (pick 10 for your first try)");
            int approxCount = reqInt();

        Parallel.For(1, approxCount, (j, loop) => //Run a loop in parallel to make multiple approximations of pi and pick the closest one
        {
          int circCount = 0; //To keep track of the number of points inside of an imaginary circle

          Parallel.For(1, pointsCount, (i, state) => //Creates a bunch of imaginary points
          {
            double x = ( (double)RandomGen3.Next() / (double)2147483647 ) - 0.5; //generates an x and y value with a range of -0.5 to 0.5
            double y = ( (double)RandomGen3.Next() / (double)2147483647 ) - 0.5; //We divide by the maximum value this number could be to get a range of 0 - 1

              double xSqr = Math.Pow(x, 2);
              double ySqr = Math.Pow(y, 2);

            double dist = Math.Pow(xSqr + ySqr, 0.5); //Gets the magnitude distance of the point from the origin

              if(dist <= 0.5) //If the distance is less than the radius of our imaginary circle, then the point is within that circle...
                Interlocked.Increment(ref circCount); //and so we add one to the circle count
          });

        double tempPi = (4 * (double)circCount) / (double)pointsCount; //Formula to generate our approximation

          if(Math.Pow(tempPi - 3.141592653589793, 2) < Math.Pow(piApprox - 3.141592653589793, 2)) //If this guess is better than our current best guess...
            piApprox = tempPi; //Replace it as the new best guess

          Console.WriteLine(j + " : " + tempPi); //Write this approximation to the consle (purely for fun)
        });

        Console.WriteLine(piApprox); //Finally write the best approximation to console
        Console.ReadLine();
      }

      public static int reqInt()
      {
        Retry:

        try
        { return int.Parse( Console.ReadLine() ); } //The number of points generated (we parse to convert the string response to an int)

        catch
        {
          Console.WriteLine("Enter a valid Number");
            goto Retry;
        }

      }

}

  public static class RandomGen3 //This is a very complex random number generator required as the default random does not work in parallel
{
  private static RNGCryptoServiceProvider _global =
      new RNGCryptoServiceProvider();
  [ThreadStatic]
  private static Random _local;

  public static int Next()
  {
      Random inst = _local;
      if (inst == null)
      {
          byte[] buffer = new byte[4];
          _global.GetBytes(buffer);
          _local = inst = new Random(
              BitConverter.ToInt32(buffer, 0));
      }
      return inst.Next();
  } //Note: The diviser in the last step is what I think the maximum value could be, if estimates are off, try and find a closer value for the upper range
}
