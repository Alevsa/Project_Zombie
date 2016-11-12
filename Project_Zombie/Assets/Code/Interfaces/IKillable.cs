using UnityEngine;

interface IKillable {

    int health { get; set; }
    
    //Dead or Alive basically
    int state { get; set; }

    void Die();

}
