#ifndef _Triplet_h_
#define _Triplet_h_

#include <iostream>

struct Triplet
{
	float X;
	float Y;
	float Z;

	Triplet(float x, float y, float z);

	Triplet operator+(float modifier) const;
	Triplet operator+(Triplet const & modifier) const;

	Triplet operator*(float modifier) const;
	Triplet operator*(Triplet const & modifier) const;

	void Serialize(std::ostream & stream) const;
};

#endif