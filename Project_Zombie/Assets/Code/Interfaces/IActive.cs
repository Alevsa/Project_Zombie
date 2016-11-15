using System;

interface IActive {

    int initiativeGain { get; set; }
    int initiative { get; set; }

    void PushToController();
}
