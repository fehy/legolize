#include "WavefrontMaster.h"
#include <assert.h>
#include <fstream>

bool WavefrontMaster::DUMP_WAVEFRONT_SCENE(false);
std::string WavefrontMaster::WAVEFRONT_FILE;

void WavefrontMaster::DropFile(std::vector<GLfloat> const & vertex, std::vector<GLfloat> const & colors)
{
	if (!DUMP_WAVEFRONT_SCENE)
		return;

	std::fstream file(WAVEFRONT_FILE.c_str(), std::ios::binary | std::ios::out | std::ios::trunc);
	if (file.bad())
	{
		file.close();
		return;
	}
	
	for (auto i(0U); i < vertex.size(); i += 3)
		file << "v " << vertex[i] << " " << vertex[i + 1]<< " " << vertex[i + 2] << std::endl;

	for (auto i(0U); i < vertex.size() / 9; ++i)
	{
		auto i3(i*3);
		file << "f " << i3 + 1 << " " << i3 + 3 << " " << i3 + 2 << std::endl;
	}

	if (!file.good())
		std::cout << "Dump Wavefront failed in the middle" << std::endl;
	file.close();
}