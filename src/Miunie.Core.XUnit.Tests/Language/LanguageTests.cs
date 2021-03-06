using System;
using System.Collections.Generic;
using System.Linq;
using Miunie.Core.Logging;
using Miunie.Core.Storage;
using Xunit;
using Moq;
using Miunie.Core.Providers;

namespace Miunie.Core.XUnit.Tests.Language
{
    public class LanguageTests
    {
        private const string Key = "TestKey";

        [Fact]
        public void ShouldReturnSimplePhrase()
        {
            const string resourceKey = "GREETING_PHRASE";
            const string expected = "Hello, World!";
            var dataStorageMock = GetMockedStorageFor(
                new Dictionary<string, string>
                {
                    { resourceKey, expected }
                }
                .ToLangResources()
            );

            var langResources = new LanguageProvider(dataStorageMock.Object, new Random(), new Mock<ILogger>().Object);

            var actual = langResources.GetPhrase(resourceKey);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldReturnEmptyStringIfNotFound()
        {
            var expected = string.Empty;
            var dataStorageMock = GetMockedStorageFor(
                new Dictionary<string, string>()
                .ToLangResources()
            );

            var langResources = new LanguageProvider(dataStorageMock.Object, new Random(), new Mock<ILogger>().Object);

            var actual = langResources.GetPhrase("UnknownKey");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldFormatResult()
        {
            const string template = "Hello, {0}!";
            const string parameter = "WORLD";
            const string expected = "Hello, WORLD!";
            var dataStorageMock = GetMockedStorageFor(
                new Dictionary<string, string>
                {
                    { Key, template }
                }
                .ToLangResources()
            );
            var langResources = new LanguageProvider(dataStorageMock.Object, new Random(), new Mock<ILogger>().Object);

            var actual = langResources.GetPhrase(Key, parameter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldFormatMultiple()
        {
            const string template = "{0}, {1}!";
            const string parameter1 = "Hello";
            const string parameter2 = "WORLD";
            const string expected = "Hello, WORLD!";
            var dataStorageMock = GetMockedStorageFor(
                new Dictionary<string, string>
                {
                    { Key, template }
                }
                .ToLangResources()
            );
            var langResources = new LanguageProvider(dataStorageMock.Object, new Random(), new Mock<ILogger>().Object);

            var actual = langResources.GetPhrase(Key, parameter1, parameter2);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldIgnoreExtraParameters()
        {
            const string template = "Hello, {0}!";
            const string parameter = "WORLD";
            const string expected = "Hello, WORLD!";
            var dataStorageMock = GetMockedStorageFor(
                new Dictionary<string, string>
                    {
                        { Key, template }
                    }
                    .ToLangResources()
            );
            var langResources = new LanguageProvider(dataStorageMock.Object, new Random(), new Mock<ILogger>().Object);

            var actual = langResources.GetPhrase(Key, parameter, "Extra", string.Empty, null);

            Assert.Equal(expected, actual);
        }

        private static Mock<IPersistentStorage> GetMockedStorageFor(IEnumerable<LangResource> resources)
        {
            var dataStorageMock = new Mock<IPersistentStorage>();
            dataStorageMock
                .Setup(ds => ds.RestoreSingle<LanguageResourceCollection>(
                    It.IsAny<string>(),
                    It.IsAny<string>())
                )
                .Returns(new LanguageResourceCollection
                {
                    Resources = resources.ToArray()
                });
            return dataStorageMock;
        }
    }
}
