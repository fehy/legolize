#include "Triangles.h"
#include <math.h>
#include <assert.h>

GLfloat readFloat(std::istream & stream)
{
	GLfloat value;
	stream.read((char*)&value, sizeof(GLfloat));
	return value;
}

PntVector::PntVector(std::istream & stream)
	: X(readFloat(stream)), Y(readFloat(stream)), Z(readFloat(stream))
{}

void PntVector::Print(std::vector<GLfloat> & target) const
{
	target.push_back(X);
	target.push_back(Y);
	target.push_back(Z);
}

void PntVector::Print(std::vector<GLfloat> & target, PntVector const & offset, GLint rotation) const
{
	target.push_back(X);
	target.push_back(Y);
	target.push_back(Z);
}

Vertice::Vertice(std::istream & stream)
	: Position(stream), Color(stream)
{}

Triangle::Triangle(std::istream & stream)
	: A(stream), B(stream), C(stream), Normal(stream)
{}

void Triangle::PrintVertexPosition(std::vector<GLfloat> & target, PntVector const & offset, GLint rotation)
{
	A.Position.Print(target, offset, rotation);
	B.Position.Print(target, offset, rotation);
	C.Position.Print(target, offset, rotation);
}

void Triangle::PrintVertexColor(std::vector<GLfloat> & target)
{
	A.Color.Print(target);
	B.Color.Print(target);
	C.Color.Print(target);
}