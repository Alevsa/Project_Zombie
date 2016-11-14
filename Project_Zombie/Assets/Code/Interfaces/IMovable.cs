using UnityEngine;
using System.Collections;

interface IMovable {

    Tile tile { get; set; }
    int distance { get; set; }

    bool ignoreObstacles { get; set; }

}
