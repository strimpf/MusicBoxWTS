using System.Linq;
using System.Threading.Tasks;

using MusicBoxWTS.Core.Services;

using NUnit.Framework;

namespace MusicBoxWTS.Core.Tests.NUnit
{
    // TODO WTS: Add appropriate unit tests.
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        // TODO WTS: Remove or update this once your app is using real data and not the SampleDataService.
        // This test serves only as a demonstration of testing functionality in the Core library.
        [Test]
        public async Task EnsureSampleDataServiceReturnsChartDataAsync()
        {
            var actual = await SampleDataService.GetChartDataAsync();

            Assert.IsNotEmpty(actual);
        }

        // TODO WTS: Remove or update this once your app is using real data and not the SampleDataService.
        // This test serves only as a demonstration of testing functionality in the Core library.
        [Test]
        public async Task EnsureSampleDataServiceReturnsContentGridDataAsync()
        {
            var actual = await SampleDataService.GetContentGridDataAsync();

            Assert.AreNotEqual(0, actual.Count());
        }

        // TODO WTS: Remove or update this once your app is using real data and not the SampleDataService.
        // This test serves only as a demonstration of testing functionality in the Core library.
        [Test]
        public async Task EnsureSampleDataServiceReturnsGridDataAsync()
        {
            var actual = await SampleDataService.GetGridDataAsync();

            Assert.AreNotEqual(0, actual.Count());
        }

        // TODO WTS: Remove or update this once your app is using real data and not the SampleDataService.
        // This test serves only as a demonstration of testing functionality in the Core library.
        [Test]
        public async Task EnsureSampleDataServiceReturnsMasterDetailDataAsync()
        {
            var actual = await SampleDataService.GetMasterDetailDataAsync();

            Assert.AreNotEqual(0, actual.Count());
        }
    }
}
