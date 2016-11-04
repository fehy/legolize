#ifndef _WavefrontMaster_h_
#define _WavefrontMaster_h_

#include <string>
#include <vector>
#include "Triangles.h"

struct WavefrontMaster
{
	static bool DUMP_WAVEFRONT_SCENE;
	static std::string WAVEFRONT_FILE;

	static void DropFile(std::vector<GLfloat> const & vertex, std::vector<GLfloat> const & colors);
};

#endif