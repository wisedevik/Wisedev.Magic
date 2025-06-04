using Wisedev.Magic.Titan.Math;

namespace Wisedev.Magic.Logic.GameObject.Component;

public class LogicMovementSystem
{
    private LogicVector2 _position; // 40
    private List<LogicVector2> _wayPoints; // 60
    private LogicVector2 _pushBackStartPosition; // 20
    private LogicVector2 _pushBackEndPosition; // 24

    private LogicMovementComponent _movementComponent;

    private int _derection; // 36
    private int _speed; // 64
    private int _boostTime; // 76
    private int _boostSpeed; // 80
    private int _slowSpeed; // 0?

    public LogicMovementSystem(LogicMovementComponent movementComponent, int speed)
    {
        _position = new LogicVector2();
        _wayPoints = new List<LogicVector2>();
        _movementComponent = movementComponent;
        _speed = 16 * speed / 1000;
    }

    public void Boost(int speed, int time)
    {
        if (speed <= -1)
        {
            _slowSpeed = (int)(1374389535L * Math.Max(-100, speed) * _speed >> 32);
            speed = (_slowSpeed >> 5) + (_slowSpeed >> 31);
        }
        _boostSpeed = speed;
        _boostTime = time;
    }
}
