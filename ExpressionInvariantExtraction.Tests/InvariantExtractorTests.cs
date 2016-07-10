using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExpressionInvariantExtraction.Implementation;
using System.Linq.Expressions;
using System.Diagnostics;

namespace ExpressionInvariantExtraction.Tests
{
    [TestClass]
    public class InvariantExtractorTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void ExtractInvariants_WhenAllInvariants_ThenAllValues()
        {
            var accountId = 123;

            var createTime = new DateTime(
                2016, 7, 10, 
                1, 2, 3, 
                DateTimeKind.Utc);

            Expression<Func<FileKey, bool>> redacted;

            var invariant = new InvariantExtractor().ExtractInvariants(
                c => c.AccountId == accountId && c.CreateTime == createTime, 
                out redacted);

            Assert.AreEqual(accountId, invariant.AccountId);

            Assert.AreEqual(createTime, invariant.CreateTime);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ExtractInvariants_WhenSomeInvariant_ThenSomeInvariantValue()
        {
            var accountId = 123;

            var createTime = new DateTime(
                2016, 7, 10,
                1, 2, 3,
                DateTimeKind.Utc);

            Expression<Func<FileKey, bool>> redacted;

            var invariant = new InvariantExtractor().ExtractInvariants(
                c => c.AccountId == accountId && c.CreateTime < createTime,
                out redacted);

            Assert.AreEqual(accountId, invariant.AccountId);

            Assert.IsNull(invariant.CreateTime);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ExtractInvariants_WhenNoInvariants_ThenNoValues()
        {
            var accountId = new Random().Next();//123;

            var createTime = new DateTime(
                2016, 7, 10,
                1, 2, 3,
                DateTimeKind.Utc);

            Expression<Func<FileKey, bool>> redacted;

            var invariant = new InvariantExtractor().ExtractInvariants(
                c => c.AccountId != accountId && c.CreateTime < createTime,
                out redacted);

            Assert.IsNull(invariant.AccountId);

            Assert.IsNull(invariant.CreateTime);
        }

        [TestMethod]
        [TestCategory("Speed")]
        public void ExtractInvariants_SpeedTest()
        {
            var accountId = 123;

            var createTime = new DateTime(
                2016, 7, 10,
                1, 2, 3,
                DateTimeKind.Utc);

            Expression<Func<FileKey, bool>> redacted;

            var time = Stopwatch.StartNew();

            var count = 0;

            while (time.Elapsed < TimeSpan.FromSeconds(5))
            {
                var invariant = new InvariantExtractor().ExtractInvariants(
                    c => c.AccountId == accountId && c.CreateTime == createTime,
                    out redacted);

                Assert.AreEqual(accountId, invariant.AccountId);

                Assert.AreEqual(createTime, invariant.CreateTime);

                count++;
            }

            var rate = count / time.Elapsed.TotalSeconds;

            var avgMs = time.Elapsed.TotalMilliseconds / count;

            Console.WriteLine($"Rate: {rate:0.00} / sec, Avg: {avgMs:0.00} ms");
        }

        class FileKey
        {
            public int? AccountId { get; set; }

            public DateTime? CreateTime { get; set; }
        }
    }
}


