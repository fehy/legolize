#include "SceneItem.h"

GLint readInt(std::istream & stream)
{
	GLint value;
	stream.read((char*)&value, sizeof(GLint));
	return value;
}

SceneItem::SceneItem(std::istream & stream)
	: Model(readInt(stream)), Offset(stream), Rotation(readInt(stream))
{}