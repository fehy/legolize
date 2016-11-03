#ifndef _BrickBuilder_h_
#define _BrickBuilder_h_

struct Triplet;

#include <iostream>

class BrickBuilder
{
	static void storeTriangle(std::ostream & stream, 
		Triplet const & a, Triplet const & b, Triplet const & c, Triplet const & color);

	static void storeFace(std::ostream & stream,
		Triplet const & TL, Triplet const & TR, Triplet const & BR, Triplet const & BL, Triplet const & color);

	public:
		static bool BuildBrick(char const * fileName, Triplet const & dimension, Triplet const & color);
};

#endif