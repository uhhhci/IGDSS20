using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CameraSettings", order = 1)]
    public class CameraSettings : ScriptableObject
    {
        public float MoveSpeed;
        public float ZoomSpeed;
    }
}
