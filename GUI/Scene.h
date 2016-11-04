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

	/// \return success
	static bool reloadItems(std::vector<SceneItem> & items);

	static void reloadScene(std::vector<SceneItem> const & items, 
		std::vector<GLfloat> & vertex, std::vector<GLfloat> & colors, std::vector<GLfloat> & normals);

	static void centerScene(std::vector<GLfloat> & vertex);

	public:
		static long SCENE_PERIOD;
		static std::string SceneFile;

		static void StartHandler();
		static void StopHandler();
};

#endif