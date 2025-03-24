namespace Wisedev.Magic.Logic.Level;

public class LogicRect
{
    private int _startX;
    private int _startY;
    private int _endX;
    private int _endY;

    public LogicRect(int startX, int startY, int endX, int endY)
    {
        this._startX = startX;
        this._startY = startY;
        this._endX = endX;
        this._endY = endY;
    }

    public void Destruct()
    {
        ;
    }

    public int GetStartX()
    {
        return this._startX;
    }

    public int GetStartY()
    {
        return this._startY;
    }

    public int GetEndX()
    {
        return this._endX;
    }

    public int GetEndY()
    {
        return this._endY;
    }

    public bool IsInside(int x, int y)
    {
        return this._startX <= x && this._startY <= y && this._endX >= x && this._endY >= y;
    }
}
