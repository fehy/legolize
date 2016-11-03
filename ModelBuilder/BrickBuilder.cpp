#include "BrickBuilder.h"
#include <fstream>
#include "Triplet.h"

void BrickBuilder::storeTriangle(std::ostream & stream, 
	Triplet const & a, Triplet const & b, Triplet const & c,
	Triplet const & color) 
{
	// prepare normal
	Triplet const normal(Triplet::ComputeNormal(a, b, c));

	a.Serialize(stream);
	color.Serialize(stream);

	b.Serialize(stream);
	color.Serialize(stream);

	c.Serialize(stream);
	color.Serialize(stream);

	normal.Serialize(stream);
}

void BrickBuilder::storeFace(std::ostream & stream,
	Triplet const & TL, Triplet const & TR, Triplet const & BR, Triplet const & BL, Triplet const & color)
{
	storeTriangle(stream, TL, TR, BR, color);
	storeTriangle(stream, BR, BL, TL, color);
}

bool BrickBuilder::BuildBrick(char const * fileName, Triplet const & dimension, Triplet const & color)
{
	std::fstream file(fileName, std::ios::binary | std::ios::out | std::ios::trunc);
	if (file.bad())
	{
		file.close();
		return false;
	}

	float hi = 0.45f;
	float lo = -0.45f;

	Triplet TLN(dimension.X * lo, dimension.Y * hi, dimension.Z * hi);
	Triplet TRN(dimension.X * hi, dimension.Y * hi, dimension.Z * hi);
	Triplet BRN(dimension.X * hi, dimension.Y * lo, dimension.Z * hi);
	Triplet BLN(dimension.X * lo, dimension.Y * lo, dimension.Z * hi);

	Triplet TRF(dimension.X * hi, dimension.Y * hi, dimension.Z * lo);
	Triplet TLF(dimension.X * lo, dimension.Y * hi, dimension.Z * lo);
	Triplet BLF(dimension.X * lo, dimension.Y * lo, dimension.Z * lo);
	Triplet BRF(dimension.X * hi, dimension.Y * lo, dimension.Z * lo);

	storeFace(file, TLN, TRN, BRN, BLN, color); // Near
	storeFace(file, TRN, TRF, BRF, BRN, color); // Right
	storeFace(file, TLF, TLN, BLN, BLF, color); // Left
	storeFace(file, TRF, TLF, BLF, BRF, color); // Far
	storeFace(file, TLF, TRF, TRN, TLN, color); // Top

	auto success(file.good());
	file.close();
	return success;
}