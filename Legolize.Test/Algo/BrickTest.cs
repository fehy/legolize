using Legolize.Algo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Legolize.Test.Algo
{
    [TestClass]
    public class BrickTest
    {
        [TestMethod]
        public void InCollision()
        {
            Assert.IsTrue(new Brick(new Point(0, 0, 0), new Point(2, 2, 2)).InCollision(new Brick(new Point(1, 1,1), new Point(3, 3, 3))));
            Assert.IsFalse(new Brick(new Point(0, 0, 0), new Point(2, 2, 2)).InCollision(new Brick(new Point(2, 1, 1), new Point(3, 3, 3))));
            Assert.IsFalse(new Brick(new Point(0, 0, 0), new Point(2, 2, 2)).InCollision(new Brick(new Point(1, 2, 1), new Point(3, 3, 3))));
            Assert.IsFalse(new Brick(new Point(0, 0, 0), new Point(2, 2, 2)).InCollision(new Brick(new Point(1, 1, 2), new Point(3, 3, 3))));
        }
        
    }
}
