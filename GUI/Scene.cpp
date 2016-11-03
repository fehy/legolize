#include "Scene.h"
#include <assert.h>
#include <process.h>
#include <sstream>
#include <fstream>
#include "Model.h"

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
			reloadScene(items, vertex, colors);

		Sleep(SCENE_PERIOD);
	}
	return 0;
}

long Scene::SCENE_PERIOD(15);
std::string Scene::SceneFile;

void Scene::reloadScene(std::vector<SceneItem> & items, std::vector<GLfloat> & vertex, std::vector<GLfloat> & colors)
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

	OpenGL::SwapInputBuffers(vertex, colors);
}

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
	stream.close();
	return success;
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