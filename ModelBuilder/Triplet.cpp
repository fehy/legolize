#include "Triplet.h"

Triplet Triplet::operator-(Triplet const & other) const
{
	return Triplet(this->X - other.X, this->Y - other.Y, this->Z - other.Z);
}

Triplet Triplet::CrossProduct(Triplet const & left, Triplet const & right)
{
	return Triplet(
		left.Y*right.Z - left.Z*right.Y,
		left.Z*right.X - left.X*right.Z,
		left.X*right.Y - left.Y*right.X);
}

float Triplet::DotProduct(Triplet const & left, Triplet const & right)
{
	return left.X*right.X + left.Y*right.Y + left.Z*right.Z;
}

Triplet Triplet::ComputeNormal(Triplet const & a, Triplet const & b, Triplet const & c)
{
	// obtain normal
	Triplet normal(Triplet::CrossProduct(b - a, c - a));

	// normalize
	float const length(sqrt(Triplet::DotProduct(normal, normal)));
	normal.X /= length;
	normal.Y /= length;
	normal.Z /= length;

	return normal;
}

Triplet::Triplet(float x, float y, float z) : X(x), Y(y), Z(z)
{}

void Triplet::Serialize(std::ostream & stream) const
{
	stream.write((char*)&X, sizeof(float));
	stream.write((char*)&Y, sizeof(float));
	stream.write((char*)&Z, sizeof(float));
}

