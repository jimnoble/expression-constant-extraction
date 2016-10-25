using ExpressionInvariantExtraction.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ExpressionInvariantExtraction.Tests
{
    [TestClass]
    public class MutateTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void Mutate_WhenSourceAndOverrides_ThenCorrectValues()
        {
            var original = new FileKey(
                1234,
                DateTime.UtcNow);

            var mutant = original.Mutate(k => k.AccountId == 2345);

            Assert.AreEqual(original.CreateTime, mutant.CreateTime);

            Assert.AreEqual(2345, mutant.AccountId);
        }

        class FileKey
        {
            public FileKey(
                int? accountId = default(int?),
                DateTime? createTime = default(DateTime?))
            {
                AccountId = accountId;

                CreateTime = createTime;
            }

            public int? AccountId { get; }

            public DateTime? CreateTime { get; }
        }
    }
}


