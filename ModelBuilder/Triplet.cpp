#include "Triplet.h"

Triplet::Triplet(float x, float y, float z) : X(x), Y(y), Z(z)
{}

Triplet Triplet::operator+(float modifier) const
{
	return *this + Triplet(modifier, modifier, modifier);
}

Triplet Triplet::operator+(Triplet const & modifier) const
{
	return Triplet(X + modifier.X, Y + modifier.Y, Z + modifier.Z);
}

Triplet Triplet::operator*(float modifier) const
{
	return *this * Triplet(modifier, modifier, modifier);
}

Triplet Triplet::operator*(Triplet const & modifier) const
{
	return Triplet(X * modifier.X, Y * modifier.Y, Z * modifier.Z);
}

void Triplet::Serialize(std::ostream & stream) const
{
	stream.write((char*)&X, sizeof(float));
	stream.write((char*)&Y, sizeof(float));
	stream.write((char*)&Z, sizeof(float));
}

