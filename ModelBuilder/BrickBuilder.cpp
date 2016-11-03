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

	float hi = 0.5f;
	float lo = -0.5f;
	float gap = 0.00f;

	Triplet TLN(dimension.X * lo + gap, dimension.Y * hi - gap, dimension.Z * hi - gap);
	Triplet TRN(dimension.X * hi - gap, dimension.Y * hi - gap, dimension.Z * hi - gap);
	Triplet BRN(dimension.X * hi - gap, dimension.Y * lo + gap, dimension.Z * hi - gap);
	Triplet BLN(dimension.X * lo + gap, dimension.Y * lo - gap, dimension.Z * hi - gap);

	Triplet TRF(dimension.X * hi - gap, dimension.Y * hi - gap, dimension.Z * lo + gap);
	Triplet TLF(dimension.X * lo + gap, dimension.Y * hi - gap, dimension.Z * lo + gap);
	Triplet BLF(dimension.X * lo + gap, dimension.Y * lo + gap, dimension.Z * lo + gap);
	Triplet BRF(dimension.X * hi - gap, dimension.Y * lo + gap, dimension.Z * lo + gap);

	storeFace(file, TLN, TRN, BRN, BLN, color); // Near
	storeFace(file, TRN, TRF, BRF, BRN, color); // Right
	storeFace(file, TLF, TLN, BLN, BLF, color); // Left
	storeFace(file, TRF, TLF, BLF, BRF, color); // Far
	storeFace(file, TLF, TRF, TRN, TLN, color); // Top

	auto success(file.good());
	file.close();
	return success;
}