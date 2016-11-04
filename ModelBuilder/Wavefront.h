#ifndef _Wavefront_h_
#define _Wavefront_h_

struct Triplet;

struct Wavefront
{
	static bool Convert(char const * inObjFileName, char const * outModelFileName, 
		Triplet const & dimension, Triplet const & color);
};

#endif