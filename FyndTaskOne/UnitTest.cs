using HtmlAgilityPack;
using NUnit.Framework;
using System;

namespace FyndTaskOne
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetFileData_ShouldNotReturnException()
        {
            Program p = new Program();
            Assert.DoesNotThrow(() => p.GetFileData());
        }

        [Test]
        public void GetFileData_ShouldNotBeNullOrEmpty()
        {
            Program p = new Program();
            Assert.IsTrue(!string.IsNullOrEmpty(p.GetFileData()));
        }

        [Test]
        public void GetFilePath_ShouldNotBeNullOrEmpty()
        {
            Program p = new Program();
            Assert.IsTrue(!string.IsNullOrEmpty(p.GetFilePath()));
        }

        [Test]
        public void CheckFilePath_ShouldReturnException()
        {
            Program p = new Program();
            var ex = Assert.Throws<Exception>(() => p.CheckFilePath("..//"));
            Assert.That(ex.Message, Is.EqualTo("File does not exist!"));
        }

        [Test]
        public void CheckFilePath_ShouldNotReturnException()
        {
            Program p = new Program();
            Assert.DoesNotThrow(() => p.CheckFilePath(p.GetFilePath()));
        }

        [Test]
        public void GetFirstNodeText_ShouldReturnException()
        {
            Program p = new Program();
            var ex = Assert.Throws<Exception>(() => p.GetFirstNodeText(null, null));
            Assert.That(ex.Message, Is.EqualTo("Field not found!Object reference not set to an instance of an object."));
        }

        [TestCase("<p id='hotel_name'>Mehmet</p>", "//*[@id='hotel_name']", "Mehmet")]
        [TestCase("<p class='summary'>Lorem..</p>", "//*[@class='summary']", "Lorem..")]
        public void GetFirstNodeText_ShouldEqualExpectedString(string input, string path, string expected)
        {
            Program p = new Program();
            HtmlDocument htmldoc = new HtmlDocument();
            htmldoc.LoadHtml(input);
            Assert.That(p.GetFirstNodeText(htmldoc, path), Is.EqualTo(expected));
        }

        [Test]
        public void ExtractData_ShouldNotReturnException()
        {
            Program p = new Program();
            Assert.DoesNotThrow(() => p.ExtractData());
        }

        [Test]
        public void ExtractData_ShouldNotBeNullOrEmpty()
        {
            Program p = new Program();
            Assert.IsTrue(!string.IsNullOrEmpty(p.ExtractData()));
        }

    }
}