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
	float rot = rotation / 180.0f * 3.141592654f; // Degree to Radian

	// XMatrix
	/*float rotMatrix[] =
	{
		1,0,0,0,
		0,cos(rot),-sin(rot),0,
		0,sin(rot),cos(rot),0,
		0,0,0,1
	};*/

	// YMatrix
	/*float rotMatrix[] =
	{
		cos(rot), 0, sin(rot), 0,
		0, 1, 0, 0,
		-sin(rot), 0, cos(rot), 0,
		0, 0, 0, 1
	};*/

	// ZMatrix
	float rotMatrix[] =
	{
		cos(rot),-sin(rot), 0, 0,
		sin(rot), cos(rot), 0, 0,
		0, 0, 1, 0,
		0, 0, 0, 1
	};

	float afterRot[3];

	afterRot[0] = (X * rotMatrix[0] + Y * rotMatrix[4] + Z * rotMatrix[8] + rotMatrix[12]);
	afterRot[1] = (X * rotMatrix[1] + Y * rotMatrix[5] + Z * rotMatrix[9] + rotMatrix[13]);
	afterRot[2] = (X * rotMatrix[2] + Y * rotMatrix[6] + Z * rotMatrix[10] + rotMatrix[14]);

	target.push_back(afterRot[0] + offset.X);
	target.push_back(afterRot[1] + offset.Y);
	target.push_back(afterRot[2] + offset.Z);
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