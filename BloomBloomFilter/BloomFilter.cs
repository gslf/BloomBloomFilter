using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace TestDownloadResources;

/// <summary>
/// A Bloom filter implementation, a space-efficient probabilistic data structure 
/// that is used to test whether an element is a member of a set. 
/// False positive matches are possible, but false negatives are not. 
/// In other words, a query returns either "possibly in set" or "definitely not in set".
/// </summary>
/// <remarks>
/// This class allows adding elements to the filter and checking for their presence, 
/// with a tunable trade-off between the error rate (chance of false positives) and 
/// space efficiency determined by the capacity and desired error rate parameters 
/// specified at construction.
/// </remarks>
public class BloomFilter {
    private readonly BitArray _bits;
    private readonly int _hashCount;

    /// <summary>
    /// Initializes a new instance of the <see cref="BloomFilter"/> class.
    /// </summary>
    /// <param name="capacity">The expected number of elements the filter 
    /// will hold.</param>
    /// <param name="errorRate">The desired false positive probability rate, 
    /// must be between 0 and 1 (exclusive).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the error 
    /// rate is not between 0 and 1 (exclusive).</exception>
    public BloomFilter(int capacity, double errorRate) {
        var bitArraySize = GetBitArraySize(capacity, errorRate);
        _bits = new BitArray(bitArraySize);
        _hashCount = GetHashCount(capacity, bitArraySize);
    }

    /// <summary>
    /// Adds a new string value to the Bloom filter.
    /// </summary>
    /// <param name="value">The string value to add to the filter.</param>
    public void Add(string value) {
        var hashValues = GetHashValues(value);
        foreach (var hashValue in hashValues) {
            _bits[hashValue] = true;
        }
    }

    /// <summary>
    /// Checks if a string value is possibly in the Bloom filter.
    /// </summary>
    /// <param name="value">The string value to check for presence 
    /// in the filter.</param>
    /// <returns>true if the value might be in the set; false if the 
    /// value is definitely not in the set.</returns>
    public bool Contains(string value) {
        var hashValues = GetHashValues(value);
        foreach (var hashValue in hashValues) {
            if (!_bits[hashValue]) {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Generates hash values for a given string based on the 
    /// filter's hash functions.
    /// </summary>
    /// <param name="value">The string to hash.</param>
    /// <returns>An array of integers representing hash values 
    /// for the input string.</returns>
    private int[] GetHashValues(string value) {
        var hashValues = new int[_hashCount];

        using (MD5 md5Hash = MD5.Create()) {
            var hashBytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(value));

            for (var i = 0; i < _hashCount; i++) {
                int startIndex = (i * 4) % hashBytes.Length;
                hashValues[i] = Math.Abs(BitConverter.ToInt32(hashBytes, startIndex)) % _bits.Length;
            }
        }

        return hashValues;
    }

    /// <summary>
    /// Calculates the size of the bit array needed to achieve a 
    /// desired false positive rate for a given capacity.
    /// </summary>
    /// <param name="capacity">The expected number of elements 
    /// in the Bloom filter.</param>
    /// <param name="errorRate">The desired false positive rate.</param>
    /// <returns>The size of the bit array.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the error 
    /// rate is not between 0 and 1 (exclusive).</exception>
    private static int GetBitArraySize(int capacity, double errorRate) {
        if (errorRate <= 0 || errorRate >= 1) {
            throw new ArgumentOutOfRangeException(nameof(errorRate), "Error rate must be between 0 and 1 (exclusive).");
        }
        var size = -(capacity * Math.Log(errorRate)) / (Math.Log(2) * Math.Log(2));
        return (int)Math.Ceiling(size);
    }

    /// <summary>
    /// Determines the optimal number of hash functions to use based 
    /// on the filter's capacity and bit array size.
    /// </summary>
    /// <param name="capacity">The expected number of elements in the Bloom filter.</param>
    /// <param name="bitArraySize">The size of the bit array.</param>
    /// <returns>The optimal number of hash functions to use.</returns>
    private static int GetHashCount(int capacity, int bitArraySize) {
        var count = (bitArraySize / capacity) * Math.Log(2);
        return Math.Max(1, (int)Math.Round(count));
    }
}
