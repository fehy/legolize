#include "Triplet.h"

Triplet::Triplet(float x, float y, float z) : X(x), Y(y), Z(z)
{}

void Triplet::Serialize(std::ostream & stream) const
{
	stream.write((char*)&X, sizeof(float));
	stream.write((char*)&Y, sizeof(float));
	stream.write((char*)&Z, sizeof(float));
}

