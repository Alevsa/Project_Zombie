using System;

interface IActive {

    int initiative { get; set; }

    void PushToController();
}
