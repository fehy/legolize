#include "Scene.h"
#include <assert.h>
#include <process.h>
#include <iostream>
#include <sstream>
#include <fstream>
#include "Model.h"
#include "WavefrontMaster.h"

HANDLE Scene::reloadThread(0);
volatile bool Scene::reloadThreadRunning(false);

unsigned _stdcall Scene::realoadSceneHandler(void * instance)
{
	std::vector<GLfloat> vertex;
	std::vector<GLfloat> colors;
	std::vector<SceneItem> items;

	while (reloadThreadRunning)
	{
		if (reloadItems(items))
		{
			reloadScene(items, vertex, colors);
			centerScene(vertex);
			WavefrontMaster::DropFile(vertex, colors);

			OpenGL::SwapInputBuffers(vertex, colors);
		}

		Sleep(SCENE_PERIOD);
	}
	return 0;
}

long Scene::SCENE_PERIOD(15);
bool DUMP_WAVEFRONT_SCENE(false);
std::string Scene::SceneFile;

bool Scene::reloadItems(std::vector<SceneItem> & items)
{
	std::fstream stream(SceneFile.c_str(), std::ios::in | std::ios::binary);
	if (!stream)
		return false;

	stream.seekg(0, std::ios::end);
	std::streamoff size(stream.tellg());
	stream.seekg(0, std::ios::beg);
	unsigned int records((unsigned int)(size / sizeof(SceneItem)));

	items.clear();
	items.reserve(records);

	for (auto i(0U); i < records && stream.good(); ++i)
		items.push_back(SceneItem(stream));

	auto success(stream.good());
	
	if (!success)
		std::cout << "LoadScene failed in the middle" << std::endl;

	static unsigned lastItems(0U);
	if (items.size() < lastItems)
		std::cout << "LoadScene itemsDrop from " << lastItems << " to " << items.size() << std::endl;
	lastItems = items.size();

	stream.close();
	_unlink(SceneFile.c_str());
	return success;
}

void Scene::reloadScene(std::vector<SceneItem> const & items, std::vector<GLfloat> & vertex, std::vector<GLfloat> & colors)
{
	vertex.clear();
	colors.clear();
	
	unsigned trianglesEstimate(0);
	for (auto item : items)
		trianglesEstimate += Model::Models[item.Model].Triangles.size();

	vertex.reserve(trianglesEstimate * 9); // X,Y,Z × A,B,C
	colors.reserve(trianglesEstimate * 9); // R,G,B × A,B,C

	for (auto item : items)
	{
		auto & model(Model::Models[item.Model]);

		model.PrintVertexPositions(vertex, item.Offset, item.Rotation);
		model.PrintVertexColors(colors);
	}
}

void Scene::centerScene(std::vector<GLfloat> & vertex)
{
	assert(vertex.size() % 3 == 0);
	static GLfloat extremeX = 0;
	static GLfloat extremeY = 0;
	static GLfloat extremeZ = 0;

	auto cpnter(vertex.cbegin());
	while (cpnter != vertex.cend())
	{
		if (*cpnter > extremeX)
			extremeX = *cpnter;

		if (*++cpnter > extremeY)
			extremeY = *cpnter;

		if (*++cpnter > extremeZ)
			extremeZ = *cpnter;
		++cpnter;
	}

	extremeX *= 0.5f;
	extremeY *= 0.5f;
	extremeZ *= 0.5f;

	auto pnter(vertex.begin());
	while (pnter != vertex.end())
	{
		*pnter -= extremeX;
		*++pnter -= extremeY;
		*++pnter -= extremeZ;
		++pnter;
	}
}

void Scene::StartHandler()
{
	if (reloadThreadRunning)
		return;

	reloadThreadRunning = true;

	assert(!reloadThread);
	reloadThread = (HANDLE)_beginthreadex(NULL, 0, realoadSceneHandler, 0, 0, NULL);
}

void Scene::StopHandler()
{
	if (!reloadThreadRunning)
		return;
	reloadThreadRunning = false;

	WaitForSingleObject(reloadThread, 20*1000);

	CloseHandle(reloadThread);
	reloadThread = 0;
}