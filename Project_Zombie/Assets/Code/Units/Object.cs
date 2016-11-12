using UnityEngine;
using System.Collections;

public class Object : MonoBehaviour, IKillable {

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
}
