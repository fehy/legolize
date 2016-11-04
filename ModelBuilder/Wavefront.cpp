#include "Wavefront.h"
#include <assert.h>
#include <iostream>
#include <fstream>
#include <sstream>
#include <vector>
#include "Triplet.h"

bool loadFile(char const * file, std::vector<Triplet> & vertex, std::vector<unsigned> & faces)
{
	vertex.clear();
	faces.clear();
	std::fstream stream(file, std::ios::in | std::ios::binary);
	if (!stream)
	{
		std::cout << "Open File Failed" << std::endl;
		return false;
	}


	std::string oneLine;
	while (std::getline(stream, oneLine))
	{
		// extract name
		std::stringstream lineCont(oneLine);
		lineCont >> oneLine;

		if (oneLine == "v")
		{
			float x;
			float y;
			float z;
			lineCont >> x;
			lineCont >> y;
			lineCont >> z;
			vertex.push_back(Triplet(x,y,z));
		}

		if (oneLine == "f")
		{
			int a;
			int b;
			int c;
			lineCont >> a;
			lineCont >> b;
			lineCont >> c;
			faces.push_back(a-1);
			faces.push_back(b-1);
			faces.push_back(c-1);
		}
	}

	stream.close();
	return true;
}

void normalize(std::vector<Triplet> & vertex, Triplet dimension)
{
	float maxX(vertex[0].X);
	float maxY(vertex[0].Y);
	float maxZ(vertex[0].Z);

	float minX(vertex[0].X);
	float minY(vertex[0].Y);
	float minZ(vertex[0].Z);

	for (auto const triplet : vertex)
	{
		if (triplet.X > maxX)
			maxX = triplet.X;
		if (triplet.Y > maxY)
			maxY = triplet.Y;
		if (triplet.Z > maxZ)
			maxZ = triplet.Z;

		if (triplet.X < minX)
			minX = triplet.X;
		if (triplet.Y < minY)
			minY = triplet.Y;
		if (triplet.Z < minZ)
			minZ = triplet.Z;
	}

	Triplet offset(minX, minY, minZ);
	Triplet scale(dimension.X / (maxX - minX), dimension.Y / (maxY - minY), dimension.Z / (maxZ - minZ));

	for (auto & triplet : vertex)
	{
		triplet.X = (triplet.X - minX) * scale.X;
		triplet.Y = (triplet.Y - minY) * scale.Y;
		triplet.Z = (triplet.Z - minZ) * scale.Z;
	}
}

bool writeFile(char const * fileName, std::vector<Triplet> const & vertex, std::vector<unsigned> const & faces, Triplet const & color)
{
	std::fstream file(fileName, std::ios::binary | std::ios::out | std::ios::trunc);
	if (file.bad())
	{
		file.close();
		std::cout << "Cannot Open Target file" << std::endl;
		return false;
	}

	for (auto i(0U); i < faces.size() / 3; ++i)
	{
		auto i3(i * 3);

		vertex[faces[i3]].Serialize(file);
		color.Serialize(file);

		vertex[faces[i3 + 1]].Serialize(file);
		(color * 0.9f).Serialize(file);

		vertex[faces[i3 + 2]].Serialize(file);
		(color * 0.8f).Serialize(file);
	}

	auto success(file.good());

	if (!success)
		std::cout << "Serialization failed in the middle" << std::endl;

	file.close();
	return success;
}

bool Wavefront::Convert(char const * inObjFileName, char const * outModelFileName,
	Triplet const & dimension, Triplet const & color)
{
	std::vector<Triplet> vertex;
	std::vector<unsigned> faces;

	if (!loadFile(inObjFileName, vertex, faces))
		return false;

	normalize(vertex, dimension);

	return writeFile(outModelFileName, vertex, faces, color);
}