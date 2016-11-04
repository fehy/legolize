#include "ConfigHandler.h"
#include "HeightMapHandler.h"
#include "Triplet.h"
#include "BrickBuilder.h"
#include "Wavefront.h"

int HeightMapProcess(int argc, char * argv[])
{
	// Prepare heightMapHandler
	HeightMapHandler heightMap;

	// Load BMP file
	bool loadSucceeded(heightMap.LoadBmpFile(argv[1]));
	if (!loadSucceeded)
	{
		std::cout << "File load failed" << std::endl;
		return 1;
	}
	else
		std::cout << "File loaded" << std::endl;

	// Save triangle file
	bool saveSucceeded(heightMap.SaveTriangleFile(argv[2], (argc>3 && !strcmp(argv[3], "HiDef"))));
	if (!saveSucceeded)
	{
		std::cout << "File save failed" << std::endl;
		return 1;
	}
	
	std::cout << "File saved" << std::endl;
	return 0;
}

int BrickProcess(int argc, char * argv[])
{
	bool saveSucceeded(BrickBuilder::BuildBrick(argv[1], 
		Triplet(atof(argv[2]), atof(argv[3]), atof(argv[4])),
		Triplet(atof(argv[5]), atof(argv[6]), atof(argv[7]))));
	if (!saveSucceeded)
	{
		std::cout << "File save failed" << std::endl;
		return 1;
	}

	std::cout << "File saved" << std::endl;
	return 0;
}

int WavefrontProcess(int argc, char * argv[])
{
	bool saveSucceeded(Wavefront::Convert(argv[1], argv[2],
		Triplet(atof(argv[3]), atof(argv[4]), atof(argv[5])),
		Triplet(atof(argv[6]), atof(argv[7]), atof(argv[8]))));
	if (!saveSucceeded)
	{
		std::cout << "File save failed" << std::endl;
		return 1;
	}

	std::cout << "File saved" << std::endl;
	return 0;
}

int main(int argc, char * argv[])
{
   // Process config
   ConfigHandler::ProcessConfig("HeightMapLoader.conf");

   // Invalid arguments, print usage
   if (argc!=3 && argc!=4 && argc!=8  && argc!=9)
   {
      std::cout << "Usage standard: " << std::endl;
      std::cout << "program.exe <inputFile_heightMap> <outputFile_triangleFile>" << std::endl;
      std::cout << "Usage High Definition: " << std::endl;
      std::cout << "program.exe <inputFile_heightMap> <outputFile_triangleFile> HiDef" << std::endl;
	  std::cout << "Usage brick: " << std::endl;
	  std::cout << "program.exe <outputFile_triangleFile> X Y Z R G B" << std::endl;
	  std::cout << "Usage wavefront: " << std::endl;
	  std::cout << "program.exe <input_wavefront> <outputFile_triangleFile> X Y Z R G B" << std::endl;
      return 1;
   }

   if (argc == 8)
	   return BrickProcess(argc, argv);
   
   if (argc == 9)
	   return WavefrontProcess(argc, argv);

   return HeightMapProcess(argc, argv);
}