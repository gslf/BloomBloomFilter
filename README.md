# BloomBloomFilter
A C# implementation of Bloom Filters. Available on (NuGet)[https://www.nuget.org/packages/BloomBloomFilter]

A Bloom filter is a data structure used to test if an element is a member of a set or not. It was invented by Burton Howard Bloom in 1970. The Bloom filter uses multiple hash functions to map the input data to a bit array. Let’s see together how it works. We’ll start with a bit of theory that helps to understand how it works, and then we’ll look at a direct example in C#.

Bloom Filters: The Theory
-------------------------

A Bloom filter is based on the idea of hashing, which is a process of mapping data of arbitrary size to data of a fixed size. The Bloom filter uses multiple hash functions to map the input data to a bit array. The bit array is then used to represent the set. The Bloom filter is a space-efficient data structure because it does not store the actual elements of the set. Instead, it stores the hash values of the elements in the set. This makes it possible to represent very large sets with a small amount of memory.

One of the key advantages of Bloom filters is that they are very fast. Both the add and contains operations have a time complexity of O(1). However, there is a trade-off between speed and accuracy. **Bloom filters have a non-zero probability of returning a false positive**, which means that it may indicate that an element is in the set when it is not. The probability of a false positive can be controlled by adjusting the size of the bit array and the number of hash functions used. In many cases, however, we just need to quickly verify that an element **is not** in a set. This is where the Bloom filter can significantly improve the performance of our software. Check out this visual example of using a Bloom Filter to reduce disk accesses.

![Bloom filters visual explaination](https://gslf.it/res/img/articles/bloom-filters-explaination-1.jpg)

Bloom Filters: How it work
--------------------------

Bloom filter algorithms use a bit array and multiple hash functions. When an item is added to the bloom filter, it undergoes a series of hash operations, and the bits at the calculated positions are set to 1 (refer to the image below). To check if an item is present, the same hash calculations are performed, and if the bits at the calculated positions are all set to 1, the item is potentially present. However, if even one of those bits is 0, the item is definitely not present. Since false positives can occur, the algorithm is considered probabilistic, but it compensates by offering extremely high performance.

![Bloom filters: how it works visual explaination](https://gslf.it/res/img/articles/bloom-filters-explaination-2.jpg)

---

Made in southern Italy with love ❤️ - © 2023 - Gioele Stefano Luca Fierro
