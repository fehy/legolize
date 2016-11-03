#ifndef _SceneItem_h_
#define _SceneItem_h_

#include <iostream>
#include "CommonClassMacros.h"
#include "Triangles.h"
#include "OpenGL.h"

struct SceneItem
{
	DEFAULT_COPYMOVE(SceneItem);
	DEFAULT_DESTROY(SceneItem);
	
	GLint const Model;
	PntVector const Offset;
	GLint const Rotation;

	SceneItem(std::istream & stream);
};

#endif