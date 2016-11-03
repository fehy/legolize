#ifndef _Triplet_h_
#define _Triplet_h_

#include <iostream>

struct Triplet
{
	float X;
	float Y;
	float Z;

	Triplet operator-(Triplet const & other) const;

	static Triplet CrossProduct(Triplet const & left, Triplet const & right);

	static float DotProduct(Triplet const & left, Triplet const & right);

	static Triplet ComputeNormal(Triplet const & a, Triplet const & b, Triplet const & c);

	Triplet(float x, float y, float z);

	void Serialize(std::ostream & stream) const;
};

#endif