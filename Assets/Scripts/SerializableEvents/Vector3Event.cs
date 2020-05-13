using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.SerializableEvents
{
    [Serializable]
    public class Vector3Event : UnityEvent<Vector3>
    {
        //Necessary as Unity can not serialize generics, therefore a UnityEvent with a generic type does not show up in the Inspector
    }
}

