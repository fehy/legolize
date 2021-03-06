#ifndef _Model_h_
#define _Model_h_

#include <vector>
#include <istream>
#include "CommonClassMacros.h"
#include "OpenGL.h"
#include "Triangles.h"

struct Model
{
	DEFAULT_COPYMOVE(Model);
	DEFAULT_DESTROY(Model);

	static std::vector<Model> Models;

	std::vector<Triangle> const Triangles;

	Model(std::istream & stream);

	void PrintVertexPositions(std::vector<GLfloat> & target, PntVector const & offset, GLint rotation) const;
	void PrintVertexColors(std::vector<GLfloat> & target) const;
};

#endif