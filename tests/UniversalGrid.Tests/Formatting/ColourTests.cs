using NUnit.Framework;
using UniversalGrid.Drawing;

namespace UniversalGrid.Tests.Formatting
{
    [TestFixture]
    public class ColourTests
    {
        [Test]
        public void ToHex_ReturnsExpectedString()
        {
            var black = Colour.Black;

            var hex = black.ToHex();

            Assert.That(hex, Is.EqualTo("#000000"));
        }

        [TestCase(0, 0, 0, 255, false, "#000000")]
        [TestCase(255, 0, 0, 255, false, "#ff0000")]
        [TestCase(255, 255, 0, 255, false, "#ffff00")]
        [TestCase(255, 0, 0, 255, true, "#ff0000ff")]
        public void ToHex(byte r, byte g, byte b, byte a, bool incAlpha, string expected)
        {
            var c = new Colour()
            {
                R = r,
                G = g,
                B = b,
                A = a
            };

            var hex = c.ToHex(incAlpha);

            Assert.That(hex, Is.EqualTo(expected));
        }
    }
}
