#ifndef _Triangles_h_
#define _Triangles_h_

struct PntVector;
struct Vertice;
struct Triangle;

#include <vector>
#include <istream>
#include "CommonClassMacros.h"
#include "OpenGL.h"

struct PntVector
{
	DEFAULT_COPYMOVE(PntVector);
	DEFAULT_DESTROY(PntVector);

	GLfloat X;
	GLfloat Y;
	GLfloat Z;

	PntVector(GLfloat x, GLfloat y, GLfloat z);

	// [X],[Y],[Z]
	PntVector(std::istream & stream);

	void Print(std::vector<GLfloat> & target) const;
	void Print(std::vector<GLfloat> & target, PntVector const & offset, GLint rotation) const;
};

struct Vertice
{
	DEFAULT_COPYMOVE(Vertice);
	DEFAULT_DESTROY(Vertice);

	PntVector const Position;
	PntVector const Color;

	// Pos.X, Pos.Y, Pos.Z, Col.R, Col.G, Col.B
	Vertice(std::istream & stream);
};

struct Triangle
{
	DEFAULT_COPYMOVE(Triangle);
	DEFAULT_DESTROY(Triangle);

	Vertice const A;
	Vertice const B;
	Vertice const C;

	// A.X, A.Y, A.Z, A.R, A.G, A.B
	// B.X, B.Y, B.Z, B.R, B.G, B.B
	// C.X, C.Y, C.Z, C.R, C.G, C.B
	Triangle(std::istream & stream);

	void PrintVertexPosition(std::vector<GLfloat> & target, PntVector const & offset, GLint rotation);
	void PrintVertexColor(std::vector<GLfloat> & target);
};

#endif