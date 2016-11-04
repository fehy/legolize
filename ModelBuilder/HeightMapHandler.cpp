#include "HeightMapHandler.h"
#include <fstream>
#include "Triplet.h"

float HeightMapHandler::WIDTH_MIN(-1.0f);    //< Left
float HeightMapHandler::WIDTH_MAX(1.0f);     //< Right
float HeightMapHandler::HEIGHT_MIN(-1.0f);   //< Top
float HeightMapHandler::HEIGHT_MAX(1.0f);    //< Bottom
float HeightMapHandler::DEPTH_MIN(-0.5f);    //< Near
float HeightMapHandler::DEPTH_MAX(1.0f);     //< Far

HeightMapHandler::HeightMapHandler() : maxDepth(0x800000), minDepth(0x7FFFFF), coreData(0)
{}

void HeightMapHandler::setMaxMin()
{
	// crawl all pixels
	for (unsigned char const * pnter(coreData); pnter != coreData + infoHeader.biSizeImage;)
	{
		// Crawl all bits per pixel
		int curVal(extractValue(pnter));

		// New minimum
		if (curVal < minDepth)
			minDepth = curVal;

		// New maximum
		if (curVal > maxDepth)
			maxDepth = curVal;
	}
}

Triplet COLORS[] =
{
	Triplet(0.7f, 0, 0),
	Triplet(0, 0.7f, 0),
	Triplet(0, 0, 0.7f),
};

int ITER = 0;

bool HeightMapHandler::storeTriangle(std::ostream & stream, Triplet const & a, Triplet const & b, Triplet const & c) const
{
	++ITER;
	ITER %= 3;

	a.Serialize(stream);
	COLORS[ITER].Serialize(stream);

	b.Serialize(stream);
	COLORS[ITER].Serialize(stream);

	c.Serialize(stream);
	COLORS[ITER].Serialize(stream);

	// return success state
	return stream.good();
}

int HeightMapHandler::extractValue(unsigned char const * & firstByte) const
{
	// Crawl all bits per pixel
	int curVal(0);
	for (int i(0); i < infoHeader.biBitCount >> 3; ++i)
	{
		curVal <<= 8;
		curVal |= *firstByte;
		++firstByte;
	}

	return curVal;
}

int HeightMapHandler::extractValue(int x, int y) const
{
	unsigned char const * firstByte(coreData + (x + y*infoHeader.biWidth) * (infoHeader.biBitCount >> 3));
	return extractValue(firstByte);
}

Triplet HeightMapHandler::normalizeValues(int x, int y, int z) const
{
	return Triplet(
		(x * (WIDTH_MAX - WIDTH_MIN) / (infoHeader.biWidth * 2)) + WIDTH_MIN,
		(y * (HEIGHT_MAX - HEIGHT_MIN) / (infoHeader.biHeight * 2)) + HEIGHT_MIN,
		((z - minDepth) * (DEPTH_MAX - DEPTH_MIN) / (maxDepth - minDepth)) + DEPTH_MIN);
}

bool HeightMapHandler::LoadBmpFile(char const * fileName)
{
	// Data allready loaded
	if (coreData)
		return false;

	// Open file
	std::fstream file(fileName, std::ios::binary | std::ios::in);
	if (file.fail())
	{
		file.close();
		return false;
	}

	// read file type header
	file.read((char *)&fileHeader, sizeof(fileHeader));
	if (file.fail() || fileHeader.bfType != 0x4D42)
	{
		file.close();
		return false;
	}

	// read data header
	file.read((char *)&infoHeader, sizeof(infoHeader));
	if (file.fail())
	{
		file.close();
		return false;
	}

	// Move to data position
	file.seekg(fileHeader.bfOffBits, std::ios_base::beg);
	if (file.fail())
	{
		file.close();
		return false;
	}

	// Line size unalligned in bytes
	unsigned lineSize(infoHeader.biWidth * (infoHeader.biBitCount >> 3));

	// Correct image size (can be set to 0 according to norm, stupid) also kick off alignement
	infoHeader.biSizeImage = lineSize * infoHeader.biHeight;

	// Allocate space
	coreData = new unsigned char[infoHeader.biSizeImage];

	// compute alligned width
	LONG alignement((4 - (lineSize % 4)) % 4);

	// Read image itself
	char * pos((char *)coreData);
	for (int y(0); y < infoHeader.biHeight; ++y)
	{
		file.read(pos, lineSize);
		pos += lineSize;
		file.seekg(alignement, std::ios_base::cur);

		if (file.fail())
		{
			file.close();
			delete[] coreData;
			return false;
		}
	}

	// Close file
	file.close();

	// Obtain values for normalization
	setMaxMin();

	// All seems OK
	return true;
}

bool HeightMapHandler::SaveTriangleFile(char const * fileName, bool hiDef)
{
	// height map not loaded
	if (!coreData)
		return false;

	// Open file
	std::fstream file(fileName, std::ios::binary | std::ios::out | std::ios::trunc);
	if (file.bad())
	{
		file.close();
		return false;
	}

	// Work all pixels
	bool success(true);
	int byteDepth(infoHeader.biBitCount / 8);
	for (int y(0); y < infoHeader.biHeight - 1; ++y)
	{
		for (int x(0); x < infoHeader.biWidth - 1; ++x)
		{
			auto pntTL(extractValue(x, y));
			auto pntTR(extractValue(x + 1, y));
			auto pntBL(extractValue(x, y + 1));
			auto pntBR(extractValue(x + 1, y + 1));
			auto pntMM((pntTL + pntTR + pntBL + pntBR) / 4);

			auto TL(normalizeValues(2 * x, 2 * y, pntTL));
			auto TR(normalizeValues(2 * x + 2, 2 * y, pntTR));
			auto BL(normalizeValues(2 * x, 2 * y + 2, pntBL));
			auto BR(normalizeValues(2 * x + 2, 2 * y + 2, pntBR));
			auto MM(normalizeValues(2 * x + 1, 2 * y + 1, pntMM));

			if (hiDef)
			{
				success &= storeTriangle(file, TL, TR, MM);
				success &= storeTriangle(file, TR, BR, MM);
				success &= storeTriangle(file, BR, BL, MM);
				success &= storeTriangle(file, BL, TL, MM);
			}
			else
			{
				success &= storeTriangle(file, BR, BL, TL);
				success &= storeTriangle(file, TL, TR, BR);
			}
		}
	}

	// Close file
	file.close();

	// All seems OK
	return success;
}

HeightMapHandler::~HeightMapHandler()
{
	delete[] coreData;
}