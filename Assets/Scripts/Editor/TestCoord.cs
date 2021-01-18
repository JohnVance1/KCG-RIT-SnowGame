using NUnit.Framework;

public class HexCoordinatesTest
{
    [Test]
    public void TestCoordPlus_00()
    {
        HexCoordinates c1 = new HexCoordinates(HexDirection.N, 1, 2);
        HexCoordinates c2 = c1 + HexDirection.S;
        HexCoordinates c3 = c1 + HexDirection.N;
        HexCoordinates c4 = c1 + HexDirection.NE;
        HexCoordinates c5 = c1 + HexDirection.NW;
        HexCoordinates c6 = c1 + HexDirection.SE;
        HexCoordinates c7 = c1 + HexDirection.SW;

        Assert.AreEqual(c2, new HexCoordinates(HexDirection.NE, 0, 1));
        Assert.AreEqual(c3, new HexCoordinates(HexDirection.N, 1, 3));
        Assert.AreEqual(c4, new HexCoordinates(HexDirection.N, 2, 3));
        Assert.AreEqual(c5, new HexCoordinates(HexDirection.N, 0, 2));
        Assert.AreEqual(c6, new HexCoordinates(HexDirection.NE, 0, 2));
        Assert.AreEqual(c7, new HexCoordinates(HexDirection.N, 0, 1));
    }

    [Test]
    public void TestCoordPlus_01()
    {
        HexCoordinates c1 = new HexCoordinates(HexDirection.S, 0, 1);
        HexCoordinates c2 = c1 + HexDirection.S;
        HexCoordinates c3 = c1 + HexDirection.N;
        HexCoordinates c4 = c1 + HexDirection.NE;
        HexCoordinates c5 = c1 + HexDirection.NW;
        HexCoordinates c6 = c1 + HexDirection.SE;
        HexCoordinates c7 = c1 + HexDirection.SW;

        Assert.AreEqual(c2, new HexCoordinates(HexDirection.S, 0, 2));
        Assert.AreEqual(c3, HexCoordinates.Origin);
        Assert.AreEqual(c4, new HexCoordinates(HexDirection.SE, 0, 1));
        Assert.AreEqual(c5, new HexCoordinates(HexDirection.SW, 0, 1));
        Assert.AreEqual(c6, new HexCoordinates(HexDirection.SE, 1, 2));
        Assert.AreEqual(c7, new HexCoordinates(HexDirection.S, 1, 2));
    }

    [Test]
    public void TestCoordPlus_02()
    {
        HexCoordinates c1 = new HexCoordinates(HexDirection.SW, 0, 2);
        HexCoordinates c6 = c1 + HexDirection.SE;

        Assert.AreEqual(c6, new HexCoordinates(HexDirection.S, 1, 2));
    }
}
