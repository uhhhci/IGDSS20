using System;
using UnityEngine;

[Serializable]
public class Boundaries
{
	public Vector3 LowerBoundaries { get; private set;}
	public Vector3 UpperBoundaries { get; private set;}

	public Boundaries(Vector3 lowerBoundaries, Vector3 upperBoundaries)
	{
		LowerBoundaries = lowerBoundaries;
		UpperBoundaries = upperBoundaries;
	}

	public override string ToString()
	{
		return $"{LowerBoundaries} - {UpperBoundaries}";
	}


	public Vector3 ClampToBoundaries(Vector3 value)
	{
		return new Vector3(
			x: Mathf.Clamp(value.x,LowerBoundaries.x,UpperBoundaries.x),
			y: Mathf.Clamp(value.y, LowerBoundaries.y, UpperBoundaries.y),
			z: Mathf.Clamp(value.z, LowerBoundaries.z, UpperBoundaries.z)
			);
	}
}