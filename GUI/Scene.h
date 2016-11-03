#ifndef _Scene_h_
#define _Scene_h_

class Scene;

#include <string>
#include <vector>
#include <istream>
#include "CommonClassMacros.h"
#include "OpenGL.h"
#include "Triangles.h"
#include "SceneItem.h"

class Scene
{
	static HANDLE reloadThread;
	static volatile bool reloadThreadRunning;
	static unsigned _stdcall realoadSceneHandler(void * instance);

	static void reloadScene(std::vector<SceneItem> & items, std::vector<GLfloat> & vertex, std::vector<GLfloat> & colors);

	/// \return success
	static bool reloadItems(std::vector<SceneItem> & items);

	public:
		static long SCENE_PERIOD;
		static std::string SceneFile;

		static void StartHandler();
		static void StopHandler();
};

#endif