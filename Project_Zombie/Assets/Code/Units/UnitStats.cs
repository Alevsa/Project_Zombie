﻿using UnityEngine;
using System.Collections;

public class UnitStats : MonoBehaviour, IKillable , IActive , IMovable{

    #region IKillable
    [Header("Health")]
    public int _health;
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

    public virtual void Die()
    {
        state = 0;
    }
    #endregion

    #region IActive
    [Header("Turn Order")]
    public int _initiative;

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
    [Header("Movement")]
    public int _distance;
    public bool _ignoreObstacles;

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

    public bool ignoreObstacles
    {
        get
        {
            return _ignoreObstacles;
        }

        set
        {
            _ignoreObstacles = value;
        }
    }
    #endregion
}
