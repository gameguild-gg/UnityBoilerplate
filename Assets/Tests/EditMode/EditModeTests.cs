using NUnit.Framework;

namespace Tests.EditMode
{
    /// <summary>
    /// Edit Mode tests run inside the Unity Editor. They are fast and are the
    /// right place for plain C# logic, math, data classes and editor tooling.
    /// Add your own tests here — every test in this folder runs in CI before
    /// the project is allowed to build and deploy.
    /// </summary>
    public class EditModeTests
    {
        [Test]
        public void Arithmetic_WorksInEditMode()
        {
            Assert.AreEqual(4, 2 + 2);
        }

        [Test]
        public void UnityEngine_TypesAreAvailable()
        {
            // Even in Edit Mode, engine types like Vector3 are available.
            var v = new UnityEngine.Vector3(1f, 2f, 3f);
            Assert.AreEqual(2f, v.y);
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(2, 4)]
        [TestCase(-3, 9)]
        public void Squares_AreComputedCorrectly(int input, int expected)
        {
            Assert.AreEqual(expected, input * input);
        }
    }
}
