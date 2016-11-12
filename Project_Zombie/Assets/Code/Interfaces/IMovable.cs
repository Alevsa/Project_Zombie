using UnityEngine;
using System.Collections;

interface IMovable {

    int distance { get; set; }

    bool ignoreObstacles { get; set; }

}
