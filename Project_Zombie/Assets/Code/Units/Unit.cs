using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour, IKillable , IActive , IMovable{

    #region IKillable
    private int _health;
    private int _state;

    public int health
    {
        get
        {
            return _health;
        }

        set
        {
            _health = value;
        }
    }

    public int state
    {
        get
        {
            return _state;
        }

        set
        {
            _state = value;
        }
    }

    public void Die()
    {

    }
    #endregion

    #region IActive

    private int _initiative;

    public int initiative
    {
        get
        {
            return _initiative;
        }

        set
        {
            _initiative = value;
        }
    }
    #endregion

    #region IMovable
    private int _distance;

    public int distance
    {
        get
        {
            return _distance;
        }

        set
        {
            _distance = value;
        }
    }

    public void Move()
    {

    }
    #endregion
}
