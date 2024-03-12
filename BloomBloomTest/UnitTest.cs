using System.Text;
using TestDownloadResources;

namespace BloomBloomTest;

[TestClass]
public class UnitTest {
    
    private BloomFilter bf;
    private List<string> stringList;

    [TestInitialize]
    public void TestSetup() {
        // Instance a new BloomFilter Object
        bf = new BloomFilter(100, 0.01);

        // Generate 100 random strings
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        stringList = new List<string>();

        for (int i = 0; i < 100; i++) {
            var stringaCasuale = new StringBuilder(50);
            for (int j = 0; j < 50; j++) {
                stringaCasuale.Append(chars[random.Next(chars.Length)]);
            }

            stringList.Add(stringaCasuale.ToString());
        }

        // Add generated random strings to the bloom filter
        foreach (string str in stringList) {
            bf.Add(str);
        }
    }


    [TestMethod]
    public void TestForAcuracy() {
        int falsePositives = 0;
        for (int i = 0; i < 100; i++) {
            var ressultNotExist = bf.Contains($"Test{i}");
            if (ressultNotExist) {
                falsePositives++;
            }
        }

        // False positive must be less then 10%
        Console.WriteLine($"False positives: {falsePositives}");
        Assert.IsTrue(falsePositives < 10);
    }

    [TestMethod]
    public void TestForFalseNegative() {
        // A bloom filter cannot have false negatives
        foreach (string str in stringList) {
            var resultExist = bf.Contains(str);
            Assert.IsTrue(resultExist);
        }
    }
}