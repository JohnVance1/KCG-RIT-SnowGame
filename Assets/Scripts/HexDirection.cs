/**
 * @author  Lingxiao Yu
 * @github  http://github.com/KHN190
 */
public enum HexDirection
{
    N, NE, SE, S, SW, NW
}

public static class HexDirectionExtensions
{
    public static HexDirection Opposite(this HexDirection direction)
    {
        return (int)direction < 3 ? (direction + 3) : (direction - 3);
    }

    public static HexDirection Previous(this HexDirection direction)
    {
        return direction == HexDirection.N ? HexDirection.NW : (direction - 1);
    }

    public static HexDirection Next(this HexDirection direction)
    {
        return direction == HexDirection.NW ? HexDirection.N : (direction + 1);
    }

    public static string ToString(this HexDirection direction)
    {
        return direction switch
        {
            HexDirection.N => "N",
            HexDirection.NE => "NE",
            HexDirection.SE => "SE",
            HexDirection.S => "S",
            HexDirection.SW => "SW",
            HexDirection.NW => "NW",
            _ => "",
        };
    }
}