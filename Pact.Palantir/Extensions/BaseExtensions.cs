﻿namespace Pact.Palantir.Extensions
{
  using System;
  using System.Collections.Generic;

  using Tangle.Net.Entity;

  /// <summary>
  /// The extensions.
  /// </summary>
  public static class BaseExtensions
  {
    /// <summary>
    /// The encode bytes as string.
    /// </summary>
    /// <param name="byteArray">
    /// The byte array.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public static string EncodeBytesAsString(this IEnumerable<byte> byteArray)
    {
      string[] trytesArray =
        {
          "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
        };

      var trytes = string.Empty;

      foreach (var value in byteArray)
      {
        // If outside bounderies of a byte, return null
        if (value > 255)
        {
          return null;
        }

        var firstValue = value % 27;
        var secondValue = (value - firstValue) / 27;

        var trytesValue = trytesArray[firstValue] + trytesArray[secondValue];

        trytes += trytesValue;
      }

      return trytes;
    }

    public static byte[] DecodeBytesFromTryteString(this TryteString tryteString)
    {
      var trytesArray = new List<string> { "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

      // If input length is odd, return null
      if (tryteString.Value.Length % 2 != 0)
      {
        return null;
      }

      var byteList = new List<byte>();

      for (var i = 0; i < tryteString.Value.Length; i += 2)
      {
        var firstValue = trytesArray.IndexOf(tryteString.Value.Substring(i, 1));
        var secondValue = trytesArray.IndexOf(tryteString.Value.Substring(i + 1, 1));

        var value = firstValue + (secondValue * 27);
        byteList.Add(Convert.ToByte(value));
      }

      return byteList.ToArray();
    }

    public static string Truncate(this string value, int maxLength)
    {
      if (!string.IsNullOrEmpty(value) && value.Length > maxLength)
      {
        return value.Substring(0, maxLength);
      }

      return value;
    }
  }
}