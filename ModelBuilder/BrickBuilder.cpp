#include "BrickBuilder.h"
#include <fstream>
#include "Triplet.h"

void BrickBuilder::storeTriangle(std::ostream & stream, 
	Triplet const & a, Triplet const & b, Triplet const & c,
	Triplet const & color) 
{
	a.Serialize(stream);
	color.Serialize(stream);

	b.Serialize(stream);
	color.Serialize(stream);

	c.Serialize(stream);
	color.Serialize(stream);
}

void BrickBuilder::storeFace(std::ostream & stream,
	Triplet const & TL, Triplet const & TR, Triplet const & BR, Triplet const & BL, Triplet const & color)
{
	storeTriangle(stream, TL, TR, BR, color);
	storeTriangle(stream, BR, BL, TL, color * 0.9f);
}

void BrickBuilder::nearFace(std::ostream & stream, Triplet const & low, Triplet const & high, Triplet const & color)
{
	storeFace(stream,
		Triplet(low.X, high.Y, high.Z), 
		Triplet(high.X, high.Y, high.Z), 
		Triplet(high.X, low.Y, high.Z), 
		Triplet(low.X, low.Y, high.Z), 
		color);
}

void BrickBuilder::farFace(std::ostream & stream, Triplet const & low, Triplet const & high, Triplet const & color)
{
	storeFace(stream,
		Triplet(high.X, high.Y, low.Z), 
		Triplet(low.X, high.Y, low.Z), 
		Triplet(low.X, low.Y, low.Z), 
		Triplet(high.X, low.Y, low.Z), 
		color);
}

void BrickBuilder::topFace(std::ostream & stream, Triplet const & low, Triplet const & high, Triplet const & color)
{
	storeFace(stream,
		Triplet(low.X, high.Y, low.Z),
		Triplet(high.X, high.Y, low.Z), 
		Triplet(high.X, high.Y, high.Z), 
		Triplet(low.X, high.Y, high.Z), 
		color);
}

void BrickBuilder::bottomFace(std::ostream & stream, Triplet const & low, Triplet const & high, Triplet const & color)
{
	storeFace(stream,
		Triplet(low.X, low.Y, high.Z), 
		Triplet(high.X, low.Y, high.Z), 
		Triplet(high.X, low.Y, low.Z),
		Triplet(low.X, low.Y, low.Z), 
		color);
}

void BrickBuilder::leftFace(std::ostream & stream, Triplet const & low, Triplet const & high, Triplet const & color)
{
	storeFace(stream,
		Triplet(low.X, high.Y, low.Z),
		Triplet(low.X, high.Y, high.Z), 
		Triplet(low.X, low.Y, high.Z), 
		Triplet(low.X, low.Y, low.Z), 
		color);
}

void BrickBuilder::rightFace(std::ostream & stream, Triplet const & low, Triplet const & high, Triplet const & color)
{
	storeFace(stream,
		Triplet(high.X, high.Y, high.Z), 
		Triplet(high.X, high.Y, low.Z), 
		Triplet(high.X, low.Y, low.Z),
		Triplet(high.X, low.Y, high.Z), 
		color);
}

bool BrickBuilder::BuildBrick(char const * fileName, Triplet const & dimension, Triplet const & color)
{
	std::fstream file(fileName, std::ios::binary | std::ios::out | std::ios::trunc);
	if (file.bad())
	{
		file.close();
		return false;
	}

	// Core
	auto low(dimension * -0.5f + 0.05f);
	auto high(dimension * 0.5f + -0.05f);
	nearFace(file, low, high, color);
	farFace(file, low, high, color);
	topFace(file, low, high, color);
	bottomFace(file, low, high, color*0.9f);
	leftFace(file, low, high, color);
	rightFace(file, low, high, color);

	// Topper
	float topperGap(0.1f);
	auto topperColor(color * 0.7f);
	Triplet topperLow(low.X + topperGap, low.Y + topperGap, high.Z);
	Triplet topperHigh(high.X - topperGap, high.Y - topperGap, high.Z + topperGap);
	nearFace(file, topperLow, topperHigh, topperColor);
	farFace(file, topperLow, topperHigh, topperColor);
	topFace(file, topperLow, topperHigh, topperColor);
	leftFace(file, topperLow, topperHigh, topperColor);
	rightFace(file, topperLow, topperHigh, topperColor);

	auto success(file.good());
	file.close();
	return success;
}