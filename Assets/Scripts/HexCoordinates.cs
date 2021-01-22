/**
 * @author  Lingxiao Yu
 * @github  http://github.com/KHN190
 */

using System;
using UnityEngine;

[Serializable]
public struct HexCoordinates : IEquatable<HexCoordinates> {
    [SerializeField]
    private HexDirection direction;
    [SerializeField]
    private int outerIndex, step;
    public HexDirection X {
        get {
            return direction;
        }
    }

    public int Y {
        get {
            return outerIndex;
        }
    }

    public int Z {
        get {
            return step;
        }
    }

    public static HexCoordinates Origin {
        get {
            return new HexCoordinates(HexDirection.N, 0, 0);
        }
    }

    public HexCoordinates(HexDirection direction, int outerIndex, int step) {
        if (step > 0 && outerIndex >= step)
            throw new IndexOutOfRangeException("Outer index must be less than step size.");
        if (outerIndex < 0 || step < 0)
            throw new IndexOutOfRangeException("Outer index and step must be no less than 0.");

        this.direction = direction;
        this.outerIndex = outerIndex;
        this.step = step;
    }

    public override string ToString() {
        return "(" + X + ", " + Y + ", " + Z + ")";
    }

    // step the coordinate on given direction
    public HexCoordinates Step(HexDirection dir) {
        return this + dir;
    }

    // calculate local position (world transform pos not included)
    public Vector3 ToPosition() {
        if (this == Origin)
            return Vector3.zero;

        HexDirection dir = direction;
        HexDirection outerDir = dir.Next().Next();

        Vector3 vbase = HexMetrics.corners[(int)direction] * step;
        Vector3 offset = HexMetrics.corners[(int)outerDir] * outerIndex;
        return vbase + offset;
    }

    // calculate from local position
    public static HexCoordinates FromPosition(Vector3 pos) {
        throw new System.NotImplementedException();
    }

    public bool Equals(HexCoordinates other) {
        // origin doesn't have direction
        if (step == 0 || other.step == 0)
            return Y == other.Y && Z == other.Z;
        // otherwise, compare direction
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    public override bool Equals(object obj) {
        return base.Equals(obj);
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }

    public static bool operator ==(HexCoordinates c1, HexCoordinates c2) {
        return c1.Equals(c2);
    }

    public static bool operator !=(HexCoordinates c1, HexCoordinates c2) {
        return !c1.Equals(c2);
    }

    // todo this may have bugs, if you found any, report to me.
    public static HexCoordinates operator +(HexCoordinates c1, HexDirection dir) {
        if (c1 == Origin)
            return new HexCoordinates(dir, 0, 1);

        // direction 1
        if (dir == c1.X)
            return new HexCoordinates(c1.X, c1.Y, c1.Z + 1);
        // direction 2
        if (dir == c1.X.Opposite()) {
            if (c1.Z - 1 == 0) return Origin;
            if (c1.Y == c1.Z - 1) return new HexCoordinates(c1.X.Next(), 0, c1.Z - 1);
            return new HexCoordinates(c1.X, c1.Y, c1.Z - 1);
        }
        // direction 3
        if (dir == c1.X.Next().Next()) {
            if (c1.Y + 1 == c1.Z) return new HexCoordinates(c1.X.Next(), 0, c1.Z);
            return new HexCoordinates(c1.X, c1.Y + 1, c1.Z);
        }
        // direction 4
        if (dir == c1.X.Previous()) {
            if (c1.Y == 0) return new HexCoordinates(c1.X.Previous(), c1.Z, c1.Z + 1);
            return new HexCoordinates(c1.X, c1.Y - 1, c1.Z);
        }
        // direction 5
        if (dir == c1.X.Previous().Previous()) {
            if (c1.Y == 0) return new HexCoordinates(c1.X.Previous(), c1.Z - 1, c1.Z);
            return new HexCoordinates(c1.X, c1.Y - 1, c1.Z - 1);
        }
        // direction 6
        if (dir == c1.X.Next()) {
            return new HexCoordinates(c1.X, c1.Y + 1, c1.Z + 1);
        }
        // never actually reaches here.
        throw new System.NotImplementedException();
    }
}