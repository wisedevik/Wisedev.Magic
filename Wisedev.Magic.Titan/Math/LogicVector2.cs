using Wisedev.Magic.Titan.DataStream;

namespace Wisedev.Magic.Titan.Math;

public class LogicVector2
{
    private int _x;
    private int _y;


    public LogicVector2()
    {
    }

    public LogicVector2(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public void Destruct()
    {
        _x = 0;
        _y = 0;
    }

    public void Add(LogicVector2 vector)
    {
        _x += vector._x;
        _y += vector._y;
    }

    public void Multiply(LogicVector2 vector2)
    {
        _x *= vector2._x;
        _y *= vector2._y;
    }

    public int GetDistanceSquaredTo(int x, int y)
    {
        int distance = 0x7FFFFFFF;

        x -= _x;

        if ((uint)(x + 46340) <= 92680)
        {
            y -= _y;

            if ((uint)(y + 46340) <= 92680)
            {
                int distanceX = x * x;
                int distanceY = y * y;

                if ((uint)distanceY < (distanceX ^ 0x7FFFFFFFu))
                {
                    distance = distanceX + distanceY;
                }
            }
        }

        return distance;
    }

    public void Substruct(LogicVector2 vector)
    {
        _x -= vector._x;
        _y -= vector._y;
    }

    public void SetX(int x)
    {
        _x = x;
    }

    public void SetY(int y)
    {
        _y = y;
    }

    public void Set(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public int Normalize(int value)
    {
        int len = GetDistance();

        if (len != 0)
        {
            _x = _x * value / len;
            _y = _y * value / len;
        }

        return len;
    }

    public int GetDistance()
    {
        int length = 0x7FFFFFFF;

        if ((uint)(46340 - _x) <= 92680)
        {
            if ((uint)(46340 - _y) <= 92680)
            {
                int lengthX = _x * _x;
                int lengthY = _y * _y;

                if ((uint)lengthY < (lengthX ^ 0x7FFFFFFFu))
                {
                    length = lengthX + lengthY;
                }
            }
        }

        return LogicMath.Sqrt(length);
    }

    public int GetAngle()
    {
        return LogicMath.GetAngle(_x, _y);
    }

    public void Encode(ByteStream stream)
    {
        stream.WriteInt(_x);
        stream.WriteInt(_y);
    }

    public void Decode(ByteStream stream)
    {
        _x = stream.ReadInt();
        _y = stream.ReadInt();
    }

    public int GetX()
    {
        return this._x;
    }

    public int GetY()
    {
        return this._y;
    }
}
